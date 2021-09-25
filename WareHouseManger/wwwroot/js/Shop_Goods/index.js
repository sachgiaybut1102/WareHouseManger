$(document).ready(function () {
    $('#btn-closing-stock').click(function () {
        $('#modal-closing-stock').modal('toggle');
    });

    $('#btn-accecpt').click(function () {
        $.ajax({
            type: 'POST',
            datatype: 'JSON',
            url: '/Shop_Goods_ClosingStock/Create/',
            success: function (result) {
                var isSuccess = result.isSuccess;
                var msg = result.msg;

                if (isSuccess) {
                    $('#modal-msg').removeClass('alert-danger').removeClass('alert-success').addClass('alert-success');
                } else {
                    $('#modal-msg').removeClass('alert-danger').removeClass('alert-success').addClass('alert-danger');
                }

                $('#modal-msg').text(msg);
            }
        })
    })
})