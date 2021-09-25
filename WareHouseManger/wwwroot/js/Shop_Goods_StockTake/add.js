$(document).ready(function () {
    $('#btn-addshopgoods').click(function () {
        getShopGoods(getIds(), $('#select-category').val());

        $('#modal-shopgoods').modal('toggle');
    });

    $('#select-category').change(function () {
        getShopGoods(getIds(), $(this).val());
    });

    $('#btn-acceptshoppgoods').click(function () {
        var cbs = $('#tb-addshopgoods tbody').find('.cb');

        var html = '';

        for (var i = 0; i < cbs.length; i++) {
            var e = cbs[i];

            if ($(e).is(':checked')) {
                var tds = $($(e).parent().parent()).find('td');
                html += '<tr>' +
                    '<td>' + $(tds[0]).text() + '</td>' +
                    '<td>' + $(tds[1]).text() + '</td>' +
                    '<td>' + $('#select-category').text() + '</td>' +
                    '<td>' + $(tds[2]).text() + '</td>' +
                    '<td>' + $(tds[3]).text() + '</td>' +
                    '<td style="width:120px;"><input class="form-control text-right number" min="1" value="' + $(tds[4]).text() + '"  readonly/></td>' +
                    '<td style="width:120px;"><input class="form-control text-right number actual-amount" min="1" value="1"  /></td>' +
                    '<td style="width:120px;"><input class="form-control text-right number quantity-difference" min="1" value="1"  /></td>' +
                    '<td style="width:300px;"><textarea class="form-control" rows="5"></textarea></td>' +
                    '<td class="text-center" style="width:50px;"><button class="btn btn-sm btn-danger btn-remove">Xóa</button></td>' +
                    '</tr>';
            }
        }
        $('#tb-shopgoods tbody').append(html);

        getShopGoods(getIds(), $('#select-category').val());
    });

    $('body').on('click', '.btn-remove', function () {
        $($(this).parent().parent()).remove();
    });

    $('body').on('keyup', '.number', function () {
        $(this).val(formatNumber(formatDisplayNumberToNumber($(this).val())));
    });

    $('#btn-addrecepit').click(function () {
        var stockTake = {
            DateCreated: $('#DateCreated').val(),
            EmployeeID: $('#EmployeeID').val(),
            Remark: $('#Remark').val()
        };

        var stockTakeDetails = [];

        $.each($('#tb-shopgoods tbody').find('tr'), function (index, item) {
            var tds = $(item).find('td');

            if (tds.length > 0) {
                var stockTakeDetail = {
                    TemplateID: $(tds[0]).text(),
                    AmountOfStock: formatDisplayNumberToNumber($($(tds[tds.length - 4]).children(0)).val()),
                    ActualAmount: formatDisplayNumberToNumber($($(tds[tds.length - 3]).children(0)).val()),
                    Remark: $(tds[tds.length - 2]).text(),
                };

                console.log(tds.length);
                //{
                //    TemplateID: $(tds[0]).text(),
                //    Count: formatDisplayNumberToNumber($(tds[tds.length - 4]).children(0).val()),
                //    UnitPrice: formatDisplayNumberToNumber($(tds[tds.length - 3]).children(0).val()),
                //}

                stockTakeDetails.push(stockTakeDetail);
            }
        });

        console.log(stockTake);

        console.log(stockTakeDetails);

        createConfirmed(stockTake, stockTakeDetails);
    });

    //23/09/2021
    $('body').on('keyup', '.actual-amount', function () {
        updateActualyAmount(this, true);
    });

    $('body').on('keyup', '.quantity-difference', function () {
        updateActualyAmount(this, false);
    });
});

function getIds() {
    var trs = $('#tb-shopgoods tbody').find('tr');

    var ids = [];

    for (var i = 0; i < trs.length; i++) {
        ids.push($($(trs[i]).find('td')[0]).text().trim());
    }

    return ids;
}

function getShopGoods(ids, categoryId) {
    $.ajax({
        type: 'GET',
        datatype: 'JSON',
        data: {
            templateIDs: JSON.stringify(ids),
            categoryID: categoryId
        },
        url: '/Shop_Goods/GetAnother/',
        success: function (result) {
            if (result.data != null) {
                if (result.data.length > 0) {
                    var html = '';
                    result.data.forEach(item => {
                        html += '<tr>' +
                            '<td>' + item.id + '</td>' +
                            '<td>' + item.name + '</td>' +
                            '<td>' + item.producer + '</td>' +
                            '<td class="text-center">' + item.unit + '</td>' +
                            '<td class="text-right">' + formatNumber(item.count) + '</td>' +
                            '<td class="text-center"><input class="cb" type="checkbox"/></td>' +
                            '</tr>';
                    });

                    $('#tb-addshopgoods tbody').empty().append(html);

                    $('#btn-acceptshoppgoods').removeAttr('disabled');

                } else {
                    html += '<tr><td colspan="5">Không tìm thấy hàng hóa nào!</td></tr>'

                    $('#btn-acceptshoppgoods').attr('disabled', 'disabled');
                    $('#tb-addshopgoods tbody').empty().append(html);
                }
            }
        }
    })
}

function createConfirmed(info, json) {
    $.ajax({
        type: 'POST',
        datatype: 'JSON',
        data: {
            info: info,
            json: JSON.stringify(json)
        },
        url: '/Shop_Goods_StockTake/CreateConfirmed/',
        success: function (result) {
            if (result.msg == 'msg') {
                window.location = "/Shop_Goods_StockTake";
            }
        }
    })
}

function updateActualyAmount(e, isActualAmountChanged) {
    var parent = $($(e).parent()).parent();

    var inputs = $(parent).find('input');

    var amountOfStock = parseInt($(inputs[0]).val() != "" ? formatDisplayNumberToNumber($(inputs[0]).val()) : "0");

    if (isActualAmountChanged) {
        var actualAmount = parseInt($(inputs[1]).val() != "" ? formatDisplayNumberToNumber($(inputs[1]).val()) : "0");
        $(inputs[2]).val(formatNumber(amountOfStock - actualAmount));
    } else {
        var quantityDifference = parseInt($(inputs[2]).val() != "" ? formatDisplayNumberToNumber($(inputs[2]).val()) : "0");
        $(inputs[1]).val(formatNumber(amountOfStock - quantityDifference));
    }
}