$(document).ready(function () {
    var num = $('#Price').val();
    $('#price').val(formatNumber(num));

    $('.num').keyup(function () {
        var num = formatString($(this).val());
        var display = formatNumber(num);
        $(this).val(display);

        $('#' + $(this).data('target')).val(num);
    })
});