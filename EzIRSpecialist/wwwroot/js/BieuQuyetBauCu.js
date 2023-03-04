(function () {
	selfSearch = this;
	$('.bauCuSubmit').click(function SetValueSoQuyenBau() {
		var form = $(this).closest('form');
		var nameTitle = form.parent().find('.titleName').html();
		var load = $(this).closest('.section-content').find('#load');


		console.log($(this).closest('form').find('.quyenInput'));
		// Thay thế dấu chấm trong giá trị, 2.000.000 => 2000000
		var sq = $(this).closest('form').find('.quyenInput');
		for (var i = 0; i < sq.length; i++) {
			var sqValue = sq[i].value.replace(/\./g, '');
			sq[i].value = sqValue;

			if (sq[i].value == "") {
				sq[i].style.color = 'white';
				sq[i].value = -1;
			}
		};
		// Submit form
		load.addClass('bqbc-loading');
		form.submit();
		
		
	});
	$('.bieuQuyetSubmit').click(function SetValueSoQuyenBau() {
		var form = $(this).closest('form');
		var nameTitle = form.parent().find('.titleName').html();
		var load = $(this).closest('.section-content').find('#load');
		


		load.addClass('bqbc-loading');
		
		$(this).closest('form').submit();
		
		
		
	});

	// init cac code xu ly su kien cua cac DOM tren form
	function bqbcDataTable() {
		$('.table-bqbc').DataTable({
			"lengthChange": false,
			"searching": false,
			"pageLength": 50,
			"ordering": false,
			"paging": false,
			"info": false
		});
	}
	// Sắp xếp các phiếu chưa điền lên trên đầu
	function sapXepPhieuTrong() {
		var elem = $('#fptsvote').find('section').sort(sortMe);
		$('#fptsvote').append(elem);


	};

	function sortMe(a, b) {
		return a.className < b.className ? -1 : 1;
	}
	function checkAll() {
		$('.checkbox-yes').click(function () {
			$('.checkbox-na, .checkbox-no').prop('checked', false);
			var listIpnut = $(this).closest('table').find('.tick-yes');
			if ($(this).is(":checked")) {
				for (var i = 0; i < listIpnut.length; i++) {
					$(listIpnut[i]).prop('checked', true)
				}
			} else {
				for (var i = 0; i < listIpnut.length; i++) {
					$(listIpnut[i]).prop('checked', false)
				}
			}
		});
		$('.checkbox-no').click(function () {
			$('.checkbox-yes, .checkbox-na').prop('checked', false);

			var listIpnut = $(this).closest('table').find('.tick-no');
			if ($(this).is(":checked")) {
				for (var i = 0; i < listIpnut.length; i++) {
					$(listIpnut[i]).prop('checked', true)
				}
			} else {
				for (var i = 0; i < listIpnut.length; i++) {
					$(listIpnut[i]).prop('checked', false)
				}
			}
		});
		$('.checkbox-na').click(function () {
			$('.checkbox-yes, .checkbox-no').prop('checked', false);

			var listIpnut = $(this).closest('table').find('.tick-na');
			if ($(this).is(":checked")) {
				for (var i = 0; i < listIpnut.length; i++) {
					$(listIpnut[i]).prop('checked', true)
				}
			} else {
				for (var i = 0; i < listIpnut.length; i++) {
					$(listIpnut[i]).prop('checked', false)
				}
			}
		});
    }
    //function checkAllBc() {
    //    $('.checkbox-dophieu').click(function () {
           
    //        var listIpnut = $(this).closest('table').find('.tick-donphieu');
    //        if ($(this).is(":checked")) {
    //            for (var i = 0; i < listIpnut.length; i++) {
    //                $(listIpnut[i]).prop('checked', true)
    //            }
    //        } else {
    //            for (var i = 0; i < listIpnut.length; i++) {
    //                $(listIpnut[i]).prop('checked', false)
    //            }
    //        }
    //    });
      
    //}
	$('.btnModify').click(function () {
		var sq = $(this).closest('form').find('input:disabled');
		var checkAllBtn = $(this).closest('form').find('.checkAll');
		var btnSubmit = $(this).closest('form').find('.bqbc-btn-submit');
		var btnModify = $(this).closest('form').find('.bqbc-modify');
		var customcheckbox = $(this).closest('form').find('.checkbox-custom');
		sq.prop("disabled", false);
		sq.removeClass('lbl-uyquyen');
		sq.removeClass('hidden');
		sq.removeClass('invisible');
		checkAllBtn.removeClass('hidden');
		customcheckbox.removeClass('invisible');
		customcheckbox.removeClass('hidden');
		customcheckbox.find('span').removeClass('bqbc-voted');
	

		btnSubmit.removeClass('hidden');
		btnModify.addClass('hidden');
		btnSubmit.css('display', 'inline-block');

	});


	this.initHandlers = function () {
		sapXepPhieuTrong();
        checkAll();
        //checkAllBc();
		//bqbcDataTable();

	}


	// tat ca init can thiet (init all)
	this.initForm = function () {
		// init event handlers
		this.initHandlers();

	}
	// contructor => auto exec
	this.initForm();
})();