
//2020-03-26 10:35:38 PhucVM
(function () {
    var mediaScreen = window.matchMedia("(min-width: 600px)")

    //load page
    function loadpage() {

        if ($("#numberstatus").val() == '1' || $("#numberstatus").val() == '0' || $("#numberstatus").val() == '8') {
            $("#cancel").addClass("fpts-btn--gsmRengiter");
            $("#rengiter").removeClass("fpts-btn--gsmRengiter");
        } else if ($("#numberstatus").val() == '4') {
            $("#cancel").removeClass("fpts-btn--gsmRengiter");
            $("#rengiter").addClass("fpts-btn--gsmRengiter");
        } else {
            $("#cancel").addClass("fpts-btn--gsmRengiter");
            $("#rengiter").addClass("fpts-btn--gsmRengiter");
        }
    }
    //2020-03-26 10:35:38 PhucVM
    //đăng ký tham gia hội thảo
    //6/4/2020 HieuND
    //function rengiter() {
    //    $("#rengiter").click(function () {

    //        var request_method = "post";


    //        $.ajax({
    //            url: '/DangKyDuHop/Register',
    //            type: request_method,

    //            contentType: "application/json; charset=utf-8",
    //            dataType: "json"
    //        }).done(function (response) {
    //            console.log(response);
    //            if (response.code == "0") {
    //                $("#rengiter").addClass("fpts-btn--gsmRengiter");
    //                $("#cancel").removeClass("fpts-btn--gsmRengiter");
    //            }
    //            else {
    //                Swal.fire(
    //                    response.message, '', 'error');
    //            }
    //        }).fail(function () {
    //            console.log("Đăng ký không thành công");
    //        });
    //    });
    //}

    ////2020-03-26 12:35:38 PhucVM
    ////Hủy đăng ký tham gia hội thảo
    ////6/4/2020 HieuND
    //function cancel() {
    //    $("#cancel").click(function () {
    //        var purchaserightcode = $("#purchaserightcode").val();
    //        var maCD = $("#maCD").val();
    //        var status = "2";
    //        var form_data = { "purchaserightcode": purchaserightcode, "maCD": maCD, "status": status };

    //        var post_url = url;
    //        var request_method = "post";
    //        var form_data = form_data;

    //        $.ajax({
    //            url: '/DangKyDuHop/Cancel',
    //            type: request_method,

    //            contentType: "application/json; charset=utf-8",
    //            dataType: "json"
    //        }).done(function (response) {
    //            console.log(response);
    //            if (response.code == "0") {
    //                $("#rengiter").removeClass("fpts-btn--gsmRengiter");
    //                $("#cancel").addClass("fpts-btn--gsmRengiter");
    //            } else {
    //                Swal.fire(
    //                    response.message, '', 'error');
    //            }
    //        }).fail(function () {
    //            console.log("Hủy đăng ký không thành công")
    //        });
    //    });
    //}
    // Căn text bằng nhau
    function calcPadding(mediaScreen) {
        if (mediaScreen.matches) {
            var alignWidth = $('.org-pad1').width();
            var alignWidth2 = $('.org-pad2').width();

            var arr = $('.calc-pad1');
            var arr2 = $('.calc-pad2');
            arr.each(function () {
                var selfWidth = $(this).width();
                var padPixel = alignWidth - selfWidth;
                $(this).css('padding-left', padPixel);
            });
            arr2.each(function () {
                var selfWidth = $(this).width();
                var padPixel = alignWidth2 - selfWidth;
                $(this).css('padding-left', padPixel);
            });
        }

    }


    function main() {
        loadpage();
      
        calcPadding(mediaScreen);
       // mediaScreen.addEventListener(calcPadding);
        //rengiter();
        //cancel();
    };
    main();
})();