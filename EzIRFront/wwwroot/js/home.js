//Hàm kiểm tra ấn enter input search
function enterSearch(e) {
    //See notes about 'which' and 'key'
    if (e.keyCode == 13) {
        //debugger;
        var valSerach = document.getElementById("fpts-search-stockcode").value;

        if (valSerach != null && valSerach != "") {
            // viet vao day
            var json = JSON.parse(sessionStorage.getItem('dataSearch'));
            for (var i = 0; i < json.length; i++) {
                if (valSerach.toUpperCase() === json[i].astock_CODE.toUpperCase()) {
                    return window.location.href = `/ThongTinDoanhNghiep/${valSerach.toUpperCase()}`;
                } else if (valSerach === json[i].nameSearch) {
                    return window.location.href = `/ThongTinDoanhNghiep/${json[i].astock_CODE.toUpperCase()}`;
                }
            }
        }

        return false;
    }

    //Change css search'
    $('#fpts-search-stockcode').keyup(function () {
        $('.fptsSearchAuto').css('font-weight', 'bold');
        $('.fptsSearchAuto').addClass('borderradius');

        if ($(this).val() == '') {
            $('.fptsSearchAuto').removeClass('borderradius');
            $('.easy-autocomplete-container ul').removeClass('d-block');
            $('.easy-autocomplete-container ul').addClass('d-none');
            $(".fptsSearchAuto").removeClass("borderradius_bot");
            $(this).css('border-bottom-left-radius', '27px');
            $(this).css('border-bottom-right-radius', '27px');

        } else {
            if ($('.easy-autocomplete-container ul').css('display') == 'none') {
                $('.easy-autocomplete-container ul').removeClass('d-none');
                $('.easy-autocomplete-container ul').addClass('d-block');
                $('.fptsSearchAuto').removeClass('borderradius');
                $(this).css('border-bottom-left-radius', '0px');
                $(this).css('border-bottom-right-radius', '0px');

            }
        }
    });

    $('#fpts-search-stockcode').focus(function () {

        $('.easy-autocomplete-container').show();
       

        if ($(this).val() != '') {
            $(".fptsSearchAuto").addClass("borderradius");
             $(this).css('border-bottom-left-radius', '0px');
            $(this).css('border-bottom-right-radius', '0px');
        }
        if ($(".easy-autocomplete-container ul").css('display') == 'none') {
            $(".fptsSearchAuto").removeClass("borderradius");
           
        }



    });


    $('.easy-autocomplete-container ul').mouseover(function () {

        var html_code = $('.easy-autocomplete-container').find('ul li.selected div.eac-item').html();

        $('.html_codetemp').html(html_code);

    });

    $('.easy-autocomplete-container ul').mouseleave(function () {

        $('.html_codetemp').empty();

    });


    //Click chuột ra bên ngoài ô input
    $("#fpts-search-stockcode").blur(function () {

        $('.easy-autocomplete-container').hide();

        $(".fptsSearchAuto").addClass("borderradius_bot");

        if ($(this).val() != '') {
            $(this).css('border-bottom-left-radius', '27px');
            $(this).css('border-bottom-right-radius', '27px');
            $(".fptsSearchAuto").addClass("borderradius");
            $('.easy-autocomplete-container ul').addClass('d-block');   
        }

        else {

            $('.easy-autocomplete-container ul').removeClass('d-block');

            $('.easy-autocomplete-container ul').addClass('d-none');

            if ($(".easy-autocomplete-container ul").css('display') == 'none') {

                $(".fptsSearchAuto").removeClass("borderradius");
            }

            if ($(".easy-autocomplete-container").css('display') == 'none') {

                $(".fptsSearchAuto").removeClass("borderradius_bot");
                $(".fptsSearchAuto").removeClass("borderradius");
            }

        }

   
        if ($('.html_codetemp').html() != '') {
            $('#btn_unset').trigger('click');
        }
           

    });

   

};

