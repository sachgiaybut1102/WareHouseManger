function formatNumber(nStr) {
    var groupSeperate = ',';

    var x = formatDisplayNumberToNumber(nStr);

    var rgx = /(\d+)(\d{3})/;

    while (rgx.test(x)) {
        x = x.replace(rgx, '$1' + groupSeperate + '$2');
    }
    return x;
}

function formatDisplayNumberToNumber(money) {
    var x = '';
    var arr = money.toString().split(',');
    arr.forEach((str) => {
        x += str;
    });

    return x;
}