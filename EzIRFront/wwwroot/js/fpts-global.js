//2018-11-21 15:21:26 PhucVM
function $$(id) { return document.getElementById(id); }
var MenuMobile = function () {
    self = this;
    //khai báo
    self.CLASS_BTNMENUMOBILE = '.MmenuResponsiv';
    self.ID_MOBILESHOW = '#MobileShow';
    self.ID_SEARCH = '#SearchShow';
    self.ID_MOBILEHEDEN = '#MobileHeden';
    self.ID_HOME = '#MenuResposive';
    self.CLASS_SHOWMENU = '.glyphicon-menu-hamburger';
    self.ID_MENULEFT = '#ItemLeftShowMobile';
    self.ID_SEARCHMOBILE = '#searchmobile';
    self.ID_BLOCKSEARCH = '#SearchMobile';
    self.ID_MENUCOSO = '#showcontent'; //span chứng khoán cơ sở
    self.ID_MENUPHAISINH = '#showcontent_'; //span menu chứng khoán phái sinh
    self.ID_CONTENTCOSO = '#submenumobile'
    self.ID_CONTENTPHAISINH = '#submenumobile'


    //dùng chung cho sự kiện click
    //event click menu mobile
    self.EventMenu = function () {
        $('#MenuResposive').slideToggle();
        $("#MobileShow").toggleClass("Mobileclose_");
    }
    self.EventBtnMenuLeft = function () {
        $('.MenuItemLeft').slideToggle();
        $("#ItemLeftShowMobile").toggleClass("ItemLeftMobileclose_");
    }
    //event click search mobile
    self.EventSearch = function () {
        $("#SearchShow").toggleClass("glyphicon-search");
        //$('#MenuResposive').slideToggle();
    }

    //event check with remove class boder
    self.CheckWidth = function () {
        if ($(window).width() > 1020) {
            $("#bordermobile").removeClass("boder-menumobile");
        }
    }
    //sư kiện click menu mobile phần Hỗ trợ khách hàng chứng khoán cơ sở và phái sinh
    self.CheckWidthHT = function () {
        if ($(window).width() < 1025) {
            $('#htmobile').addClass("menudown");
            //$('#submenumobile').hide();
            $('#htmobile_').addClass("menuup_");
            $('#submenumobile_').hide();

        } else {
            $('#htmobile').removeClass("menudown menuup");
            $('#htmobile_').removeClass("menudown_ menuup_");
        }

    }
    self.Showupmenu = function () {
        $("#submenumobile").slideToggle("");
        $("#htmobile").toggleClass("menuup");

    }
    self.Showupmenu_ = function () {
        $("#submenumobile_").slideToggle("");
        $("#htmobile_").toggleClass("menudown_");

    }
    //Event scroll up
    self.UpTop = function () {
        if ($(window).scrollTop() > 300) {
            $('#ScrollTop').fadeIn();
        } else {
            $('#ScrollTop').fadeOut();
        }
    }
    self.EventScroll = function () {
        $("html, body").animate({
            scrollTop: 0
        }, 600);
        return false;
    }
    //Event scroll hiden top Header
    self.EventScrollHeader = function () {
        if ($(window).width() > 0 && $(window).width() <= 1024) {
            //console.log($(window).scrollTop());
            if ($(window).scrollTop() > 30) {
                $$("fptsMenuHome").style.position = "fixed";
                $$("fptsMenuHome").style.zIndex = "50";
                $$("fptsMenuHome").style.top = "0";
                $$("fptsMenuHome").style.paddingBottom = "2px";
                $('.slogan_EzIR').css("display", "none");
                $$("fpts_EzIR").style.paddingTop = "10px";
                $(".ez-content").css("font-size", "28px");
                $(".ez-text").css("font-size", "28px");
                //icon btn menu
                $('.MmenuResponsiv').css("position", "fixed");
                $('.MmenuResponsiv').css("zIndex", "100");
                $('.MmenuResponsiv').css("top", "13px");
                $('.MmenuResponsiv').css("right", "13px");
                $('.MmenuResponsiv').css("left", "unset");
                $('.MmenuResponsiv>span').css("background", "#007db8");
                //
                $('.fptsMobileIconSearch').css("position", "fixed");
                $('.fptsMobileIconSearch').css("zIndex", "50");
                $('.fptsMobileIconSearch').css("top", "26px");
                $('.fptsMobileIconSearch').css("right", "41px");
                $('.fptsMobileIconSearch').css("left", "unset");
                $('.glyphicon-search').css("color", "#007db8");
                //icon search
                $('.fptsMobileIconSearch').css("position", "fixed");
                $('.fptsMobileIconSearch').css("zIndex", "100");
                $('.fptsMobileIconSearch').css("top", "13px");
                $('.fptsMobileIconSearch').css("right", "2px");
                $('.fptsMobileIconSearch>span').css("color", "#007db8");

                if ($(window).width() > 481 && $(window).width() <= 1024) {
                    $(".fpt-logoslogan").css("padding-left", "10%");
                    $(".fpts-ez").css("padding-top", "0px");
                    if ($(window).width() > 426 && $(window).width() <= 768) {
                        $(".fpts-ez").css("padding-top", "5px");
                    }
                    if ($(window).width() > 991) {
                        $(".img-mobile").css("padding-bottom", "6px");
                    }
                }
            }
            if ($(window).scrollTop() < 30) {
                $$("fptsMenuHome").style.position = "relative";
                $$("fptsMenuHome").style.zIndex = "unset";
                $$("fptsMenuHome").style.top = "unset";
                $('.slogan_EzIR').css("display", "inline");
                $$("fpts_EzIR").style.paddingTop = "0px";
                //
                $('.MmenuResponsiv').css("position", "absolute");
                $('.MmenuResponsiv').css("zIndex", "unset");
                $('.MmenuResponsiv').css("top", "7px");
                $('.MmenuResponsiv').css("right", "unset");
                $('.MmenuResponsiv').css("left", "13px");
                $('.MmenuResponsiv>span').css("background", "#fff");
                //
                $('.fptsMobileIconSearch').css("position", "absolute");
                $('.fptsMobileIconSearch').css("zIndex", "unset");
                $('.fptsMobileIconSearch').css("top", "6px");
                $('.fptsMobileIconSearch').css("right", "unset");
                $('.fptsMobileIconSearch').css("left", "80%");
                $('.glyphicon-search').css("color", "#fff");
                //icon search
                $('.fptsMobileIconSearch').css("position", "absolute");
                $('.fptsMobileIconSearch>span').css("color", "#fff");

                if ($(window).width() > 0 && $(window).width() < 480) {
                    $('.fptsMobileIconSearch').css("top", "8px");
                } else {
                    $('.fptsMobileIconSearch').css("top", "8px");
                    $('.fptsMobileIconSearch').css("left", "85%");
                    $('.fpt-logoslogan').css("padding-left", "0%");
                    if ($(window).width() > 426 && $(window).width() <= 768) {
                        $(".fpts-ez").css("padding-top", "5px");
                        $(".fpt-logoslogan").css("padding-left", "15px");
                    }
                    if ($(window).width() > 768) {
                        $(".fpt-logoslogan").css("padding-left", "15px");
                    }
                    if ($(window).width() > 991) {
                        $(".img-mobile").css("padding-bottom", "1%");
                        $(".fpt-logoslogan").css("padding-left", "15px");
                    }
                }
            }
        }
        if ($(window).width() > 1024 && $(window).width() <= 1920) {
            //console.log($(window).scrollTop())
            $('.fpts-header').css("position", "fixed");
            $('.fpts-header').css("zIndex", "17");
            if ($(window).scrollTop() > 30) {
                $(".img-desktop").css("padding-top", "10px");
                $(".fpts-titleEz_").css("font-size", "27px");
                $(".logoEzIR").css("font-size", "40px");
                $(".fpts-ez").css("padding-top", "15px");
                $(".ez-content").css("font-size", "38px");
                $("#fpts_EzIR").css("width", "100%");
                /*$(".fpts-slogan-EzIR").css("padding-top", "27px");*/
                $(".fpt-logoslogan").css("padding-top", "0");
                $(".fptsSearchAuto").css("height", "30px");
                if ($(window).width() > 1024 && $(window).width() <= 1185) {
                    $(".fix-content").css("padding-top", "15%");
                    $$("fptsMenuHome").style.height = "70px";
                    $(".fptsSearchAuto").css("margin-top", "20px");
                    $(".img-desktop").css("width", "70%");
                }
                if ($(window).width() > 1185 && $(window).width() <= 1680) {
                    $(".fix-content").css("padding-top", "10%");
                    $$("fptsMenuHome").style.height = "auto";
                    $(".fptsSearchAuto").css("margin-top", "13px");
                    $(".img-desktop").css("width", "50%");
                    $(".fpts-ez").css("padding-top", "3px");
                    $(".fptsSearchAuto").css("background-size", "4%");
                }
                if ($(window).width() > 1680 && $(window).width() <= 1920) {
                    $(".fix-content").css("padding-top", "5%");
                    $$("fptsMenuHome").style.height = "75px";
                    $(".fptsSearchAuto").css("margin-top", "17px");
                    $(".img-desktop").css("width", "50%");
                    $(".fptsSearchAuto").css("background-size", "4%");
                    $(".fptsSearchAuto").css("margin-top", "23px");
                    $("#fpts_EzIR").css("width", "100%");
                    $(".fpts-titleEz_").css("font-size", "27px");
                    $(".ez-content").css("font-size", "50px");
                    $(".slogan_EzIR").css("font-size", "24px");

                }
            }
            if ($(window).scrollTop() < 30) {
                $$("fptsMenuHome").style.height = "auto";
                $(".img-desktop").css("width", "85%");
                $(".img-desktop").css("padding-top", "14px");
                $(".fpt-logoslogan").css("padding-top", "10px");
                $(".fptsSearchAuto").css("background-size", "6%");
                $(".fptsSearchAuto").css("margin-top", "30px");
                $(".fptsSearchAuto").css("height", "40px");
                $(".logoEzIR").css("font-size", "66px");
                $(".fpts-ez").css("padding-top", "10px");
                if ($(window).width() > 1024 && $(window).width() <= 1680) {
                    if ($(window).width() <= 1149) {
                        $(".fix-content").css("padding-top", "11%");
                    } else if ($(window).width() > 1150 && $(window).width() <= 1321) {
                        $(".fix-content").css("padding-top", "10%");
                    } else {
                        $(".fix-content").css("padding-top", "9%");
                    }
                    if ($(window).width() <= 1240) {
                        $(".logoEzIR").css("font-size", "40px");

                        $(".fpt-logoslogan").css("padding-top", "10px");
                    } else {
                        $(".logoEzIR").css("font-size", "56px");

                    }
                    $(".fpts-titleEz_").css("font-size", "28px");
                    $(".ez-content").css("font-size", "50px");
                    $("#fpts_EzIR").css("width", "100%");
                    $(".fptsSearchAuto").css("font-size", "15px");
                }
                if ($(window).width() > 1680 && $(window).width() <= 1920) {
                    $(".fix-content").css("padding-top", "8.5%");
                    $(".fptsSearchAuto").css("margin-top", "40px");
                    $(".fpts-titleEz_").css("font-size", "24px");
                    $(".fpts-titleEz_").css("padding-top", "0px");
                    $(".ez-content").css("font-size", "64px");
                    $(".slogan_EzIR").css("font-size", "32px");
                    $("#fpts_EzIR").css("width", "100%");
                    $(".fptsSearchAuto").css("font-size", "24px");
                }

            }
        }
    }

    //Event scroll hiden top Header
    self.EventScrollHeaderDesktop = function () {

        if ($(window).width() >= 1024) {
            //console.log($(window).scrollTop());
            if ($(window).scrollTop() > 100) {
                $$("MenuResposive").style.position = "fixed";
                $$("MenuResposive").style.zIndex = "50";
                $$("MenuResposive").style.top = "0";
                $$("MenuResposive").classList.add("fptsNavBoxshadow")
            }
            if ($(window).scrollTop() < 30) {
                $$("MenuResposive").style.position = "relative";
                $$("MenuResposive").style.zIndex = "unset";
                $$("MenuResposive").style.top = "unset";
                $$("MenuResposive").classList.remove("fptsNavBoxshadow")
            }

        }
    }

    //Event Show and hiden input search Mobile -- note
    self.EventClickSearchMobile = function () {

        $('#SearchMobile').slideToggle();
    }
    self.CheckWidthImg = function () {
        var imgs = document.getElementsByClassName("fix-rickeditor").getElementsByTagName('img');
        for (var i = 0; i < imgs.length; i++) {
            console.log(imgs.length);

        }
    }
}

