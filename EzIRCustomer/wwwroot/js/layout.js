(function () {
	//Function :nút chuyển ngôn ngữ
	function changeCulture() {
		$('.culture-image').click(function () {
			$('form#selectLanguage').submit();

		});
	}
	//function checkIsLogin() {
	//	if (window.location.pathname == "/Login") {
	//		$('.header-contain').css('visibility', 'hidden');

	//		$('#page-sidebar').addClass('sidebar-hidden');

	//	} else {
	//		$('#page-sidebar').removeClass('sidebar-hidden');
	//	}
 //   }
    swal_alert = function (msg, type) {
        Swal.fire(
            '',
            msg,
            type
        );
    }






	this.initHandlers = function () {
		changeCulture();
		//checkIsLogin();
	}


	// tat ca init can thiet (init all)
	this.initForm = function () {
		// init event handlers
		this.initHandlers();

	}
	// contructor => auto exec
	this.initForm();
})()