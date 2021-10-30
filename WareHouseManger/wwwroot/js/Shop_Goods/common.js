$(document).ready(function () {
    var num = $('#Price').val();
    $('#price').val(formatNumber(num));

    $('.num').keyup(function () {
        var num = formatDisplayNumberToNumber($(this).val());
        var display = formatNumber(num);
        $(this).val(display);

        $('#' + $(this).data('target')).val(num);
    });

    $('#price').keyup(function () {
        var num = formatDisplayNumberToNumber($(this).val());
        console.log(num);
        $('#Price').val(num);
    });
});