$(document).ready(function () {
    //call
    var $_service_ = new MenuMobile();
    self = this;

    // đóng menu cha
    $('.menu-multilevel').click(function () {
        $('.fpts-megamenu').hide();
        $(this).next().show();
        $('.fpts-menunav').animate({ left: "-100%" });
    });
    // đóng menu con
    $('.fpts-btn-premenu').click(function () {
        $('.fpts-menunav').animate({ left: "0px" });
    });

    //load 
    //_service_.EventMenu();  //load trc khi có sự kiện click nên bỏ :))
    $_service_.CheckWidth();
    $_service_.UpTop();
    $_service_.CheckWidthHT();
    // $_service_.CheckWidthImg();
    //$_service_.CheckWidthMobile();
    // _service_.EventScrollHeader();

    // global instance
    window.g_classFeedback = this;

    //Gọi sự kiện click đến ID của tab tương ứng

    $($_service_.ID_MOBILESHOW).click(function () {
        $('#SearchMobile').hide();
        $('.removesearch').hide();
        $('.iconsearch').show();
        $_service_.EventMenu();
    });

    $($_service_.ID_MENUCOSO).click(function () {
        $_service_.Showupmenu();
    });
    $($_service_.ID_MENUPHAISINH).click(function () {
        $_service_.Showupmenu_();
    });
    $($_service_.ID_MENULEFT).click(function () {
        $_service_.EventBtnMenuLeft();
    });
    //show input search
    $($_service_.ID_SEARCH).click(function () {
        $_service_.EventSearch();

    });
    //show and hiden input  Search ----- note

    $($_service_.ID_SEARCHMOBILE).click(function () {
        document.getElementById("MobileShow").classList.remove("Mobileclose_");
        $('#MenuResposive').hide();
        $_service_.EventClickSearchMobile();

        // alert('this is');
    });

    $(window).scroll(function () {
        $_service_.UpTop();
        $_service_.EventScrollHeader();
        // $_service_.EventScrollHeaderDesktop();


    });
    $('#ScrollTop').click(function () {
        $_service_.EventScroll();
    });

    //console.log($(window).width());

});

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
}

if (document.getElementById('DataByMenuLeft') && getElementsByTagName('img')) {
    var x = document.getElementById('DataByMenuLeft').getElementsByTagName('img')
    for (i = 0; i < x.length; i++) {
        if (x[i].width < '150') {
            x[i].classList.add('fixSizeImg');
            x[i].style.display = 'initial !important';
            x[i].style.marginLeft = 'unset !important';
            x[i].style.marginRight = 'unset !important';
        }
    }
}