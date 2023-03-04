const ChatBoxConfig = {
    //variable constant for chat box
    DEFAULT: -1,
    SYSTEM: 0,
    USER: 1,
    SYSTEM_WARNING: 2,
    SYSTEM_WARNING_DO_NOT_HAVE_EMAIL: 3, // đại hội không có email nhận tin nhắn
    SYSTEM_SEND_MESSAGE: 4,
    USER_ONLINE: 1,
    USER_IS_CHECK: 0,
    QUANTITIES_DOC: 0,
    NO_FILE_APPROVED: 0,
    LANGUAGE_CODE: 'vi-VN',
    MESSAGE_THANKS_VI: `Cảm ơn Quý đại biểu đã gửi câu hỏi đến cuộc họp Đại hội đồng cổ đông. 
                        Câu hỏi của Quý đại biểu đã được Ban tổ chức ghi lại và gửi đến Chủ tọa Đại hội trong thời gian sớm nhất. 
                        Quý đại biểu vui lòng theo dõi cuộc họp Đại hội đồng cổ đông của công ty! 
                        Xin cảm ơn!`,
    MESSAGE_THANKS_EN: `Thanks for sending questions to The General Meeting of Shareholders.
                        Your questions were collected by the Organizing Commitee and would be delivered to the Chairman of GMS soon.
                        Please follow the Meeting for further information.
                        Thank you!`,

    MESSAGE_NEW_DOCUMENTS_VN: `Ban tổ chức Đại hội đã bổ sung tài liệu họp Đại hội đồng cổ đông. 
                               Quý đại biểu vui lòng tham khảo tại Tab <a href="/TaiLieuDaiHoi">Tài liệu đại hội</a>.`,
    MESSAGE_NEW_DOCUMENTS_EN: `The Organizing Commitee had just added documents for the General Meeting of Shareholders. 
                               Please read and download in <a href="/TaiLieuDaiHoi">DOCUMENTS Tab</a>. Thanks!`,

    MESSAGE_WARRING_MESSAGE_SEND_VI: `Cảnh báo: Bạn gửi tin nhắn quá nhanh, vui lòng đợi 1 phút để có thể tiếp tục gửi tin nhắn!`,

    MESSAGE_WARRING_MESSAGE_SEND_EN: `Warrning: You are sending messages too quickly, please wait for 1 minute to continue sending!`,   

    MESSAGE_WARRING_EMAIL_SEND_VI: `Cảnh báo: Đại hội chưa cài đặt Email riêng để nhận tin nhắn.`,

    MESSAGE_WARRING_EMAIL_SEND_EN: `Warning: Email receiving messages has not set up.`,

    MESSAGE_WARRING_MESSAGE_SEND_ERROR_VI: `Cảnh báo: Lỗi không gửi được tin nhắn, hãy thử lại sau 1 đến 2 phút.`,

    MESSAGE_WARRING_MESSAGE_SEND_ERROR_EN: `Warning: This message failed to deliver. Please try again after 1 to 2 minutes!`,

    NEW_DOCUMENTS_VI : `Tài liệu mới:`,

    NEW_DOCUMENTS_EN : `New Documents:`,

}

class ChatBox {

    constructor() {
        this.loadData();
        this.registerEvents();
    }
    
