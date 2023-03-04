// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// This function format DateTime(yyyy-MM-dd T HH:MM:SS) to Date(dd/MM/yyyy)
const formatDateTimeToDate = function (dateTime) {
    try {
        if (dateTime != '0001-01-01T00:00:00') {
            var date = new Date(dateTime);
            return date.getDate() + '/' + (date.getMonth() + 1) + '/' + date.getFullYear();
        }
        else {
            return '';
        }
    }
    catch {
        return '';
    }
}

// Check valid email
function isEmail(email) {
    var regex = /^([a-zA-Z0-9_.+-])+\@@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return regex.test(email);
}

// Check message
function isMessageSuccess(data) {
    if (data.code == 0) {
        return toastr.success(data.message);
    }
    return toastr.error(data.message);
}

function getFormattedDate1(date) {
    //Định dạng YYYY/MM/dd
    try {
        var tem = date.split("T")[0]
        var TemDate = tem.split("-")[0] + "-" + tem.split("-")[1] + "-" + tem.split("-")[2];
        return TemDate;
    }
    catch { }
    return "01/01/0001";
}

/**
 * Format số VD: 100000 sang 100,000
 * @param {any} num
 */
const numberWithCommas = (num) => num == null ? '' : num.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");