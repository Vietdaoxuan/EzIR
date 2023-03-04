let perPage = 10;
let idPage = 1;
let start = 0;
let end = perPage;

const data = [
    { id: 1, poster: "FPT_01",posted_date: "10/06/2019 14:16",kind_of_news: "Báo cáo tài chính",title: "Báo cáo tài chính quý I/2019",download_file: "fa fa-download",action_edit:"fa fa-edit",action_delete:"fa fa-times" },
    { id: 2, poster: "FPT_02",posted_date: "10/06/2019 14:16",kind_of_news: "Điều lệ quy chế quản trị",title: "Báo cáo tài chính quý I/2019",download_file: "fa fa-download",action_edit:"fa fa-edit",action_delete:"fa fa-times" },
    { id: 3, poster: "FPT_01",posted_date: "10/06/2019 14:16",kind_of_news: "Báo cáo thường niên",title: "Báo cáo tài chính quý I/2019",download_file: "fa fa-download",action_edit:"fa fa-edit",action_delete:"fa fa-times" },
    { id: 4, poster: "FPT_01",posted_date: "10/06/2019 14:16",kind_of_news: "Báo cáo tình hình quản trị",title: "Báo cáo tài chính quý I/2019",download_file: "fa fa-download",action_edit:"fa fa-edit",action_delete:"fa fa-times" },
    { id: 5, poster: "FPT_03",posted_date: "10/06/2019 14:16",kind_of_news: "Nghị quyết/ quyết định của HĐQT",title: "Báo cáo tài chính quý I/2019",download_file: "fa fa-download",action_edit:"fa fa-edit",action_delete:"fa fa-times" },
    { id: 6, poster: "FPT_01",posted_date: "10/06/2019 14:16",kind_of_news: "Tài liệu ĐHĐCĐ",title: "Báo cáo tài chính quý I/2019",download_file: "fa fa-download",action_edit:"fa fa-edit",action_delete:"fa fa-times" },
    { id: 7, poster: "FPT_04",posted_date: "10/06/2019 14:16",kind_of_news: "Nghị quyết/ quyết định của ĐHĐCĐ",title: "Báo cáo tài chính quý I/2019",download_file: "fa fa-download",action_edit:"fa fa-edit",action_delete:"fa fa-times" },
    { id: 8, poster: "FPT_01",posted_date: "10/06/2019 14:16",kind_of_news: "Tin khác",title: "Báo cáo tài chính quý I/2019",download_file: "fa fa-download",action_edit:"fa fa-edit",action_delete:"fa fa-times" },
    { id: 9, poster: "FPT_03",posted_date: "10/06/2019 14:16",kind_of_news: "Báo cáo tài chính",title: "Báo cáo tài chính quý I/2019",download_file: "fa fa-download",action_edit:"fa fa-edit",action_delete:"fa fa-times" },
    { id: 10,poster: "FPT_01",posted_date: "10/06/2019 14:16",kind_of_news: "Tin khác",title: "Báo cáo tài chính quý I/2019",download_file: "fa fa-download",action_edit:"fa fa-edit",action_delete:"fa fa-times" },
    { id: 11,poster: "FPT_02",posted_date: "10/06/2019 14:16",kind_of_news: "Báo cáo thường niên",title: "Báo cáo tài chính quý I/2019",download_file: "fa fa-download",action_edit:"fa fa-edit",action_delete:"fa fa-times" },
    { id: 12,poster: "FPT_01",posted_date: "10/06/2019 14:16",kind_of_news: "Điều lệ quy chế quản trị",title: "Báo cáo tài chính quý I/2019",download_file: "fa fa-download",action_edit:"fa fa-edit",action_delete:"fa fa-times" },
    { id: 13,poster: "FPT_04",posted_date: "10/06/2019 14:16",kind_of_news: "Nghị quyết/ quyết định của HĐQT",title: "Báo cáo tài chính quý I/2019",download_file: "fa fa-download",action_edit:"fa fa-edit",action_delete:"fa fa-times" },
    { id: 14,poster: "FPT_01",posted_date: "10/06/2019 14:16",kind_of_news: "Báo cáo tình hình quản trị",title: "Báo cáo tài chính quý I/2019",download_file: "fa fa-download",action_edit:"fa fa-edit",action_delete:"fa fa-times" },
    { id: 15,poster: "FPT_06",posted_date: "10/06/2019 14:16",kind_of_news: "Tin khác",title: "Báo cáo tài chính quý I/2019",download_file: "fa fa-download",action_edit:"fa fa-edit",action_delete:"fa fa-times" },
    { id: 16,poster: "FPT_01",posted_date: "10/06/2019 14:16",kind_of_news: "Báo cáo thường niên",title: "Báo cáo tài chính quý I/2019",download_file: "fa fa-download",action_edit:"fa fa-edit",action_delete:"fa fa-times" },
    { id: 17,poster: "FPT_07",posted_date: "10/06/2019 14:16",kind_of_news: "Nghị quyết/ quyết định của HĐQT",title: "Báo cáo tài chính quý I/2019",download_file: "fa fa-download",action_edit:"fa fa-edit",action_delete:"fa fa-times" },
    { id: 18,poster: "FPT_01",posted_date: "10/06/2019 14:16",kind_of_news: "Điều lệ quy chế quản trị",title: "Báo cáo tài chính quý I/2019",download_file: "fa fa-download",action_edit:"fa fa-edit",action_delete:"fa fa-times" },
    { id: 19,poster: "FPT_03",posted_date: "10/06/2019 14:16",kind_of_news: "Tin khác",title: "Báo cáo tài chính quý I/2019",download_file: "fa fa-download",action_edit:"fa fa-edit",action_delete:"fa fa-times" },
    { id: 20,poster: "FPT_01",posted_date: "10/06/2019 14:16",kind_of_news: "Báo cáo tình hình quản trị",title: "Báo cáo tài chính quý I/2019",download_file: "fa fa-download",action_edit:"fa fa-edit",action_delete:"fa fa-times" },
    { id: 21,poster: "FPT_09",posted_date: "10/06/2019 14:16",kind_of_news: "Tài liệu ĐHĐCĐ",title: "Báo cáo tài chính quý I/2019",download_file: "fa fa-download",action_edit:"fa fa-edit",action_delete:"fa fa-times" },
    { id: 22,poster: "FPT_01",posted_date: "10/06/2019 14:16",kind_of_news: "Nghị quyết/ quyết định của HĐQT",title: "Báo cáo tài chính quý I/2019",download_file: "fa fa-download",action_edit:"fa fa-edit",action_delete:"fa fa-times" },
    { id: 23,poster: "FPT_011",posted_date: "10/06/2019 14:16",kind_of_news: "Báo cáo tài chính",title: "Báo cáo tài chính quý I/2019",download_file: "fa fa-download",action_edit:"fa fa-edit",action_delete:"fa fa-times" },
    { id: 24,poster: "FPT_01",posted_date: "10/06/2019 14:16",kind_of_news: "Điều lệ quy chế quản trị",title: "Báo cáo tài chính quý I/2019",download_file: "fa fa-download",action_edit:"fa fa-edit",action_delete:"fa fa-times" },
    { id: 25,poster: "FPT_012",posted_date: "10/06/2019 14:16",kind_of_news: "Nghị quyết/ quyết định của HĐQT",title: "Báo cáo tài chính quý I/2019",download_file: "fa fa-download",action_edit:"fa fa-edit",action_delete:"fa fa-times" },
    { id: 26,poster: "FPT_01",posted_date: "10/06/2019 14:16",kind_of_news: "Báo cáo tài chính",title: "Báo cáo tài chính quý I/2019",download_file: "fa fa-download",action_edit:"fa fa-edit",action_delete:"fa fa-times" },
    { id: 27,poster: "FPT_02",posted_date: "10/06/2019 14:16",kind_of_news: "Tin khác",title: "Báo cáo tài chính quý I/2019",download_file: "fa fa-download",action_edit:"fa fa-edit",action_delete:"fa fa-times" },
    { id: 28,poster: "FPT_01",posted_date: "10/06/2019 14:16",kind_of_news: "Nghị quyết/ quyết định của HĐQT",title: "Báo cáo tài chính quý I/2019",download_file: "fa fa-download",action_edit:"fa fa-edit",action_delete:"fa fa-times" },
    { id: 29,poster: "FPT_03",posted_date: "10/06/2019 14:16",kind_of_news: "Tài liệu ĐHĐCĐ",title: "Báo cáo tài chính quý I/2019",download_file: "fa fa-download",action_edit:"fa fa-edit",action_delete:"fa fa-times" },
]

