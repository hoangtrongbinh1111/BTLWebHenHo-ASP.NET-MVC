$(function() {
    $(window).scroll(function() {
        if ($(this).scrollTop() == 0) {
            $("#menu-header").addClass('change-background-header');
        } else {
            $("#menu-header").removeClass('change-background-header');
        }
    });
});