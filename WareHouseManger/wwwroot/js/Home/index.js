// Set new default font family and font color to mimic Bootstrap's default styling
Chart.defaults.global.defaultFontFamily = 'Nunito', '-apple-system,system-ui,BlinkMacSystemFont,"Segoe UI",Roboto,"Helvetica Neue",Arial,sans-serif';
Chart.defaults.global.defaultFontColor = '#858796';

var realRevenue = 0;
var realCost = 0;

function number_format(number, decimals, dec_point, thousands_sep) {
    // *     example: number_format(1234.56, 2, ',', ' ');
    // *     return: '1 234,56'
    number = (number + '').replace(',', '').replace(' ', '');
    var n = !isFinite(+number) ? 0 : +number,
        prec = !isFinite(+decimals) ? 0 : Math.abs(decimals),
        sep = (typeof thousands_sep === 'undefined') ? ',' : thousands_sep,
        dec = (typeof dec_point === 'undefined') ? '.' : dec_point,
        s = '',
        toFixedFix = function (n, prec) {
            var k = Math.pow(10, prec);
            return '' + Math.round(n * k) / k;
        };
    // Fix for IE parseFloat(0.55).toFixed(0) = 0;
    s = (prec ? toFixedFix(n, prec) : '' + Math.round(n)).split('.');
    if (s[0].length > 3) {
        s[0] = s[0].replace(/\B(?=(?:\d{3})+(?!\d))/g, sep);
    }
    if ((s[1] || '').length < prec) {
        s[1] = s[1] || '';
        s[1] += new Array(prec - s[1].length + 1).join('0');
    }
    return s.join(dec);
}
var ctx = document.getElementById("myAreaChart");

$(function () {
    var date = new Date();
    $('#month').val(date.getMonth() + 1);

    getDetails();
    getRankTemplate();
    getChart();
    getOutOfStock();
    getSoldOutOfStock();

    $('#btn-submit0').click(function () {
        getDetails();
    });

    $('#btn-submit-chart').click(function () {
        getChart();
        getRankTemplate();
    });

    $('#datetype').change(function () {
        if ($(this).val() == 'month') {
            $('#month').removeAttr('disabled');
        } else {
            $('#month').attr('disabled', 'disabled');
        }
    });
});

function getDetails() {
    //$.ajax({
    //    type: 'POST',
    //    datatype: 'JSON',
    //    data: {
    //        startDate: $('#date-start').val(),
    //        endDate: $('#date-end').val()
    //    },
    //    url: '/Home/GetDetails',
    //    success: function (result) {
    //        var data = result.data;
    //        $('#countReceipt').text(formatNumber(data.countReceipt) + " đơn hàng");
    //        $('#countIssues').text(formatNumber(data.countIssues) + " đơn hàng");
    //        $('#revenue').text(formatNumber(data.revenue) + " VNĐ");
    //        $('#cost').text(formatNumber(data.cost) + " VNĐ");
    //        $('#profit').text(formatNumber(data.revenue - data.cost) + " VNĐ");
    //    }
    //})
    getCountShop_Goods_Receipt();
    getCountShop_Goods_Issues();
    getCountShop_Goods_Revenue();
    getCountShop_Goods_Cost();
    getCountShop_Goods_RealRevenue();
    // getCountShop_Goods_RealCost();
}

function getCountShop_Goods_Receipt() {
    $.ajax({
        type: 'POST',
        datatype: 'JSON',
        data: {
            startDate: $('#date-start').val(),
            endDate: $('#date-end').val()
        },
        url: '/Home/GetCountShop_Goods_Receipt',
        success: function (result) {
            $('#countReceipt').text(formatNumber(result.value) + " đơn hàng");
            //$('#countIssues').text(formatNumber(data.countIssues) + " đơn hàng");
            //$('#revenue').text(formatNumber(data.revenue) + " VNĐ");
            //$('#cost').text(formatNumber(data.cost) + " VNĐ");
            //$('#profit').text(formatNumber(data.revenue - data.cost) + " VNĐ");
        }
    })
}

function getCountShop_Goods_Issues() {
    $.ajax({
        type: 'POST',
        datatype: 'JSON',
        data: {
            startDate: $('#date-start').val(),
            endDate: $('#date-end').val()
        },
        url: '/Home/GetCountShop_Goods_Issues',
        success: function (result) {
            $('#countIssues').text(formatNumber(result.value) + " đơn hàng");
        }
    })
}