    // function to display data to chat box
    loadData() {

        ChatBoxConfig.USER_ONLINE = 1;

        $.ajax({
            url: '/ChatBox/CheckMessage',
            method: 'GET',
            dataType: 'json',
            data: { userStatus: ChatBoxConfig.USER_ONLINE, quantitiesDoc: 0 }
        }).done(function (response) {

            ChatBoxConfig.LANGUAGE_CODE = $('#requestValue').val();
            console.log(ChatBoxConfig.LANGUAGE_CODE);

            if (response > 0) {

                let today = new Date();
                let dateTimeSend = today.getDate() + '-' + (today.getMonth() + 1) + '-' + today.getFullYear() + ' ' + today.getHours() + ":" + today.getMinutes();
                let html = '';

                if (ChatBoxConfig.LANGUAGE_CODE === 'en-US') {
                    html = `<div class="row msg_container base_receive new_document">
                                    <div class="col-md-2 col-xs-2 avatar">
                                        <img class="img_chatbox" src="./img/avatar-chatbot.jpg" class=" img-responsive ">
                                    </div>
                                    <div class="col-md-10 col-xs-10">
                                        <div class="messages msg_receive">
                                            <p>
                                                ${ChatBoxConfig.MESSAGE_NEW_DOCUMENTS_VN}
                                            </p>
                                            <time>System ${' • ' + dateTimeSend}</time>
                                        </div>
                                    </div>
                                </div>`;

                    $('#message_board_box').append(html);
                } else {
                    html = `<div class="row msg_container base_receive new_document">
                                    <div class="col-md-2 col-xs-2 avatar">
                                        <img class="img_chatbox" src="./img/avatar-chatbot.jpg" class=" img-responsive ">
                                    </div>
                                    <div class="col-md-10 col-xs-10">
                                        <div class="messages msg_receive">
                                            <p>
                                                ${ChatBoxConfig.MESSAGE_NEW_DOCUMENTS_EN}
                                            </p>
                                            <time>System ${' • ' + dateTimeSend}</time>
                                        </div>
                                    </div>
                                </div>`;

                    $('#message_board_box').append(html);
                }

                

                ChatBoxConfig.QUANTITIES_DOC = response;
            }

            const messages = document.getElementById('message_board_box');
            messages.scrollTop = messages.scrollHeight;

        }).fail(function () {
            console.log("Error while request message.");
        });
        
    }