//Hàm kiểm tra ấn enter input search
function enterSearchHome(e) {
    //See notes about 'which' and 'key'
    if (e.type == 'click' || e.keyCode == 13) {


        e.preventDefault();

        //debugger;
        var valSerach = document.getElementById("fpts-search-stockcode").value;

        if (valSerach != null && valSerach != "") {
            // viet vao day
            var json = JSON.parse(sessionStorage.getItem('dataSearchHome'));
            for (var i = 0; i < json.length; i++) {
                if (valSerach.toUpperCase() === json[i].astock_CODE.toUpperCase()) {
                    if (e.keyCode == 13) {
                        movetodetail(json[i].astock_CODE.toUpperCase());
                        return false;
                    }

                    return window.location.href = `/ThongTinDoanhNghiep/${json[i].astock_CODE.toUpperCase()}`;
                } else if (valSerach === json[i].name) {
                    if (e.keyCode == 13) {
                        movetodetail(json[i].astock_CODE.toUpperCase());
                        return false;
                    }

                    return window.location.href = `/ThongTinDoanhNghiep/${json[i].astock_CODE.toUpperCase()}`;
                }
            }
        }

        return false;
    }

    //Change css search'
    $('#fpts-search-stockcode').keyup(function () {
        $('.fptsSearchAuto').css('font-weight', 'bold');
        $('.fptsSearchAuto').addClass('borderradius');

        if ($(this).val() == '') {
            $('.fptsSearchAuto').removeClass('borderradius');
            $('.easy-autocomplete-container ul').removeClass('d-block');
            $('.easy-autocomplete-container ul').addClass('d-none');
            $(".fptsSearchAuto").removeClass("borderradius_bot");
            $(this).css('border-bottom-left-radius', '40px');
            $(this).css('border-bottom-right-radius', '40px');

        } else {
            if ($('.easy-autocomplete-container ul').css('display') == 'none') {
                $('.easy-autocomplete-container ul').removeClass('d-none');
                $('.easy-autocomplete-container ul').addClass('d-block');
                $('.fptsSearchAuto').removeClass('borderradius');
                $(this).css('border-bottom-left-radius', '0px');
                $(this).css('border-bottom-right-radius', '0px');

            }
        }
    });

    $('#fpts-search-stockcode').focus(function () {

        $('.easy-autocomplete-container').show();


        if ($(this).val() != '') {
            $(".fptsSearchAuto").addClass("borderradius");
            $(this).css('border-bottom-left-radius', '0px');
            $(this).css('border-bottom-right-radius', '0px');
        }
        if ($(".easy-autocomplete-container ul").css('display') == 'none') {
            $(".fptsSearchAuto").removeClass("borderradius");

        }



    });


    $('.easy-autocomplete-container ul').mouseover(function () {

        var html_code = $('.easy-autocomplete-container').find('ul li.selected div.eac-item').html();

        $('.html_codetemp').html(html_code);

    });

    $('.easy-autocomplete-container ul').mouseleave(function () {

        $('.html_codetemp').empty();

    });


    //Click chuột ra bên ngoài ô input
    $("#fpts-search-stockcode").blur(function () {

        $('.easy-autocomplete-container').hide();

        $(".fptsSearchAuto").addClass("borderradius_bot");

        if ($(this).val() != '') {
            $(this).css('border-bottom-left-radius', '27px');
            $(this).css('border-bottom-right-radius', '27px');
            $(".fptsSearchAuto").addClass("borderradius");
            $('.easy-autocomplete-container ul').addClass('d-block');
        }

        else {

            $('.easy-autocomplete-container ul').removeClass('d-block');

            $('.easy-autocomplete-container ul').addClass('d-none');

            if ($(".easy-autocomplete-container ul").css('display') == 'none') {

                $(".fptsSearchAuto").removeClass("borderradius");
            }

            if ($(".easy-autocomplete-container").css('display') == 'none') {

                $(".fptsSearchAuto").removeClass("borderradius_bot");
                $(".fptsSearchAuto").removeClass("borderradius");
            }

        }


        if ($('.html_codetemp').html() != '') {
            $('#btn_unset').trigger('click');
        }


    });



}

function movetodetail(valSerach) {
    /*window.location.href = `/ThongTinDoanhNghiep/${valSerach.toUpperCase()}`;*/
    setTimeout(function () { document.location.href = `/ThongTinDoanhNghiep/${valSerach.toUpperCase()}` }, 1);
    return false;
}