function getCountShop_Goods_Revenue() {
    $.ajax({
        type: 'POST',
        datatype: 'JSON',
        data: {
            startDate: $('#date-start').val(),
            endDate: $('#date-end').val()
        },
        url: '/Home/GetCountShop_Goods_Revenue',
        success: function (result) {
            $('#revenue').text(formatNumber(result.value) + " VNĐ");
        }
    })
}

function getCountShop_Goods_Cost() {
    $.ajax({
        type: 'POST',
        datatype: 'JSON',
        data: {
            startDate: $('#date-start').val(),
            endDate: $('#date-end').val()
        },
        url: '/Home/GetCountShop_Goods_Cost',
        success: function (result) {
            $('#cost').text(formatNumber(result.value) + " VNĐ");
        }
    })
}

function getCountShop_Goods_RealRevenue() {
    $.ajax({
        type: 'POST',
        datatype: 'JSON',
        data: {
            startDate: $('#date-start').val(),
            endDate: $('#date-end').val()
        },
        url: '/Home/GetCountShop_Goods_RealRevenue',
        success: function (result) {
            realRevenue = result.value;
            $('#realrevenue').text(formatNumber(result.value) + " VNĐ");

            getCountShop_Goods_RealCost();
        }
    })
}

function getCountShop_Goods_RealCost() {
    $.ajax({
        type: 'POST',
        datatype: 'JSON',
        data: {
            startDate: $('#date-start').val(),
            endDate: $('#date-end').val()
        },
        url: '/Home/GetCountShop_Goods_RealCost',
        success: function (result) {
            realCost = result.value;

            $('#realcost').text(formatNumber(result.value) + " VNĐ");

            getCountShop_Goods_Profit();
        }
    })
}

function getCountShop_Goods_Profit() {
    $('#profit').text(formatNumber(realRevenue - realCost) + " VNĐ");
}

function getRankTemplate() {

    $.ajax({
        type: 'POST',
        datatype: 'JSON',
        data: {
            type: $('#datetype').val(),
            month: $('#month').val(),
            year: $('#year').val(),
        },
        url: '/Home/GetRankTemplate',
        success: function (result) {
            console.log(result);

            var html = '';

            $.each(result.data, function (i, e) {
                html += '<tr>' +
                    '<td>' + (i + 1) + '</td>' +
                    '<td>' + e.templateID + '</td>' +
                    '<td>' + e.name + '</td>' +
                    '<td class="text-center">' + e.unit + '</td>' +
                    '<td class="text-right">' + formatNumber(e.count) + '</td>' +
                    '<td class="text-right">' + formatNumber(e.turnover) + '</td>' +
                    '</tr>';
            });

            $('#tb-rank-shopgoods tbody').empty().append(html);
        }
    })
}

function getChart() {
    var month = [];
    var sumReceipts = [];
    var sumIssues = [];
    var profits = [];

    $.ajax({
        type: 'POST',
        datatype: 'JSON',
        data: {
            type: $('#datetype').val(),
            month: $('#month').val(),
            year: $('#year').val(),
        },
        url: '/Home/GetChart',
        success: function (result) {
            $('#myAreaChart').empty();

            var data = result.data;
            console.log(data);

            var title = '';
            if ($('#datetype').val() == "month") {
                title = 'Ngày ';
            }
            else {
                title = 'Tháng ';
            }

            $.each(data.sumReceipts, function (i, item) {
                var sumReceipt = item.value;
                var sumIssue = data.sumIssues[i].value;

                month.push(title + item.index);
                sumReceipts.push(sumReceipt);
                sumIssues.push(sumIssue);
                profits.push(sumIssue - sumReceipt);
            });

            myLineChart.data.labels = month;
            myLineChart.data.datasets[0].data = sumReceipts;
            myLineChart.data.datasets[1].data = sumIssues;
            myLineChart.data.datasets[2].data = profits;
            myLineChart.update();
        }
    })
}

function getOutOfStock() {
    $.ajax({
        type: 'POST',
        datatype: 'JSON',
        url: '/Home/GetOutOfStock',
        success: function (result) {
            console.log(result);

            var html = '';

            $.each(result.data, function (i, e) {
                html += '<tr>' +
                    '<td>' + (i + 1) + '</td>' +
                    '<td>' + e.templateID + '</td>' +
                    '<td>' + e.name + '</td>' +
                    '<td class="text-center">' + e.category + '</td>' +
                    '<td class="text-center">' + e.unit + '</td>' +
                    '<td class="text-right">' + formatNumber(e.count) + '</td>' +
                    '</tr>';
            });

            $('#tb-outofstock tbody').empty().append(html);
        }
    })
}

