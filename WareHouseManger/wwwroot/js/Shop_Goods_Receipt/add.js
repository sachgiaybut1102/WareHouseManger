$(document).ready(function () {
    checkCreateButton();

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
        $('#supplierID').val($(tds[0]).text() + ' - ' + $(tds[1]).text());
        $('#SupplierID').val($(tds[0]).text());

        $(this).attr('hidden', 'hidden');

        $('#modal-supplier').modal('toggle');

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
        //var cbs = $('#tb-addshopgoods tbody').find('.cb');

        //var html = '';

        //for (var i = 0; i < cbs.length; i++) {
        //    var e = cbs[i];

        //    if ($(e).is(':checked')) {
        //        var tds = $($(e).parent().parent()).find('td');
        //        html += '<tr>' +
        //            '<td>' + $(tds[0]).text() + '</td>' +
        //            '<td>' + $(tds[1]).text() + '</td>' +
        //            '<td>' + $('#select-category').text() + '</td>' +
        //            '<td>' + $(tds[2]).text() + '</td>' +
        //            '<td>' + $(tds[3]).text() + '</td>' +
        //            '<td style="width:120px;"><input class="form-control text-right number" min="1" value="1"  /></td>' +
        //            '<td style="width:120px;"><input class="form-control text-right number" min="1" value="1"  /></td>' +
        //            '<td style="width:150px;"><input class="form-control bg-white text-right" min="1" value="1" readonly/></td>' +
        //            '<td class="text-center"><button class="btn btn-sm btn-danger btn-remove">Xóa</button></td>' +
        //            '</tr>';
        //    }
        //}
        //$('#tb-shopgoods tbody').append(html);

        //getShopGoods(getIds(), $('#select-category').val());
    });

    $('body').on('click', '.btn-remove', function () {
        $($(this).parent().parent()).remove();

        checkCreateButton();
    });

    $('body').on('keyup', '.number', function () {
        $(this).val(formatNumber(formatDisplayNumberToNumber($(this).val())));

        var tds = $($(this).parent().parent()).find('td');

        var value = parseInt(formatDisplayNumberToNumber($(tds[tds.length - 3]).children(0).val())) * parseInt(formatDisplayNumberToNumber($(tds[tds.length - 4]).children(0).val()))

        $(tds[tds.length - 2]).children(0).val(formatNumber(value));
    });

    $('body').on('keyup', '.number-input', function () {
        updateTotal();
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
            SupplierID: $('#SupplierID').val(),
            EmployeeID: $('#EmployeeID').val(),
            Remark: $('#Remark').val(),
            Prepay: formatDisplayNumberToNumber($('#prepay').val()),
            TransferMoney: formatDisplayNumberToNumber($('#TransferMoney').val())
        };

        var recepitDetails = [];

        $.each($('#tb-shopgoods tbody').find('tr'), function (index, item) {
            var tds = $(item).find('td');

            var recepitDetail = new ShopGoodsViewModel($(tds[0]).text(), formatDisplayNumberToNumber($(tds[tds.length - 4]).children(0).val()), formatDisplayNumberToNumber($(tds[tds.length - 3]).children(0).val()));
            //{
            //    TemplateID: $(tds[0]).text(),
            //    Count: formatDisplayNumberToNumber($(tds[tds.length - 4]).children(0).val()),
            //    UnitPrice: formatDisplayNumberToNumber($(tds[tds.length - 3]).children(0).val()),
            //}

            recepitDetails.push(recepitDetail);
        });

        console.log(recepit);

        console.log(recepitDetails);

        createConfirmed(recepit, recepitDetails);
    });

    (function () {

        var beforePrint = function () {
            alert('Functionality to run before printing.');
        };

        var afterPrint = function () {
            alert('Functionality to run after printing');
        };

        if (window.matchMedia) {
            var mediaQueryList = window.matchMedia('print');

            mediaQueryList.addListener(function (mql) {
                //alert($(mediaQueryList).html());
                if (mql.matches) {
                    beforePrint();
                } else {
                    afterPrint();
                }
            });
        }

        window.onbeforeprint = beforePrint;
        window.onafterprint = afterPrint;

    }());

    $('body').on('click', '.btn-add', function () {
        var tds = $($(this).parent().parent()).find('td');

        var costprice = $(this).data('costprice');

        var html = '<tr>' +
            '<td>' + $(tds[0]).text() + '</td>' +
            '<td>' + $(tds[1]).text() + '</td>' +
            '<td>' + $('#select-category').text() + '</td>' +
            '<td>' + $(tds[2]).text() + '</td>' +
            '<td>' + $(tds[3]).text() + '</td>' +
            '<td style="width:140px;"><input class="form-control text-right number number-input" min="1" value="1" /></td>' +
            '<td style="width:160px;"><input class="form-control text-right number number-input" min="1" value="' + formatNumber(costprice) + '" /></td>' +
            '<td style="width:180px;"><input class="form-control text-right number number-input" min="1" value="' + formatNumber(costprice) + '" /></td>' +
            '<td class="align-middle" style="width:1px;"><button class="btn btn-sm btn-danger btn-remove">Xóa</button></td>' +
            '</tr>';
        $('#tb-shopgoods tbody').append(html);
        $($($(this).parent().parent())).remove();
        checkCreateButton();
    });

    $('#prepay').keyup(function () {
        prepay_KeyUp();
    });

    $('#cash').keyup(function () {
        cash_KeyUp();
    });
});