let dataArr = [];
let showAdd = false;

const addBookBtn = document.getElementById('add');


const name = document.getElementById('name');
const imgLink = document.getElementById('imgLink');


function highlightText() {
    const title = document.querySelectorAll('.content__data__item h3');
    title.forEach((title, index) => {
        let titleText = title.innerHTML;
        let indexOf = Number(titleText.toLocaleLowerCase().indexOf(searchText.value.toLocaleLowerCase()));
        let searchTextLength = searchText.value.length;
        // titleText = titleText.substring(0, indexOf) + "<span class='highlight'>" + titleText.substring(indexOf, indexOf + searchTextLength) + "</span>" + titleText.substring(indexOf + searchTextLength, titleText.length);
       titleText = titleText.substring(0, indexOf) + "<span class='highlight'>" + titleText.substring(indexOf, indexOf + searchTextLength) + "</span>" + titleText.substring(indexOf + searchTextLength, titleText.length);
        
        title.innerHTML = titleText;
        console.log(titleText);
    })
}


dataArr = data;


const pageConfig = document.querySelector('.page-config select');
const mySelect = document.getElementById('mySelect');
/*const countTotalPage = document.querySelector('.total-page');*/
/*const countTotaldata = document.querySelector('.total-item');*/

let totalPages = Math.ceil(dataArr.length / perPage);

