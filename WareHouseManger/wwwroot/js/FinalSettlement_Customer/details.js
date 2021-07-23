$(document).ready(function () {
    $('.details').click(function () {
        $('#modal-addFinalSettlement').modal('toggle');
        var id = $($($(this).parent().parent()).find('td')[0]).text();
        getRemain(id);
        getFinalSettlement_Customers(id);
    });

    $('#btn-acceptFinalSettlement').click(function () {
        add();
    });

    $('.num').keyup(function () {
        var total = parseInt(formatString($('#remain').val()));
        var payment = parseInt(formatString($('#num0').val()));

        $('#num0').val(formatNumber(payment));

        var remain = total - payment;

        if (remain < 0) {
            $('#num0').val(formatNumber(total));
            $('#num1').val(0);
        } else {
            $('#num1').val(formatNumber(remain));
        }
    })
});

function getRemain(goodsIssueID) {
    $.ajax({
        type: 'GET',
        datatype: 'JSON',
        data: {
            goodsIssueID: goodsIssueID
        },
        url: '/FinalSettlement_Customer/GetRemain/',
        success: function (result) {
            var data = result.data;
            console.log(data);

            $('#id').val(goodsIssueID);
            $('#remain').val(formatNumber(result.data.remain));
            $('#num0').val(0);
            $('#num1').val(0);

            if (result.data.remain > 0) {
                $('#btn-acceptFinalSettlement').removeAttr('disabled', 'disabled');
            } else {
                $('#btn-acceptFinalSettlement').attr('disabled', 'disabled');
            }
        }
    });
}


function getFinalSettlement_Customers(goodsIssueID) {
    $.ajax({
        type: 'GET',
        datatype: 'JSON',
        data: {
            goodsIssueID: goodsIssueID
        },
        url: '/FinalSettlement_Customer/GetByGoodsIssueID/',
        success: function (result) {
            var html = '';

            $.each(result.data, function (i, e) {
                html += '<tr>' +
                    '<td>' + e.dateCreated + '</td>' +
                    '<td class="text-right">' + formatNumber(e.payment) + '</td>' +
                    '<td class="text-right">' + formatNumber(e.remainder) + '</td>' +
                    '</tr>';
            });

            $('#tb-history-finalsettlement tbody').empty().append(html);
        }
    });
}

function add() {
    var info = {
        CustomerID: $('#customer_CustomerID').val(),
        GoodsIssuesID: $('#id').val(),
        Payment: $('#num0').val(),
        DateCreated: $('#date-created').val(),
    };

    $.ajax({
        type: 'POST',
        datatype: 'JSON',
        data: {
            info: info
        },
        url: '/FinalSettlement_Customer/add/',
        success: function (result) {
            if (result.msg == 'ok') {
                window.location.reload();
            }
        }
    });
}