function prepay_KeyUp() {
    var total = parseInt(formatDisplayNumberToNumber($('#total').val()));

    var prepay = parseInt(formatDisplayNumberToNumber($('#prepay').val()));

    if (prepay <= 0) {
        $('#prepay').val(0);
    } else if (prepay >= total) {
        $('#prepay').val($('#total').val());

        prepay = total;
    }

    $('#cash').val(formatNumber(prepay));

    $('#TransferMoney').val(0);

    $('#remain').val(formatNumber(total - prepay));
}

function cash_KeyUp() {
    var prepay = parseInt(formatDisplayNumberToNumber($('#prepay').val()));

    var cash = parseInt(formatDisplayNumberToNumber($('#cash').val()));


    if (cash <= 0) {
        $('#cash').val(0);
    } else if (cash >= prepay) {
        $('#cash').val($('#prepay').val());
        prepay = cash;
    }
    //else {
    //    $('#cash').val(formatNumber(cash));
    //}

    $('#TransferMoney').val(formatNumber(prepay - cash));
}

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
                            '<td class="text-right">' + formatNumber(item.costprice) + '</td>' +
                            '<td class="text-right">' + formatNumber(item.price) + '</td>' +
                            //'<td class="text-center"><input class="cb" type="checkbox"/></td>' +
                            '<td class="align-middle"><button class="btn btn-block btn-sm btn-success btn-add" data-costprice="' + item.costprice + '">Thêm</button></td>' +
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
        url: '/Shop_Goods_Receipt/CreateConfirmed/',
        success: function (result) {
            if (result.msg == 'msg') {
                $('#Remark').empty();
                $('#tb-shopgoods tbody').empty();
                printJS('/Report/Shop_Goods_Receipt/' + result.id);
                alert("Tạo phiếu nhập hàng thành công!");
                checkCreateButton();
            }
        }
    })
}

$("#search").on("keyup", function () {
    var value = $(this).val().toLowerCase();
    $("#tb-addshopgoods tbody tr").filter(function () {
        $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
    });
});

function updateTotal() {
    var trs = $($('#tb-shopgoods tbody').find('tr'));

    var total = 0;
    $.each(trs, function (i, e) {
        var tds = $(e).find('td');
        total += parseInt(formatDisplayNumberToNumber($($(tds[tds.length - 2]).children(0)).val()));
    });

    $('#total').val(formatNumber(total));

    $('#prepay').val(0);

    $('#cash').val(0);

    $('#TransferMoney').val(0);

    $('#remain').val($('#total').val());

}

function checkCreateButton() {
    var trs = $($('#tb-shopgoods tbody').find('tr'));
    var length = trs.length;
    var flag = true;

    if (length == 0) {
        flag = false;
    }

    if ($('#EmployeeID').val() == '') {
        flag = false;
    }


    if ($('#SupplierID').val() == '') {
        flag = false;
    }

    if (flag) {
        $('#btn-addrecepit').removeAttr('disabled');
    } else {
        $('#btn-addrecepit').attr('disabled', 'disabled');
    }

    updateTotal();
}