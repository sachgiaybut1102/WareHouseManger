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
                    '<td style="width:120px;"><input class="form-control text-right number" min="1" value="1"  /></td>' +
                    '<td style="width:120px;"><input class="form-control text-right number" min="1" value="1"  /></td>' +
                    '<td style="width:150px;"><input class="form-control bg-white text-right" min="1" value="1" readonly/></td>' +
                    '<td class="text-center"><button class="btn btn-sm btn-danger btn-remove">Xóa</button></td>' +
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
        $(this).val(formatNumber(formatString($(this).val())));

        var tds = $($(this).parent().parent()).find('td');

        var value = parseInt(formatString($(tds[tds.length - 3]).children(0).val())) * parseInt(formatString($(tds[tds.length - 4]).children(0).val()))

        $(tds[tds.length - 2]).children(0).val(formatNumber(value));
    });

    $('#btn-addrecepit').click(function () {
        var recepit = {
            DateCreated: $('#DateCreated').val(),
            SupplierID: $('#SupplierID').val(),
            EmployeeID: $('#EmployeeID').val(),
            Remark: $('#Remark').val()
        };

        var recepitDetails = [];

        $.each($('#tb-shopgoods tbody').find('tr'), function (index, item) {
            var tds = $(item).find('td');

            var recepitDetail = {
                TemplateID: $(tds[0]).text(),
                Count: $(tds[tds.length - 4]).children(0).val(),
                UnitPrice: $(tds[tds.length - 3]).children(0).val(),
            }

            recepitDetails.push(recepitDetail);
        });
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
                            '<td>' + item.unit + '</td>' +
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

function formatNumber(nStr) {
    var groupSeperate = ',';

    var x = formatString(nStr);

    var rgx = /(\d+)(\d{3})/;

    while (rgx.test(x)) {
        x = x.replace(rgx, '$1' + groupSeperate + '$2');
    }
    return x;
}

function formatString(money) {
    var x = '';
    var arr = money.toString().split(',');
    arr.forEach((str) => {
        x += str;
    });

    return x;
}