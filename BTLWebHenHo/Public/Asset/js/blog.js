$(document).ready(function() {
    $(window).scroll(function() {
        var leng = $(window).scrollTop();
        if (leng != 0) {
            $('.back-to-top').fadeIn();
        } else {
            $('.back-to-top').fadeOut();
        }
    });
    $("#back-to-top").click(function() {
        $('body,html').animate({
            scrollTop: 0
        }, 500);
        return false;
    })
});