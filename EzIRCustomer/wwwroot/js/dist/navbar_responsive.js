jQuery(document).ready( function ($) {

	  	var alterClass = function() {
	  		var ww = document.body.clientWidth;
	  		if (ww < 900) {
	      		$('#sidebar-menu').addClass('navbar-nav mr-auto');
	      		$('#k_drdwn_mng_lv1').removeClass('dropdown');
	      		$('#k_drdwn_mng_lv2_1').removeClass('f1_dropdown');
	      		$('#k_drdwn_mng_lv2_2').removeClass('f2_dropdown');
	      		$('#k_drdwn_ex_lv1').removeClass('dropdown');
	      		$('#k_drdwn_ex_lv2').removeClass('f1_dropdown');
	      		$('#k_drdwn_ex_lv2').removeClass('f2_dropdown');
	      		$("#dropdownMenu1").on("click", function(event) {
					event.preventDefault();
					event.stopPropagation();

					$(this).siblings().toggleClass("show");

					if (!$(this).next().hasClass('show')) {
				      	$(this).parents('.dropdown-menu').first().find('.show').removeClass("show");
				    }
				    $(this).parents('li.nav-item.dropdown.show').on('hidden.bs.dropdown', function(e) {
				    	$('.dropdown-submenu .show').removeClass("show");
				    });
				})
	      		$("ul.dropdown-menu [data-toggle='dropdown']").on("click", function(event) {
					event.preventDefault();
					event.stopPropagation();

					$(this).siblings().toggleClass("show");

					if (!$(this).next().hasClass('show')) {
				      	$(this).parents('.dropdown-menu').first().find('.show').removeClass("show");
				    }
				    $(this).parents('li.nav-item.dropdown.show').on('hidden.bs.dropdown', function(e) {
				    	$('.dropdown-submenu .show').removeClass("show");
				    });
				})
	    	} else if (ww >= 901) {
	    		$("#dropdownMenu1").click(function(){return false;});
	    		$("ul.dropdown-menu [data-toggle='dropdown']").click(function(){return false;});
	      		$('#sidebar-menu').removeClass('navbar-nav mr-auto');
	      		$('#k_drdwn_mng_lv1').addClass('dropdown');
	      		$('#k_drdwn_mng_lv2_1').addClass('f1_dropdown');
	      		$('#k_drdwn_mng_lv2_2').addClass('f2_dropdown');
	      		$('#k_drdwn_ex_lv1').addClass('dropdown');
	    	};
	  	}

	  	$(window).resize(function(){
		    	alterClass();
		  	});
		  	alterClass();
})