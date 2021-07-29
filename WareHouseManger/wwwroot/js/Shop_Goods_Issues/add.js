$(document).ready(function () {
    checkCreateButton();

    $('.open-dialog').click(function () {
        var target = $(this).data('target');

        $('#' + target).modal('toggle');
    });

    $('.btn-accepet-employee').click(function () {
        $('.btn-accepet-employee').removeAttr('hidden');

        var tds = $($(this).parent().parent()).find('td');
        $('#employeeID').val($(tds[0]).text() + ' - ' + $(tds[1]).text());
        $('#EmployeeID').val($(tds[0]).text());

        $(this).attr('hidden', 'hidden');

        $('#modal-employee').modal('toggle');

        checkCreateButton();
    });

    $('.btn-accepet-customer').click(function () {
        $('.btn-accepet-customer').removeAttr('hidden');

        var tds = $($(this).parent().parent()).find('td');
        $('#customerID').val($(tds[0]).text() + ' - ' + $(tds[1]).text());
        $('#CustomerID').val($(tds[0]).text());

        $(this).attr('hidden', 'hidden');

        $('#modal-customer').modal('toggle');

        checkCreateButton();
    });

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
                    '<td style="width:120px;"><input class="form-control text-right number" min="1" value="' + $($(tds[5]).children(0)).val() + '" readonly/></td>' +
                    '<td style="width:120px;"><input class="form-control text-right number" min="1" value="' + $($(tds[6]).children(0)).val() + '" readonly/></td>' +
                    '<td style="width:120px;"><input class="form-control text-right number" min="1" value="' + $($(tds[7]).children(0)).val() + '" readonly/></td>' +
                    '<td class="text-center"><button class="btn btn-sm btn-danger btn-remove">Xóa</button></td>' +
                    '</tr>';
            }
        }
        $('#tb-shopgoods tbody').append(html);

        getShopGoods(getIds(), $('#select-category').val());
    });

    $('body').on('click', '.btn-remove', function () {
        $($(this).parent().parent()).remove();
        checkCreateButton();
    });

    $('body').on('keyup', '.number', function () {

        $(this).val(formatNumber(formatString($(this).val())));

        var value = parseInt(formatString($(tds[tds.length - 3]).children(0).val())) * parseInt(formatString($(tds[tds.length - 4]).children(0).val()))

        $(tds[tds.length - 2]).children(0).val(formatNumber(value));
    });

    class ShopGoodsViewModel {
        constructor(TemplateID, Count, UnitPrice) {
            this.TemplateID = TemplateID;
            this.Count = Count;
            this.UnitPrice = UnitPrice;
        }
    }

    $('#btn-addrecepit').click(function () {
        var recepit = {
            DateCreated: $('#DateCreated').val(),
            CustomerID: $('#CustomerID').val(),
            EmployeeID: $('#EmployeeID').val(),
            Remark: $('#Remark').val()
        };

        var recepitDetails = [];

        $.each($('#tb-shopgoods tbody').find('tr'), function (index, item) {
            var tds = $(item).find('td');

            var recepitDetail = new ShopGoodsViewModel($(tds[0]).text(), formatString($(tds[tds.length - 4]).children(0).val()), formatString($(tds[tds.length - 3]).children(0).val()));
            //{
            //    TemplateID: $(tds[0]).text(),
            //    Count: formatString($(tds[tds.length - 4]).children(0).val()),
            //    UnitPrice: formatString($(tds[tds.length - 3]).children(0).val()),
            //}

            recepitDetails.push(recepitDetail);
        });

        console.log(recepit);

        console.log(recepitDetails);

        createConfirmed(recepit, recepitDetails);
    });

    $('body').on('keyup', '.count', function () {

        var tds = $($(this).parent().parent()).find('td');

        if (parseInt(formatString($(this).val())) > parseInt(formatString($(tds[tds.length - 5]).children(0).val()))) {
            $(this).val(formatString($(tds[tds.length - 5]).children(0).val()));
        }

        $(this).val(formatNumber(formatString($(this).val())));

        var value = parseInt(formatString($(tds[tds.length - 3]).children(0).val())) * parseInt(formatString($(tds[tds.length - 4]).children(0).val()))

        $(tds[tds.length - 2]).children(0).val(formatNumber(value));

        //enbale button
        var btn = $(tds[tds.length - 1]).children(0);
        if (parseInt(formatString($(this).val())) > 0) {
            $(btn).removeAttr('disabled');
        } else {
            $(btn).attr('disabled', 'disabled');
        }
    });

    $('body').on('click', '.btn-add', function () {
        var tds = $($(this).parent().parent()).find('td');
        var html = '<tr>' +
            '<td>' + $(tds[0]).text() + '</td>' +
            '<td>' + $(tds[1]).text() + '</td>' +
            '<td>' + $('#select-category').text() + '</td>' +
            '<td>' + $(tds[2]).text() + '</td>' +
            '<td>' + $(tds[3]).text() + '</td>' +
            '<td style="width:120px;"><input class="form-control text-right number" min="1" value="' + $($(tds[5]).children(0)).val() + '" readonly/></td>' +
            '<td style="width:140px;"><input class="form-control text-right number" min="1" value="' + $($(tds[6]).children(0)).val() + '" readonly/></td>' +
            '<td style="width:160px;"><input class="form-control text-right number" min="1" value="' + $($(tds[7]).children(0)).val() + '" readonly/></td>' +
            '<td class="align-middle" style="width:1px;"><button class="btn btn-sm btn-danger btn-remove">Xóa</button></td>' +
            '</tr>';
        $('#tb-shopgoods tbody').append(html);
        $($($(this).parent().parent())).remove();
        checkCreateButton();
    })
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
                            '<td class="align-middle">' + item.id + '</td>' +
                            '<td class="align-middle">' + item.name + '</td>' +
                            '<td class="align-middle">' + item.producer + '</td>' +
                            '<td class="align-middle">' + item.unit + '</td>' +
                            '<td><input class="form-control text-right" value="' + formatNumber(item.count) + '" readonly/></td>' +
                            '<td><input type="text" class="form-control text-right count" style="width:100px" value="0"/></td>' +
                            '<td class="text-right"><input class="form-control text-right" value="' + formatNumber(item.price) + '" readonly/></td>' +
                            '<td><input class="form-control text-right" value="0" readonly/></td>' +
                            /*'<td class="text-center"><input class="cb" type="checkbox"/></td>' +*/
                            '<td class="align-middle"><button class="btn btn-block btn-sm btn-success btn-add" disabled="disabled">Thêm</button></td>' +
                            '</tr>';
                    });

                    $('#tb-addshopgoods tbody').empty().append(html);

                    $('#btn-acceptshoppgoods').removeAttr('disabled');

                } else {
                    html += '<tr><td colspan="9">Không tìm thấy hàng hóa nào!</td></tr>'

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
        url: '/Shop_Goods_Issues/CreateConfirmed/',
        success: function (result) {
            if (result.msg == 'msg') {
                $('#Remark').empty();
                $('#tb-shopgoods tbody').empty();
                printJS('/Report/Shop_Goods_Issues/' + result.id);
                alert("Tạo phiếu nhập hàng thành công!");
                checkCreateButton();
            }

        }
    });
}

$(".search").on("keyup", function () {
    var value = $(this).val().toLowerCase();
    var target = $(this).data('target');
    $("#" + target + " tbody tr").filter(function () {
        $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
    });
});

function updateTotal() {
    var trs = $($('#tb-shopgoods tbody').find('tr'));

    var total = 0;
    $.each(trs, function (i, e) {
        var tds = $(e).find('td');
        total += parseInt(formatString($($(tds[tds.length - 2]).children(0)).val()));
    });

    $('#total').text('Tổng tiền: ' + formatNumber(total));

}

function checkCreateButton() {
    var length = $($('#tb-shopgoods tbody').find('tr')).length;
    var flag = true;

    if (length == 0) {
        flag = false;
    }

    if ($('#CustomerID').val() == '') {
        flag = false;
    }


    if ($('#EmployeeID').val() == '') {
        flag = false;
    }

    if (flag) {
        $('#btn-addrecepit').removeAttr('disabled');
    } else {
        $('#btn-addrecepit').attr('disabled', 'disabled');
    } 

    updateTotal();
}