$(document).ready(function () {
    $('.btn-details').click(function () {
        $('#modal-finalSettlement').modal('toggle');
        getFinalSettlement($(this).data('id'));
    });

    $('#btn-addFinalSettlement').click(function () {
        $('#modal-addFinalSettlement').modal('toggle');
    });
});

function getFinalSettlement(customerID) {
    $.ajax({
        type: 'GET',
        datatype: 'JSON',
        data: {
            customerID: customerID
        },
        url: '/FinalSettlement_Customer/GetByCustomerID/',
        success: function (result) {
            var data = result.data;
            console.log(data);

        }
    })
}