function getSoldOutOfStock() {
    $.ajax({
        type: 'POST',
        datatype: 'JSON',
        url: '/Home/GetSoldOutOfStock',
        success: function (result) {
            console.log(result);

            var html = '';

            $.each(result.data, function (i, e) {
                html += '<tr>' +
                    '<td>' + (i + 1) + '</td>' +
                    '<td>' + e.templateID + '</td>' +
                    '<td>' + e.name + '</td>' +
                    '<td class="text-center">' + e.category + '</td>' +
                    '<td class="text-center">' + e.unit + '</td>' +
                    '</tr>';
            });

            $('#tb-soldoutofstock tbody').empty().append(html);
        }
    })
}

var myLineChart = new Chart(ctx, {
    type: 'line',
    data: {
        labels: [],
        datasets: [{
            label: "Nhập hàng",
            lineTension: 0.3,
            backgroundColor: "rgba(242, 38, 19, 0.05)",
            borderColor: "rgba(242, 38, 19, 1)",
            pointRadius: 3,
            pointBackgroundColor: "rgba(242, 38, 19, 1)",
            pointBorderColor: "rgba(242, 38, 19, 1)",
            pointHoverRadius: 3,
            pointHoverBackgroundColor: "rgba(242, 38, 19, 1)",
            pointHoverBorderColor: "rgba(242, 38, 19, 1)",
            pointHitRadius: 10,
            pointBorderWidth: 2,
            data: [],
        },
        {
            label: "Doanh thu",
            lineTension: 0.3,
            backgroundColor: "rgba(78, 115, 223, 0.05)",
            borderColor: "rgba(78, 115, 223, 1)",
            pointRadius: 3,
            pointBackgroundColor: "rgba(78, 115, 223, 1)",
            pointBorderColor: "rgba(78, 115, 223, 1)",
            pointHoverRadius: 3,
            pointHoverBackgroundColor: "rgba(78, 115, 223, 1)",
            pointHoverBorderColor: "rgba(78, 115, 223, 1)",
            pointHitRadius: 10,
            pointBorderWidth: 2,
            data: [0, 1],
        },
        {
            label: "Lãi thực",
            lineTension: 0.3,
            backgroundColor: "rgba(0, 230, 64, 0.05)",
            borderColor: "rgba(0, 230, 64, 1)",
            pointRadius: 3,
            pointBackgroundColor: "rgba(0, 230, 64, 1)",
            pointBorderColor: "rgba(0, 230, 64, 1)",
            pointHoverRadius: 3,
            pointHoverBackgroundColor: "rgba(0, 230, 64, 1)",
            pointHoverBorderColor: "rgba(0, 230, 64, 1)",
            pointHitRadius: 10,
            pointBorderWidth: 2,
            data: [],
        }
        ],
    },

    options: {
        maintainAspectRatio: false,
        layout: {
            padding: {
                left: 10,
                right: 25,
                top: 25,
                bottom: 0
            }
        },
        scales: {
            xAxes: [{
                time: {
                    unit: 'date'
                },
                gridLines: {
                    display: false,
                    drawBorder: false
                },
                ticks: {
                    maxTicksLimit: 7
                }
            }],
            yAxes: [{
                ticks: {
                    maxTicksLimit: 5,
                    padding: 10,
                    // Include a dollar sign in the ticks
                    callback: function (value, index, values) {
                        return number_format(value) + ' VNĐ';
                    }
                },
                gridLines: {
                    color: "rgb(234, 236, 244)",
                    zeroLineColor: "rgb(234, 236, 244)",
                    drawBorder: false,
                    borderDash: [2],
                    zeroLineBorderDash: [2]
                }
            }],
        },
        legend: {
            display: false
        },
        tooltips: {
            backgroundColor: "rgb(255,255,255)",
            bodyFontColor: "#858796",
            titleMarginBottom: 10,
            titleFontColor: '#6e707e',
            titleFontSize: 14,
            borderColor: '#dddfeb',
            borderWidth: 1,
            xPadding: 15,
            yPadding: 15,
            displayColors: false,
            intersect: false,
            mode: 'index',
            caretPadding: 10,
            callbacks: {
                label: function (tooltipItem, chart) {
                    var datasetLabel = chart.datasets[tooltipItem.datasetIndex].label || '';
                    return datasetLabel + ': ' + number_format(tooltipItem.yLabel) + ' VNĐ';
                }
            }
        }
    }
});