$(function() {

    $(window).scroll(function() {

        if ($(window).width() > 1024 || $(window).height() > 1366) {

            if ($(document).scrollTop() > 90) {

                document.getElementById("menu_lv1").style.position = "fixed";
                document.getElementById("menu_lv1").style.right = "0";
                document.getElementById("menu_lv1").style.top = "0";
                document.getElementById("menu_lv1").style.left = "0";
                document.getElementById("menu_lv1").style.zIndex = "1";
                document.getElementById("menu_lv1").style.marginTop = "0px";
                document.getElementById("k_header_menu_wrapper2").style.zIndex = "0";
                document.getElementById("company_search").style.display = "none";
                document.getElementById("txtcompany").style.height = "30px";
            } else {
                document.getElementById("menu_lv1").style.position = "relative";
                document.getElementById("company_search").style.display = "inline-block";
                document.getElementById("txtcompany").style.height = "40px";
                document.getElementById("menu_lv1").style.marginTop = "5px";
            }
        } else {

            document.getElementById("menu_lv1").style.zIndex = "100";
        }


    });

    /*Collapse_Expand_Table_Finannce*/

    $(function() {

        if ($(window).width() <= 1024) {

            $('.trfirst').addClass('hExpanded');

            var shown = false;
            $('.trfirst').click(function(e) {
                if (!shown) {
                    $('.hidden_row td').slideDown(0);

                    $(this).removeClass('.hCollapsed').addClass('hExpanded')
                } else {
                    $('.hidden_row td').slideUp(0);
                    $(this).removeClass('hExpanded').addClass('hCollapsed');
                }
                shown = !shown;
            });
        } else {
            $('.trfirst').removeClass('hExpanded');
        }
    });

});