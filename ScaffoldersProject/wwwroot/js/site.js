// Write your JavaScript code.

$("#scroll").click(function () {
    $('html, body').animate({
        scrollTop: $("#myCarousel").offset().top
    }, 1000);
});