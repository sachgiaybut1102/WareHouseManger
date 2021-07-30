$(document).ready(function () {
    $('#btn-accept').click(function () {
        changePasswordConfirmed();
        $('#msg-changepass').attr('hidden', 'hidden');
    });

    $('#btn-accept-forgotpassword').click(function () {
        forgotPasswordConfirmed();
        $('#msg-changepass1').attr('hidden', 'hidden');
    });
});

function changePasswordConfirmed() {
    $.ajax({
        type: 'POST',
        datatype: 'JSON',
        data: {
            id: $('#EmployeeID').val(),
            username: $('#account_UserName').text(),
            str0: $('#currentPassword').val(),
            str1: $('#newPassword').val(),
            str2: $('#newPassword2').val()
        },
        url: '/Account/ChangePassword/',
        success: function (result) {
            //if (result.msg == 'msg') {
            //    $('#Remark').empty();
            //    $('#tb-shopgoods tbody').empty();
            //    printJS('/Report/Shop_Goods_Receipt/' + result.id);
            //    alert("Tạo phiếu nhập hàng thành công!");
            //    checkCreateButton();
            //}
            $('#msg-changepass').removeAttr('hidden');
            $('#msg-changepass').empty().append(result.msg);
            if (result.flag) {
                $('#msg-changepass').removeClass('alert-success').removeClass('alert-danger').addClass('alert-success')
            } else {
                $('#msg-changepass').removeClass('alert-success').removeClass('alert-danger').addClass('alert-danger')
            }
        }
    })
};

function forgotPasswordConfirmed() {
    $.ajax({
        type: 'POST',
        datatype: 'JSON',
        data: {
            id: $('#EmployeeID').val(),
            username: $('#account_UserName').text(),
            str1: $('#newPassword3').val(),
            str2: $('#newPassword4').val()
        },
        url: '/Account/ForgotPassword/',
        success: function (result) {
            //if (result.msg == 'msg') {
            //    $('#Remark').empty();
            //    $('#tb-shopgoods tbody').empty();
            //    printJS('/Report/Shop_Goods_Receipt/' + result.id);
            //    alert("Tạo phiếu nhập hàng thành công!");
            //    checkCreateButton();
            //}
            $('#msg-changepass1').removeAttr('hidden');
            $('#msg-changepass1').empty().append(result.msg);
            if (result.flag) {
                $('#msg-changepass1').removeClass('alert-success').removeClass('alert-danger').addClass('alert-success')
            } else {
                $('#msg-changepass1').removeClass('alert-success').removeClass('alert-danger').addClass('alert-danger')
            }
        }
    })
};