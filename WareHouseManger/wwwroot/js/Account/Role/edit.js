$(document).ready(function () {
    $('body').on('click', '.toggle', function () {
       
        getShopGoods($($(this).children(0)).data('id'), this);
        console.log(checked);

        
    })
});

function getShopGoods(roleId, element) {
    $.ajax({
        type: 'GET',
        datatype: 'JSON',
        data: {
            roleId: roleId,
        },
        url: '/Account/UpdateRole/',
        success: function (result) {
            if (result.msg != 'OK') {
                var checked = $(element).hasClass('off');

                if (checked) {
                    $(element).removeClass('off');
                } else {
                    $(element).addClass('off');
                }
            }
        }
    })
}