

(function () {
    function getData() {
        var j, i;

        var html = ` <section id="bieuquyet" class="section-content">
                        <div class="noidung-bieuquyet">`
        html = html + `  <p class="title title-content">Biểu Quyết</p>`
        html = html + ` <div class="title-index">`

        html = html + ` <p class="title-content">Số lượng cổ phiếu đại diện: ${vDataBieuQuyet[0][0].slcp_bieuquyet}</p>`

        html = html + `</div>
                            <from>
                                <table class="table table-hover tbl-noidung">

                                    <tr>
                                        <th><p>Mã nội dung</p></th>
                                        <th class="content"><p>Nội dung</p></th>
                                        <th><p>Tán Thành</p></th>
                                        <th><p>Không tán thành</p></th>
                                        <th><p>Không ý kiến</p></th>
                                    </tr>`
        for (j = 0; j < vDataBieuQuyet[0].length; j++) {
            html = html + ` <tr>`
            html = html + ` <td class="before-tr"><p>${vDataBieuQuyet[0][j].noiDungID}</p></td>`
            html = html + ` <td class="content"><p>${vDataBieuQuyet[0][j].noiDungDesc}</p></td>`
            html = html + ` <td><p><input type="checkbox" id="" data-id="${vDataBieuQuyet[0][j].noiDungID}" name="tanthanh"></p></td>
                                 <td><p><input type="checkbox" id="" data-id="${vDataBieuQuyet[0][j].noiDungID}" name="khongtanthanh"></p></td>
                                 <td><p><input type="checkbox" id="" data-id="${vDataBieuQuyet[0][j].noiDungID}" name="khongykien"></p></td>
                              </tr>`
        }
        html = html + ` </table>
                                <div class="footer-tbl--btn">
                                    <p class="btn btn-default btn-submit" id="btnBieuphi">Biểu quyết</p>
                                </div>
                            </from>
                        </div>
                    </section>`


        var htmlP = `
                    <section id="quantri" class="section-content">
                        <div class="noidung-bieuquyet baunhansu">`
        var htmlP = htmlP + `<p class="title title-content">${vDataBauCu[0][0].noiDungTitle}</p>`
        var htmlP = htmlP + ` <div class="title-index">`

        var htmlP = htmlP + `<p class="title-content">Số lượng cổ phiếu đại diện: ${vDataBauCu[0][0].slcp_DaiDien}</p>`
        var htmlP = htmlP + `<p class="title-content">Số lượng thành viên bầu: ${vDataBauCu[0][0].soTVBau}</p>`
        var htmlP = htmlP + `<p class="title-content">Số lượng quyền bẩu cử tương ứng: ${vDataBauCu[0][0].quyenBau}</p>`
        var htmlP = htmlP + `</div>
                            <from>
                                <table class="table table-hover tbl-noidung">

                                    <tr>
                                        <th><p>Họ và tên ứng viên</p></th>
                                        <th><p>Bầu dồn phiếu</p></th>
                                        <th><p>Số phiếu bầu</p></th>
                                    </tr>`
        var htmlP = htmlP + `<input type="text" id="countcheck" name="" value="${vDataBauCu[0][0].quyenBau}" hidden>`
        var htmlP = htmlP + ` <tr>`
        for (j = 0; j < vDataBauCu[0].length; j++) {
            var htmlP = htmlP + `<td class="before-tr"><p>${vDataBauCu[0][j].ungCuVienName}</p></td>`
            var htmlP = htmlP + ` <td><p><input type="checkbox" id="" data-idungvien ="${vDataBauCu[0][j].ungCuVienID}" name=""></p></td>`
            var htmlP = htmlP + ` <td><p><input type="text" data-id="" data-name="" data-idungvien ="${vDataBauCu[0][j].ungCuVienID}" class="form-control input-noidungbau countSoPhieu"></p></td>
                                 </tr>`
        }
        var htmlP = htmlP + `</table>
                                <div class="footer-tbl--btn">
                                    <p class="btn btn-default btn-submit" id="btnquantri">Biểu quyết</p>
                                </div>
                            </from>
                        </div>
                    </section>
                    `

        var htmlH = `
                     <section id="kiemsoat" class="section-content">
                        <div class="noidung-bieuquyet baunhansu">`
        var htmlH = htmlH + ` <p class="title title-content">${vDataBauCu[1][0].noiDungTitle}</p>`
        var htmlH = htmlH + ` <div class="title-index">`
        var htmlH = htmlH + `<p class="title-content"> Số lượng cổ phiếu đại diện: ${vDataBauCu[1][0].ungCuVienName}</p> `
        var htmlH = htmlH + `<p class="title-content"> Số lượng thành viên bầu: ${vDataBauCu[1][0].soTVBau}</p > `
        var htmlH = htmlH + `<p class="title-content"> Số lượng quyền bẩu cử tương ứng: ${vDataBauCu[1][0].quyenBau}</p> `
        var htmlH = htmlH + `</div>
                            <from>
                                <table class="table table-hover tbl-noidung">

                                    <tr>
                                        <th><p>Họ và tên ứng viên</p></th>
                                        <th><p>Bầu dồn phiếu</p></th>
                                        <th><p>Số phiếu bầu</p></th>
                                    </tr>`
        for (i = 0; i < vDataBauCu[1].length; i++) {

            var htmlH = htmlH + `       <tr>`
            var htmlH = htmlH + `       <td class="before-tr"><p>${vDataBauCu[1][i].ungCuVienName}</p></td>`
            var htmlH = htmlH + `       <td><p><input type="checkbox" id="" name="2" data-idType ="${vDataBauCu[1][i].noiDungTypeID}" data-idungvien ="${vDataBauCu[1][i].ungCuVienID}"></p></td>`
            var htmlH = htmlH + `       <td><p><input type="text" data-id="" data-name="" class="form-control input-noidungbau" data-idType ="${vDataBauCu[1][i].noiDungTypeID}" data-idungvien ="${vDataBauCu[1][i].ungCuVienID}"></p></td>
                                    </tr>`
        }
        var htmlH = htmlH + `</table>
                                <div class="footer-tbl--btn">
                                    <p class="btn btn-default btn-submit" id="btnkiemsoat">Biểu quyết</p>
                                </div>
                            </from>
                        </div>
                    </section>
        `
        $("#fptsvote").append(html + htmlP + htmlH);
        //  $("#fptsvote").html(htmlP);
    }

    function submitData() {
        //submit
        $("#btnBieuphi").click(function () {

            var form_data = { "purchaserightcode": purchaserightcode };

            var post_url = url;
            var request_method = "post";
            var form_data = form_data;

            $.ajax({
                url: post_url,
                type: request_method,
                data: JSON.stringify(form_data),
                contentType: "application/json; charset=utf-8",
                dataType: "json"
            }).done(function (response) {
                console.log("thành công");
            }).fail(function () {
                console.log("không thành công");
            });
        });
    }

   

    function main() {
        getData();

    };
    main();
})();