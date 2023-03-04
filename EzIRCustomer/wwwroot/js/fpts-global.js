
(function () {
    //2020-03-26 15:35:38 PhucVM
    function cutText() {
        // lay du lieu tu cookie
        getCookie = function (name) {
            var matches = document.cookie.match(new RegExp(
                "(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
            ))
            return matches ? decodeURIComponent(matches[1]) : undefined
        }
        //set
        setCookie = function (name, data) {
            var expDate = new Date();
            expDate.setTime(expDate.getTime() + 30 * 24 * 60 * 60 * 1000);
            document.cookie = name + '=' + data + ';expires=' + expDate.toGMTString() + 'path=/';
        }

        //gán 
        if (getCookie("datecount") != null) {
           
            var as = getCookie("datecount");
            $('#counttext').html(getCookie("datecount"));
            var string_date = $('#counttext').text();
            var maxlength = 10;
            var trimmedString = string_date.substring(0, maxlength);

            setCookie("dateCookies", trimmedString);
            $('#datecount').html(getCookie("dateCookies"));
        }
        //gán và lấy ra
        if (getCookie("timeText")) {
            $('#Timecount').html(getCookie("timeText"));
        }

        var textTime = getCookie("timeText");
        //bóc tách date để gán vào js dùng cho count down
        var h = textTime.toString().split(':')[0]
        var m = textTime.toString().split(':')[1]
        var dd = trimmedString.toString().split('/')[0]
        var mm = trimmedString.toString().split('/')[1]
        var yyyy = trimmedString.toString().split('/')[2]

        //count down

        const second = 1000,
            minute = second * 60,
            hour = minute * 60,
            day = hour * 24;
        var datetime = yyyy + "-" + mm  + "-" + dd + "T" + h + ":" + m + ":00";
        var datetimes = '03 16,2020 15:00:00'; //test 
        let countDown = new Date(datetime).getTime(),
            x = setInterval(function () {

                let now = new Date().getTime(),
                    distance = countDown - now;
                if (distance > 0 || now < getCookie("dateCookies")) {
                    //var textStatus = document.getElementsByClassName('home_status_process')[0].innerHTML;

                    document.getElementById('days').innerText = Math.floor(distance / (day)),
                        document.getElementById('hours').innerText = Math.floor((distance % (day)) / (hour)),
                        document.getElementById('minutes').innerText = Math.floor((distance % (hour)) / (minute)),
                        document.getElementById('seconds').innerText = Math.floor((distance % (minute)) / second);
                    //document.getElementById('textSattus').innerText = textStatus;
                    //document.getElementById("textSattus").classList.add("tienhanh");
                    //document.getElementById("textSattus").classList.remove("error");
                    $('.bqbc-btn-submit').show();
                    $('.btnModify').show();
                    $('.bqbc-tutorial').show();


                } else {
                    document.getElementById("textSattus").classList.remove("hidden");
                    if (document.body.contains(document.getElementById('home_status_upcoming'))) {
                        document.getElementById("home_status_upcoming").classList.add("hidden");
                    }

                    if (document.body.contains(document.getElementById('home_status_process'))) {
                        document.getElementById("home_status_process").classList.add("hidden");
                    }
                    var textStatus = document.getElementsByClassName('home_status_end')[0].innerHTML;

                    document.getElementById('days').innerText = "00",
                        document.getElementById('hours').innerText = "00",
                        document.getElementById('minutes').innerText = "00",
                        document.getElementById('seconds').innerText = "00";
                    document.getElementById('textSattus').innerText = textStatus;
                    //document.getElementById("textSattus").classList.add("error");
                    //document.getElementById("textSattus").classList.remove("tienhanh");
                    $('.bqbc-btn-submit').hide(); $('.btnModify').hide();
                    $('.bqbc-tutorial').hide();
                }
            }, second)
    }

    function main() {
        cutText();
    };
    main();
})();