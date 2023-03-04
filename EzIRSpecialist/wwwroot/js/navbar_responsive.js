
jQuery(document).ready(function ($) {

    var alterClass = function () {
        var ww = document.body.clientWidth;
        if (ww < 900) {
            // 		var attr = $('#dropdownMenu2').attr('data-toggle');
            // 		if (attr == undefined) {
            // 	$('a[id=dropdownMenu2]').attr("data-toggle", "dropdown");
            // }
            $('#sidebar-menu').addClass('navbar-nav mr-auto');
            $('#k_drdwn_mng_lv1').removeClass('dropdown');
            $('#k_drdwn_mng_lv2').removeClass('f1_dropdown');
            $('#k_drdwn_ex_lv1').removeClass('dropdown');
            $('#k_drdwn_ex_lv2').removeClass('f1_dropdown');
            $('.dropdownMenu1').removeClass("disabled");
            
            $("ul.dropdown-menu [data-toggle='dropdown']").on("click", function (event) {
                event.preventDefault();
                event.stopPropagation();

                $(this).siblings().toggleClass("show");

                if (!$(this).next().hasClass('show')) {
                    $(this).parents('.dropdown-menu').first().find('.show').removeClass("show");
                }
                $(this).parents('li.nav-item.dropdown.show').on('hidden.bs.dropdown', function (e) {
                    $('.dropdown-submenu .show').removeClass("show");
                });
            })
        } else if (ww >= 901) {
            $("ul.dropdown-menu [data-toggle='dropdown']").click(function () { return false; });
            // $('a[id=dropdownMenu2]').removeAttr('data-toggle');
            $('#sidebar-menu').removeClass('navbar-nav mr-auto');
            $('#k_drdwn_mng_lv1').addClass('dropdown');
            $('#k_drdwn_mng_lv2').addClass('f1_dropdown');
            $('#k_drdwn_ex_lv1').addClass('dropdown');
            $('#k_drdwn_ex_lv2').addClass('f1_dropdown');
            $('.dropdownMenu1').addClass("disabled");
            
        };
    }

    $(window).resize(function () {
        alterClass();
    });
    alterClass();
})