// Sự kiện tab công bố thông tin
function Open_tab() {
    $('#tindoanhnghiepcongbo').removeClass('d-none');
    $('#tintuc').addClass('d-none');
    $('#tintucir').removeClass('active').addClass('active');
    $('#bantinir').removeClass('active');
}

function Close_tab() {
    $('#tindoanhnghiepcongbo').addClass('d-none');
    $('#tintuc').removeClass('d-none');
    $('#tintucir').removeClass('active');
    $('#bantinir').addClass('active');
}

//Lăn màn hình lên đầu trang
function topFunction() {
    document.body.scrollTop = 0; // For Safari
    document.documentElement.scrollTop = 0; // For Chrome, Firefox, IE and Opera
}

// box1
// a: advise_box-more(a) - r: advise_title_(r)
function openMore(a, r) {
    if (a == 0) a = '';
    $('.advise_box').not('#advise_box-default').addClass('d-none');
    $('.advise_title').not('.advise_title_1').addClass('d-none');
    $('#advise_box-more' + a).removeClass('d-none');
    $('.advise_title_' + r).removeClass('d-none');
    header.style.background = "transparent";
    navbar.style.background = "transparent";
    header.style.transition = "unset";
    document.body.style.overflow = "hidden";
    topFunction();
    hide1.style.height = "0px";
    modalTVDN.style.display = "block";
    //$('#fv-dots').addClass('d-none');
    $('.close-btn-tvdn' + a).show();
    //$('#fv-dots a').addClass('href-none');
    $('#fp-nav a').addClass('href-none');
    $('.display-change').addClass('d-none');
    $('.nav-bar_child').addClass('href-none');
    //displayElement();
}

// a: advise_box-more(a) - r: advise_title_(r)
function closeMore(a, r) {
    if (a == 0) a = '';
    $('#advise_box-more' + a).addClass('d-none');
    $('.advise_title_' + r).addClass('d-none');
    document.body.style.overflow = "visible";
    modalTVDN.style.display = "none";
    //$('#fv-dots').removeClass('d-none');
    $('.close-btn-tvdn' + a).hide();
    header.style.background = header_color; /*"#ebebeb";*/
    navbar.style.background = header_color;
    hide1.style.height = "auto";
    //$('#fv-dots a').removeClass('href-none');
    $('#fp-nav a').removeClass('href-none');
    $('.display-change').removeClass('d-none');
    $('.nav-bar_child').removeClass('href-none');
}

// Đóng khi nhấp chuột vào bất cứ khu vực nào trên màn hình
window.onclick = function (e) {
    //Tư vấn doanh nghiệp
    if (e.target == modalTVDN) {
        document.body.style.overflow = "visible";
        modalTVDN.style.display = "none";
        header.style.background = header_color;
        navbar.style.background = header_color;
        $('.closed-button').hide();
        //$('#fv-dots a').removeClass('href-none');
        $('#fp-nav a').removeClass('href-none');
        $('.display-change').removeClass('d-none');
        $('.nav-bar_child').removeClass('href-none');
    }

    //Đối tác doanh nghiệp
    if (e.target == modal) {
        document.body.style.overflow = "visible";
        document.querySelector(".hero-heading").style.zIndex = '0';
        document.querySelector(".slick-slide").style.filter = 'unset';
        modal.style.display = "none";
        hide1.style.background = "none";
        header.style.background = header_color;
        navbar.style.background = header_color;
        //$('#fv-dots a').removeClass('href-none');
        $('#fp-nav a').removeClass('href-none');
        $('.display-change').removeClass('d-none');
        $('.nav-bar_child').removeClass('href-none');
    }
}

//Event ấn chọn mã chứng khoán từ ô gợi ý tìm kiếm
$('#btn_unset').click(function (e) {

    e.preventDefault();

    var code = $('.html_codetemp').find('input[name="astock_CODE"]').val();

    $.ajax({
        url: 'HomeDemo/SelectStockCode',
        method: 'POST',
        dataType: 'json',
        data: code,
        error: function (err) {
            console.log(err);
        }
    });
});