    // function to handle events for chat box
    registerEvents() {

        //code animation for chat box
        $(document).on('click', '.panel-heading span.icon_minim', function (e) {
            var $this = $(this);
            if (!$this.hasClass('panel-collapsed')) {
                $this.parents('.panel').find('.panel-body').slideUp();
                $this.addClass('panel-collapsed');
                $this.removeClass('glyphicon-minus').addClass('glyphicon-plus');
            } else {
                $this.parents('.panel').find('.panel-body').slideDown();
                $this.removeClass('panel-collapsed');
                $this.removeClass('glyphicon-plus').addClass('glyphicon-minus');
            }
        });

        $(document).on('focus', '.panel-footer input.chat_input', function (e) {
            var $this = $(this);
            if ($('#minim_chat_window').hasClass('panel-collapsed')) {
                $this.parents('.panel').find('.panel-body').slideDown();
                $('#minim_chat_window').removeClass('panel-collapsed');
                $('#minim_chat_window').removeClass('glyphicon-plus').addClass('glyphicon-minus');
            }
        });

        $(document).on('click', '.icon_close', function (e) {
            $("#chat_window_1").hide();
            $(".icon-message").show();
        });

        $('body').on('click', '#btn_icon_message', () => {
            $("#chat_window_1").show();
            $(".icon-message").hide();
        });

        var clickDisabled = false;
        // event cliked to button send message
        $('body').on('click', '#btn-chat', (e) => {
            e.preventDefault();
            
            if (!clickDisabled) {
                let stringMessage = $('#txtMessage').val();

                if (stringMessage) {

                    if (this.validSendMessageTime()) {

                        this.sendMessage(stringMessage, ChatBoxConfig.USER, null);
                        $('#txtMessage').val('')
                        this.sendMessage(stringMessage, ChatBoxConfig.SYSTEM, null);

                    } else {

                        this.sendMessage(stringMessage, ChatBoxConfig.SYSTEM_WARNING, null);

                    }
                    this.scrollToBottom();
                }
                clickDisabled = true;
                setTimeout(function () { clickDisabled = false; }, 60000);
                
            }
            else {
                ChatBoxConfig.LANGUAGE_CODE = $('#requestValue').val();
                console.log(ChatBoxConfig.LANGUAGE_CODE);
                if (ChatBoxConfig.LANGUAGE_CODE === 'en-US') {
                    $('#message_board_box').append(`<div class="row msg_container base_sent">
                            <div class="col-md-10 col-xs-10">
                                <div class="messages msg_sent" style="background: #ddd;">
                                    <p style="color: red; font-style: italic;">${ChatBoxConfig.MESSAGE_WARRING_MESSAGE_SEND_VI}</p >
                                </div>
                            </div>
                            <div class="col-md-2 col-xs-2 avatar">
                                <img class="img_chatbox" src="./img/avatar-chatbot.jpg" class=" img-responsive ">
                            </div>
                        </div>`);
                }
                else {
                    $('#message_board_box').append(`<div class="row msg_container base_sent">
                            <div class="col-md-10 col-xs-10">
                                <div class="messages msg_sent" style="background: #ddd;">
                                    <p style="color: red; font-style: italic;">${ChatBoxConfig.MESSAGE_WARRING_MESSAGE_SEND_EN}</p>
                                </div>
                            </div>
                            <div class="col-md-2 col-xs-2 avatar">
                                <img class="img_chatbox" src="./img/avatar-chatbot.jpg" class=" img-responsive ">
                            </div>
                        </div>`);
                }
            }
            //setTimeout(function () { clickDisabled = false; }, 10000);
            $('#txtMessage').focus();
        });

        // event press enter to send message
        $('body').on('keypress', '#txtMessage', (e) => {

            let stringMessage = $('#txtMessage').val();

            if (e.which == 13 && stringMessage) {
                if (!clickDisabled) {
                    if (this.validSendMessageTime()) {

                        this.sendMessage(stringMessage, ChatBoxConfig.USER, null);
                        $('#txtMessage').val('')
                        this.sendMessage(stringMessage, ChatBoxConfig.SYSTEM, null);

                    } else {

                        this.sendMessage(stringMessage, ChatBoxConfig.SYSTEM_WARNING, null);

                    }
                    this.scrollToBottom();
                    clickDisabled = true;
                    setTimeout(function () { clickDisabled = false; }, 60000);
                    return false;
                }
                else {
                    ChatBoxConfig.LANGUAGE_CODE = $('#requestValue').val();
                    console.log(ChatBoxConfig.LANGUAGE_CODE);
                    if (ChatBoxConfig.LANGUAGE_CODE === 'en-US') {
                        $('#message_board_box').append(`<div class="row msg_container base_sent">
                            <div class="col-md-10 col-xs-10">
                                <div class="messages msg_sent" style="background: #ddd;">
                                    <p style="color: red; font-style: italic;">${ChatBoxConfig.MESSAGE_WARRING_MESSAGE_SEND_VI}</p>
                                </div>
                            </div>
                            <div class="col-md-2 col-xs-2 avatar">
                                <img class="img_chatbox" src="./img/avatar-chatbot.jpg" class=" img-responsive ">
                            </div>
                        </div>`);
                    }
                    else {
                        $('#message_board_box').append(`<div class="row msg_container base_sent">
                            <div class="col-md-10 col-xs-10">
                                <div class="messages msg_sent" style="background: #ddd;">
                                    <p style="color: red; font-style: italic;">${ChatBoxConfig.MESSAGE_WARRING_MESSAGE_SEND_EN}</p>
                                </div>
                            </div>
                            <div class="col-md-2 col-xs-2 avatar">
                                <img class="img_chatbox" src="./img/avatar-chatbot.jpg" class=" img-responsive ">
                            </div>
                        </div>`);
                    }
                    
                }
            }
        });
        this.checkDocumentStatus();
        //setInterval(() => {
        //   
        //}, 5000);

        $('body').on('click', '#img_avatar', function () {
            ChatBoxConfig.LANGUAGE_CODE = $('#requestValue').val();
            console.log('ChatBoxConfig.LANGUAGE_CODE = ' + ChatBoxConfig.LANGUAGE_CODE);
        });
    }