const searchText = document.querySelector('.content__search input');
const searchBtn = document.getElementById('search');


function initRender(dataAr, totalPage) {
    renderdata(dataAr);
    renderListPage(totalPage);
}

initRender(dataArr, totalPages);

function getCurrentPage(indexPage) {
    start = (indexPage - 1) * perPage;
    end = indexPage * perPage;
    totalPages = Math.ceil(dataArr.length / perPage);
    /*countTotalPage.innerHTML = `Total pages: ${totalPages}`;*/
    /*countTotaldata.innerHTML = `Total data:  ${dataArr.length}`*/
}

const deleteBtn = document.querySelectorAll('.content__data__item .delete');

deleteBtn.forEach((item, index) => {
    deleteBtn[index].addEventListener('click', () => {
        data.splice(index, 1);
        dataArr = data;
        renderdata(dataArr)
    });
});

getCurrentPage(1);

searchBtn.addEventListener('click', () => {
    idPage = 1;
    dataArr = [];
    data.forEach((item, index) => {
        if (item.poster.toLocaleLowerCase().indexOf(searchText.value.toLocaleLowerCase()) != -1){
            dataArr.push(item);
        }
        if (item.kind_of_news.toLocaleLowerCase().indexOf(searchText.value.toLocaleLowerCase()) != -1){
            dataArr.push(item);
        }
    });
    if (dataArr.length === 0) {
        $('.no-result').css('display', 'block')
    } else {
        $('.no-result').css('display', 'none')
    }
    getCurrentPage(idPage);
    initRender(dataArr, totalPages);
    changePage();
    if (totalPages <= 1) {
        $('.btn-prev').addClass('btn-active');
        $('.btn-next').addClass('btn-active');
    } else {
        $('.btn-next').removeClass('btn-active');
    }
});

searchText.addEventListener("keyup", (event) => {
    if (event.keyCode === 13) {
        event.preventDefault();
        searchBtn.click();
    }
});

/*addBookBtn.addEventListener('click', () => {
    showAdd = !showAdd;
    if (showAdd) {
        $('.add-book').css('display', 'flex');
    } else {
        $('.add-book').css('display', 'none');
    }
})*/


pageConfig.addEventListener('change', () => {
    idPage = 1;
    
    if(pageConfig.value == 0){
        perPage = data.length;
    }else {
        perPage = Number(pageConfig.value);

    }

    getCurrentPage(idPage);
    initRender(dataArr, totalPages);
    if (totalPages == 1) {
        $('.btn-prev').addClass('btn-active');
        $('.btn-next').addClass('btn-active');
    } else {
        $('.btn-next').removeClass('btn-active');
    }

    changePage();
});



