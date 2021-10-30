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
var ctx2 = document.getElementById("myAreaChart2");

$(document).ready(function () {
    var date = new Date();
    $('#month').val(date.getMonth() + 1);

    getCountRecepitShopGoods();
    getCountRecepitShopGoodsGroupByCustomer();

    $('#btn-submit-chart').click(function () {
        getCountRecepitShopGoods();
        getCountRecepitShopGoodsGroupByCustomer();
    });

    $('#datetype').change(function () {
        if ($(this).val() == 'month') {
            $('#month').removeAttr('disabled');
        } else {
            $('#month').attr('disabled', 'disabled');
        }
    });

    $('#btn-submit-stockcard').click(function () {
        var dateBegin = $('#date-begin-stockcard').val();
        var dateEnd = $('#date-end-stockcard').val();
        getViewStockCard(dateBegin, dateEnd, $('#TemplateID').val());
    });

    $('#btn-submit-stockcard').trigger('click');
});

function getCountRecepitShopGoods() {
    var month = [];
    var sumReceipts = [];
    var sumIssues = [];
    var profits = [];
    var countIssues = [];
    var countRecepit = [];
    var inventory = [];
    $.ajax({
        type: 'POST',
        datatype: 'JSON',
        data: {
            id: $('#TemplateID').val(),
            type: $('#datetype').val(),
            month: $('#month').val(),
            year: $('#year').val(),
        },
        url: '/Home/GetCountRecepitShopGoods',
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

            $.each(data, function (i, item) {
                var sumReceipt = item.cost;
                var sumIssue = item.turnover;

                month.push(title + item.index);
                sumReceipts.push(sumReceipt);
                sumIssues.push(sumIssue);
                profits.push(sumIssue - sumReceipt);

                countIssues.push(item.countIssues);
                countRecepit.push(item.countRecepit);
                inventory.push(item.countRecepit - item.countIssues);
            });

            myLineChart.data.labels = month;
            myLineChart.data.datasets[0].data = sumReceipts;
            myLineChart.data.datasets[1].data = sumIssues;
            myLineChart.data.datasets[2].data = profits;
            myLineChart.update();

            myBarChart.data.labels = month;
            myBarChart.data.datasets[0].data = countRecepit;
            myBarChart.data.datasets[1].data = countIssues;
            /* myLineChart2.data.datasets[2].data = inventory;*/
            myBarChart.update();
        }
    })
}

function getCountRecepitShopGoodsGroupByCustomer() {
    $.ajax({
        type: 'POST',
        datatype: 'JSON',
        data: {
            id: $('#TemplateID').val(),
            type: $('#datetype').val(),
            month: $('#month').val(),
            year: $('#year').val(),
        },
        url: '/Home/GetCountRecepitShopGoodsGroupByCustomer',
        success: function (result) {
            $('#myAreaChart').empty();

            var data = result.data;
            console.log(data);

            var html = '';

            $.each(data, function (i, item) {
                html += '<tr>' +
                    '<td>' + item.id + '</td>' +
                    '<td>' + item.name + '</td>' +
                    '<td>' + item.address + '</td>' +
                    '<td>' + item.phoneNumber + '</td>' +
                    '<td class="text-right">' + formatNumber(item.count) + '</td>' +
                    '<td class="text-right">' + formatNumber(item.turnover) + '</td>' +
                    '</tr>';
            });

            if (html == '') {
                html = '<tr><td colspan="6" class="font-italic">Không tìm thấy kết quả nào cả!</td></tr>';
            }

            $('#tb-customer tbody').empty().append(html);
        }
    })
}