    // function to handle string message
    sendMessage(message, actor, url) {
        //datetime sent
        let today = new Date();
        let dateTimeSend = today.getDate() + '-' + (today.getMonth() + 1) + '-' + today.getFullYear() + ' ' + today.getHours() + ":" + today.getMinutes();

        let userName = $('#header-username').html();
        let html = '';

        let chatBox = {
            Message: message,
        }

        switch (actor) {
            case ChatBoxConfig.USER: {
                html = `<div class="row msg_container base_sent">
                            <div class="col-md-10 col-xs-10">
                                <div class="messages msg_sent">
                                    <p>${message}</p>
                                    <time class="time-box" dateTimeSend="${dateTimeSend}">${userName + ' • ' + dateTimeSend}</time>
                                </div>
                            </div>
                            <div class="col-md-2 col-xs-2 avatar">
                                <img class="img_chatbox" src="./img/avatar-chatbot.jpg" class=" img-responsive ">
                            </div>
                        </div>`;
                this.callAjax('/ChatBox/SendMessage', 'POST', 'json', chatBox);
                break;
            }
            case ChatBoxConfig.SYSTEM: {
                if (ChatBoxConfig.LANGUAGE_CODE === 'en-US') {
                    html = `<div class="row msg_container base_receive">
                            <div class="col-md-2 col-xs-2 avatar">
                                <img class="img_chatbox" src="./img/avatar-chatbot.jpg" class=" img-responsive ">
                            </div>
                            <div class="col-md-10 col-xs-10">
                                <div class="messages msg_receive">
                                    <p>${ChatBoxConfig.MESSAGE_THANKS_VI}
                                    </p>
                                   
                                    <time>System ${' • ' + dateTimeSend}</time>
                                </div>
                            </div>
                        </div>`;
                }
                else {
                    html = `<div class="row msg_container base_receive">
                            <div class="col-md-2 col-xs-2 avatar">
                                <img class="img_chatbox" src="./img/avatar-chatbot.jpg" class=" img-responsive ">
                            </div>
                            <div class="col-md-10 col-xs-10">
                                <div class="messages msg_receive">
                                    <p>${ChatBoxConfig.MESSAGE_THANKS_EN}
                                    </p>
                                    
                                    <time>System ${' • ' + dateTimeSend}</time>
                                </div>
                            </div>
                        </div>`;
                }
                
                break;
            }
            case ChatBoxConfig.SYSTEM_WARNING: {

                if (ChatBoxConfig.LANGUAGE_CODE === 'en-US') {
                    html = `<div class="row msg_container base_re ceive">
                            <div class="col-md-2 col-xs-2 avatar">
                                <img class="img_chatbox" src="./img/avatar-chatbot.jpg" class=" img-responsive ">
                            </div>
                            <div class="col-md-10 col-xs-10">
                                <div class="messages msg_receive">
                                    <p style="color: red";> ${ChatBoxConfig.MESSAGE_WARRING_MESSAGE_SEND_VI}
           
                                    <time>System ${' • ' + dateTimeSend}</time>
                                </div>
                            </div>
                        </div>`;
                }
                else {
                    html = `<div class="row msg_container base_re ceive">
                            <div class="col-md-2 col-xs-2 avatar">
                                <img class="img_chatbox" src="./img/avatar-chatbot.jpg" class=" img-responsive ">
                            </div>
                            <div class="col-md-10 col-xs-10">
                                <div class="messages msg_receive">
                                    <p style="color: red";> ${ChatBoxConfig.MESSAGE_WARRING_MESSAGE_SEND_EN}
                                    
                                    <time>System ${' • ' + dateTimeSend}</time>
                                </div>
                            </div>
                        </div>`;
                }

               
                break;
            }
            case ChatBoxConfig.SYSTEM_WARNING_DO_NOT_HAVE_EMAIL: {

                if (ChatBoxConfig.LANGUAGE_CODE === 'en-US') {
                    html = `<div class="row msg_container base_receive">
                            <div class="col-md-2 col-xs-2 avatar">
                                <img class="img_chatbox" src="./img/avatar-chatbot.jpg" class=" img-responsive ">
                            </div>
                            <div class="col-md-10 col-xs-10">
                                <div class="messages msg_receive">

                                    <p style="color: red;">${ChatBoxConfig.MESSAGE_WARRING_EMAIL_SEND_VI}</p>                
                                                                    
                                    <time>System ${' • ' + dateTimeSend}</time>
                                </div>
                            </div>
                        </div>`;
                }
                else {
                    html = `<div class="row msg_container base_receive">
                            <div class="col-md-2 col-xs-2 avatar">
                                <img class="img_chatbox" src="./img/avatar-chatbot.jpg" class=" img-responsive ">
                            </div>
                            <div class="col-md-10 col-xs-10">
                                <div class="messages msg_receive">
                                     <p style="color: red;">${ChatBoxConfig.MESSAGE_WARRING_EMAIL_SEND_EN}</p>        
                            
                                    <time>System ${' • ' + dateTimeSend}</time>
                                </div>
                            </div>
                        </div>`;
                }


                
                break;
            }
            case ChatBoxConfig.SYSTEM_SEND_MESSAGE: {

                if (ChatBoxConfig.LANGUAGE_CODE === 'en-US') {
                    html = `<div class="row msg_container base_receive">
                            <div class="col-md-2 col-xs-2 avatar">
                                <img class="img_chatbox" src="./img/avatar-chatbot.jpg" class=" img-responsive ">
                            </div>
                            <div class="col-md-10 col-xs-10">
                                <div class="messages msg_receive">
                                    <p><b>${ChatBoxConfig.NEW_DOCUMENTS_VI}</b><a href="${url}" download>${stringMessage}</a></p>
                                    <time>System ${' • ' + dateTimeSend}</time>
                                </div>
                            </div>
                        </div>`;
                }

                else {


                    html = `<div class="row msg_container base_receive">
                            <div class="col-md-2 col-xs-2 avatar">
                                <img class="img_chatbox" src="./img/avatar-chatbot.jpg" class=" img-responsive ">
                            </div>
                            <div class="col-md-10 col-xs-10">
                                <div class="messages msg_receive">
                                    <p><b>${ChatBoxConfig.NEW_DOCUMENTS_EN}</b><a href="${url}" download>${stringMessage}</a></p>
                                    <time>System ${' • ' + dateTimeSend}</time>
                                </div>
                            </div>
                        </div>`;
                }
                break;
            }
            default: {
                if (ChatBoxConfig.LANGUAGE_CODE === 'en-US') {
                    html = `<div class="row msg_container base_receive">
                            <div class="col-md-2 col-xs-2 avatar">
                                <img class="img_chatbox" src="./img/avatar-chatbot.jpg" class=" img-responsive ">
                            </div>
                            <div class="col-md-10 col-xs-10">
                                <div class="messages msg_receive">
                                    <p style="color: red;">${ChatBoxConfig.MESSAGE_WARRING_MESSAGE_SEND_ERROR_VI}</p>
                                    <time>System ${' • ' + dateTimeSend}</time>
                                </div>
                            </div>
                        </div>`;
                }
                else {


                    html = `<div class="row msg_container base_receive">
                            <div class="col-md-2 col-xs-2 avatar">
                                <img class="img_chatbox" src="./img/avatar-chatbot.jpg" class=" img-responsive ">
                            </div>
                            <div class="col-md-10 col-xs-10">
                                <div class="messages msg_receive">
                                    <p style="color: red;">${ChatBoxConfig.MESSAGE_WARRING_MESSAGE_SEND_ERROR_EN}</p>
                                    <time>System ${' • ' + dateTimeSend}</time>
                                </div>
                            </div>
                        </div>`;
                }
                break;
            }
        }

        $('#message_board_box').append(html);
    }