function renderdata(data) {
    html = '';
    html += '<table class="table" id="data">';
    html += '<thead>';
    html += '<tr role="row" class="odd">';
    html += '<th style="color: #2999ce;font-weight: 500;">Người đăng tải</th>';
    html += '<th style="color: #2999ce;font-weight: 500;">Ngày đăng tải</th>';
    html += '<th style="color: #2999ce;font-weight: 500;">Loại tin</th>';
    html += '<th style="color: #2999ce;font-weight: 500;">Tiêu đề</th>';
    html += '<th style="color: #2999ce;font-weight: 500;">Tải file</th>';
    html += '<th style="color: #2999ce;font-weight: 500;">Hành động</th>';
    html += '</tr>';
    html += '</thead>';
    html += '<tbody>';
    const content = data.map((item, index) => {
        if (index >= start && index < end) {
            html += '<tr role="row" class="odd">';
            html += '<th style="padding-left: 20px">' + item.poster + '</th>';
            html += '<th style="padding-left: 15px">' + item.posted_date + '</th>';
            html += '<th style="padding-left: 15px">' + item.kind_of_news + '</th>';
            html += '<th style="padding-left: 20px">' + item.title + '</th>';
            html += `<th><i class="${item.download_file}" style="padding-left: 25px;"></th>`;
            html += `<th><i class="${item.action_edit}" style="padding-left: 20px;">
                         <i class="${item.action_delete} fa-delete" style="padding-left: 15px;"></th>`;
            html += '</tr>';
            return html;
        }
    });
    html += '</tbody>';
    html += '</table>';
    document.getElementById('data').innerHTML = html;
    highlightText();
}

function renderListPage(totalPages) {
    let html = '';
    html += `<li class="current-page active"><a>${1}</a></li>`;
    for (let i = 2; i <= totalPages; i++) {
        html += `<li><a>${i}</a></li>`;
    }
    if (totalPages === 0) {
        html = ''
    }
    document.getElementById('number-page').innerHTML = html;
}

function changePage() {
    const idPages = document.querySelectorAll('.number-page li');
    const a = document.querySelectorAll('.number-page li a');
    for (let i = 0; i < idPages.length; i++) {
        idPages[i].onclick = function () {
            let value = i + 1;
            const current = document.getElementsByClassName('active');
            current[0].className = current[0].className.replace('active', '');
            this.classList.add('active');
            if (value > 1 && value < idPages.length) {
                $('.btn-prev').removeClass('btn-active');
                $('.btn-next').removeClass('btn-active');
            }
            if (value == 1) {
                $('.btn-prev').addClass('btn-active');
                $('.btn-next').removeClass('btn-active');
            }
            if (value == idPages.length) {
                $('.btn-next').addClass('btn-active');
                $('.btn-prev').removeClass('btn-active');
            }
            idPage = value;
            getCurrentPage(idPage);
            renderdata(dataArr);
        };
    }
}

changePage();

$('.btn-next').on('click', () => {
    idPage++;
    if (idPage > totalPages) {
        idPage = totalPages;
    }
    if (idPage == totalPages) {
        $('.btn-next').addClass('btn-active');
    } else {
        $('.btn-next').removeClass('btn-active');
    }
    console.log(idPage);
    const btnPrev = document.querySelector('.btn-prev');
    btnPrev.classList.remove('btn-active');
    $('.number-page li').removeClass('active');
    $(`.number-page li:eq(${idPage - 1})`).addClass('active');
    getCurrentPage(idPage);
    renderdata(dataArr);
});

$('.btn-prev').on('click', () => {
    idPage--;
    if (idPage <= 0) {
        idPage = 1;
    }
    if (idPage == 1) {
        $('.btn-prev').addClass('btn-active');
    } else {
        $('.btn-prev').removeClass('btn-active');
    }
    const btnNext = document.querySelector('.btn-next');
    btnNext.classList.remove('btn-active');
    $('.number-page li').removeClass('active');
    $(`.number-page li:eq(${idPage - 1})`).addClass('active');
    getCurrentPage(idPage);
    renderdata(dataArr);
});


