$(function() {

    $(window).scroll(function() {

        if ($(window).width() > 1024 || $(window).height() > 1366) {

            if ($(document).scrollTop() > 80) {

                document.getElementById("menu_lv1").style.position = "fixed";
                document.getElementById("menu_lv1").style.right = "0";
                document.getElementById("menu_lv1").style.top = "0";
                document.getElementById("menu_lv1").style.left = "0";
                document.getElementById("menu_lv1").style.zIndex = "1";
               
                document.getElementById("company_search").style.display = "none";
                document.getElementById("txtcompany").style.height = "30px";
            } else {
                document.getElementById("menu_lv1").style.position = "relative";
                document.getElementById("company_search").style.display = "inline-block";
                document.getElementById("txtcompany").style.height = "40px";
            }
        } else {

            document.getElementById("menu_lv1").style.zIndex = "100";
        }
    });
});