    // function to handle scroll down bottom message box
    scrollToBottom() {
        const messages = document.getElementById('message_board_box');
        messages.scrollTop = messages.scrollHeight;
    }

    // this function check limit number of message (limited 10 message in 1 minute)
    validSendMessageTime() {
        let today = new Date();
        let dateTimeCurrent = today.getDate() + '-' + (today.getMonth() + 1) + '-' + today.getFullYear() + ' ' + today.getHours() + ":" + today.getMinutes();

        let dateTimeMessage = $('.time-box');
        let counter = 0;

        dateTimeMessage.prop('dateTimeSend', (i, v) => {
            if (dateTimeCurrent == v) {
                counter++;
            }
        });

        if (counter === 10) {
            return false;
        }

        return true;
    }

    // function call Ajax
    callAjax(_url, _method, _dataType, _data) {
        $.ajax({
            url: _url,
            method: _method,
            dataType: _dataType,
            data: { chatBox: _data }
        }).done(function (response) {
            console.log(response);
            response.code != 0 ? console.log(response.Message) : "";

        }).fail(function () {

            this.sendMessage('', ChatBoxConfig.DEFAULT, null);

        });
    }

    // function check document status
    checkDocumentStatus() {

        $.ajax({
            url: '/ChatBox/CheckMessage',
            method: 'GET',
            dataType: 'json',
            data: { userStatus: ChatBoxConfig.USER_IS_CHECK, quantitiesDoc: ChatBoxConfig.QUANTITIES_DOC }
        }).done(function (response) {
            console.log(response);

            if (response === ChatBoxConfig.NO_FILE_APPROVED) {
                $('.new_document').remove();

                const messages = document.getElementById('message_board_box');
                messages.scrollTop = messages.scrollHeight;
            }

            if (response > ChatBoxConfig.QUANTITIES_DOC) {

                let counter = $('.new_document').length;

                if (counter > 0) {
                    $('.new_document').remove();
                }

                let today = new Date();
                let dateTimeSend = today.getDate() + '-' + (today.getMonth() + 1) + '-' + today.getFullYear() + ' ' + today.getHours() + ":" + today.getMinutes();
                let html = '';

                if (ChatBoxConfig.LANGUAGE_CODE === 'en-US') {
                    html = `<div class="row msg_container base_receive new_document">
                                    <div class="col-md-2 col-xs-2 avatar">
                                        <img class="img_chatbox" src="./img/avatar-chatbot.jpg" class=" img-responsive ">
                                    </div>
                                    <div class="col-md-10 col-xs-10">
                                        <div class="messages msg_receive">
                                            <p>
                                                ${ChatBoxConfig.MESSAGE_NEW_DOCUMENTS_VN}
                                            </p>
                                            <time>System ${' • ' + dateTimeSend}</time>
                                        </div>
                                    </div>
                                </div>`;

                    $('#message_board_box').append(html);
                }
                else {
                    html = `<div class="row msg_container base_receive new_document">
                                    <div class="col-md-2 col-xs-2 avatar">
                                        <img class="img_chatbox" src="./img/avatar-chatbot.jpg" class=" img-responsive ">
                                    </div>
                                    <div class="col-md-10 col-xs-10">
                                        <div class="messages msg_receive">
                                            <p>
                                                ${ChatBoxConfig.MESSAGE_NEW_DOCUMENTS_EN}
                                            </p>
                                            <time>System ${' • ' + dateTimeSend}</time>
                                        </div>
                                    </div>
                                </div>`;

                    $('#message_board_box').append(html);
                }

               

                ChatBoxConfig.QUANTITIES_DOC = response;

                const messages = document.getElementById('message_board_box');
                messages.scrollTop = messages.scrollHeight;
            }
        });

    }
}

var cb = new ChatBox();

