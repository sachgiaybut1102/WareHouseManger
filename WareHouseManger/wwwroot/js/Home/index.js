$(function () {
    getDetails();

    $('#btn-submit0').click(function () {
        getDetails();
    })
});

function getDetails() {
    $.ajax({
        type: 'POST',
        datatype: 'JSON',
        data: {
            startDate: $('#date-start').val(),
            endDate: $('#date-end').val()
        },
        url: '/Home/GetDetails',
        success: function (result) {
            var data = result.data;
            $('#countReceipt').text(formatNumber(data.countReceipt) + " đơn hàng");
            $('#countIssues').text(formatNumber(data.countIssues) + " đơn hàng");
            $('#revenue').text(formatNumber(data.revenue) + " VNĐ");
            $('#cost').text(formatNumber(data.cost) + " VNĐ");
            $('#profit').text(formatNumber(data.revenue - data.cost) + " VNĐ");
        }
    })
}
