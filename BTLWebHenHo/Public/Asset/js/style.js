$(document).ready(function() {
    $("#bt_close").click(function() {
        $("#wrap_chat_max").css('display', 'none');
        $("#wrap_chat_min").css('display', 'block');
    });
    $("#wrap_chat_min").click(function() {
        $("#wrap_chat_max").css('display', 'block');
        $("#wrap_chat_min").css('display', 'none');
    });

})