var myLineChart = new Chart(ctx, {
    type: 'line',
    data: {
        labels: [],
        datasets: [{
            label: "Chi phí",
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
            label: "Lợi nhuận",
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

var myBarChart = new Chart(ctx2, {
    type: 'bar',
    data: {
        labels: [],
        datasets: [{
            label: "Số lượng nhập",
            backgroundColor: "#4e73df",
            hoverBackgroundColor: "#2e59d9",
            borderColor: "#4e73df",
            data: [],
        },
        {
            label: "Số lượng bán",
            backgroundColor: "rgba(28, 200, 138, 0.5)",
            hoverBackgroundColor: "rgba(28, 200, 138, 1)",
            borderColor: "rgba(28, 200, 138, 0.5)",
            data: [],
            maxBarThickness: 10
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
                    unit: 'month'
                },
                gridLines: {
                    display: false,
                    drawBorder: false
                },
                ticks: {
                    maxTicksLimit: 6
                },
                maxBarThickness: 55,
            }],
            yAxes: [{
                ticks: {
                    //min: 0,
                    //max: 15000,
                    maxTicksLimit: 5,
                    padding: 10,
                    // Include a dollar sign in the ticks
                    callback: function (value, index, values) {
                        return number_format(value) + ' ' + $('#UnitID').val();
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
            titleMarginBottom: 10,
            titleFontColor: '#6e707e',
            titleFontSize: 14,
            backgroundColor: "rgb(255,255,255)",
            bodyFontColor: "#858796",
            borderColor: '#dddfeb',
            borderWidth: 1,
            xPadding: 15,
            yPadding: 15,
            displayColors: false,
            caretPadding: 10,
            callbacks: {
                label: function (tooltipItem, chart) {
                    var datasetLabel = chart.datasets[tooltipItem.datasetIndex].label || '';
                    return datasetLabel + ': ' + number_format(tooltipItem.yLabel) + ' ' + $('#UnitID').val();
                }
            }
        },
    }
});

//var myLineChart2 = new Chart(ctx2, {
//    type: 'line',
//    data: {
//        labels: [],
//        datasets: [{
//            label: "Số lương nhập",
//            lineTension: 0.3,
//            backgroundColor: "rgba(242, 38, 19, 0.05)",
//            borderColor: "rgba(242, 38, 19, 1)",
//            pointRadius: 3,
//            pointBackgroundColor: "rgba(242, 38, 19, 1)",
//            pointBorderColor: "rgba(242, 38, 19, 1)",
//            pointHoverRadius: 3,
//            pointHoverBackgroundColor: "rgba(242, 38, 19, 1)",
//            pointHoverBorderColor: "rgba(242, 38, 19, 1)",
//            pointHitRadius: 10,
//            pointBorderWidth: 2,
//            data: [],
//        },
//        {
//            label: "Số lượng tồn",
//            lineTension: 0.3,
//            backgroundColor: "rgba(28, 200, 138, 0.05)",
//            borderColor: "rgba(28, 200, 138, 1)",
//            pointRadius: 3,
//            pointBackgroundColor: "rgba(28, 200, 138, 1)",
//            pointBorderColor: "rgba(28, 200, 138, 1)",
//            pointHoverRadius: 3,
//            pointHoverBackgroundColor: "rgba(28, 200, 138, 1)",
//            pointHoverBorderColor: "rgba(28, 200, 138, 1)",
//            pointHitRadius: 10,
//            pointBorderWidth: 2,
//            data: [0, 1],
//        },
//        {
//            label: "Số lượng bán",
//            lineTension: 0.3,
//            backgroundColor: "rgba(0, 230, 64, 0.05)",
//            borderColor: "rgba(0, 230, 64, 1)",
//            pointRadius: 3,
//            pointBackgroundColor: "rgba(0, 230, 64, 1)",
//            pointBorderColor: "rgba(0, 230, 64, 1)",
//            pointHoverRadius: 3,
//            pointHoverBackgroundColor: "rgba(0, 230, 64, 1)",
//            pointHoverBorderColor: "rgba(0, 230, 64, 1)",
//            pointHitRadius: 10,
//            pointBorderWidth: 2,
//            data: [],
//        }
//        ],
//    },

//    options: {
//        maintainAspectRatio: false,
//        layout: {
//            padding: {
//                left: 10,
//                right: 25,
//                top: 25,
//                bottom: 0
//            }
//        },
//        scales: {
//            xAxes: [{
//                time: {
//                    unit: 'date'
//                },
//                gridLines: {
//                    display: false,
//                    drawBorder: false
//                },
//                ticks: {
//                    maxTicksLimit: 7
//                }
//            }],
//            yAxes: [{
//                ticks: {
//                    maxTicksLimit: 5,
//                    padding: 10,
//                    // Include a dollar sign in the ticks
//                    callback: function (value, index, values) {
//                        return number_format(value) + ' ' + $('#UnitID').val();
//                    }
//                },
//                gridLines: {
//                    color: "rgb(234, 236, 244)",
//                    zeroLineColor: "rgb(234, 236, 244)",
//                    drawBorder: false,
//                    borderDash: [2],
//                    zeroLineBorderDash: [2]
//                }
//            }],
//        },
//        legend: {
//            display: false
//        },
//        tooltips: {
//            backgroundColor: "rgb(255,255,255)",
//            bodyFontColor: "#858796",
//            titleMarginBottom: 10,
//            titleFontColor: '#6e707e',
//            titleFontSize: 14,
//            borderColor: '#dddfeb',
//            borderWidth: 1,
//            xPadding: 15,
//            yPadding: 15,
//            displayColors: false,
//            intersect: false,
//            mode: 'index',
//            caretPadding: 10,
//            callbacks: {
//                label: function (tooltipItem, chart) {
//                    var datasetLabel = chart.datasets[tooltipItem.datasetIndex].label || '';
//                    return datasetLabel + ': ' + number_format(tooltipItem.yLabel) + ' ' + $('#UnitID').val();
//                }
//            }
//        }
//    }
//});

function getViewStockCard(dateBegin, dateEnd,templateID) {
    $.ajax({
        type: 'GET',
        datatype: 'JSON',
        data: {
            dateBegin: dateBegin,
            dateEnd: dateEnd,
            templateID: templateID,
        },
        url: '/Shop_Goods/ViewStockCard',
        success: function (result) {
            //console.log(result);
            $('#tb tbody').html(result);
        }
    })
}