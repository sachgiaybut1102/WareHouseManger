// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $('.btn-export').click(function () {
        console.log('ádasd');
        printJS($(this).data('target'));
    });

    $('.open-dialog').click(function () {
        var target = $(this).data('target');

        $('#' + target).modal('toggle');
    });

    //$("input[type=date]").datepicker({
    //    dateFormat: 'yy-mm-dd',
    //    onSelect: function (dateText, inst) {
    //        $(inst).val(dateText); // Write the value in the input
    //    }
    //});
});