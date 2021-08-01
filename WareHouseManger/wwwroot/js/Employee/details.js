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
var ctx2 = document.getElementById("myAreaChart2");


$(document).ready(function () {
    var date = new Date();
    $('#month').val(date.getMonth() + 1);

    $('#btn-accept').click(function () {
        changePasswordConfirmed();
        $('#msg-changepass').attr('hidden', 'hidden');
    });

    $('#btn-accept-forgotpassword').click(function () {
        forgotPasswordConfirmed();
        $('#msg-changepass1').attr('hidden', 'hidden');
    });

    getCountRecepitShopGoodsGroupByEmployee();

    $('#btn-submit-chart').click(function () {
        getCountRecepitShopGoodsGroupByEmployee();
    });

    $('#datetype').change(function () {
        if ($(this).val() == 'month') {
            $('#month').removeAttr('disabled');
        } else {
            $('#month').attr('disabled', 'disabled');
        }
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

function getCountRecepitShopGoodsGroupByEmployee() {
    var month = [];
    var sumIssues = [];

    $.ajax({
        type: 'POST',
        datatype: 'JSON',
        data: {
            id: $('#EmployeeID').val(),
            type: $('#datetype').val(),
            month: $('#month').val(),
            year: $('#year').val(),
        },
        url: '/Home/GetCountRecepitShopGoodsGroupByEmployee',
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
                var sumIssue = item.value;

                month.push(title + item.index);
                sumIssues.push(sumIssue);

            });

            myBarChart.data.labels = month;
            myBarChart.data.datasets[0].data = sumIssues;
            /* myLineChart2.data.datasets[2].data = inventory;*/
            myBarChart.update();
        }
    })
}

var myBarChart = new Chart(ctx2, {
    type: 'bar',
    data: {
        labels: [],
        datasets: [{
            label: "Doanh thu",
            backgroundColor: "#4e73df",
            hoverBackgroundColor: "#2e59d9",
            borderColor: "#4e73df",
            data: [],
        },       
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
                    return datasetLabel + ': ' + number_format(tooltipItem.yLabel) + ' VNĐ';
                }
            }
        },
    }
});
