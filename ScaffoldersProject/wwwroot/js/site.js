// Write your JavaScript code.

$("#scroll").click(function () {
    $('html, body').animate({
        scrollTop: $("#myCarousel").offset().top
    }, 1000);
});

$(document).ready(function () {
    console.log("x");
    $('#list').click(function (event) {
        console.log("a");
        event.preventDefault();
        $('#products .item').addClass('list-group-item');
    });
    $('#grid').click(function (event) {
        console.log("as");
        event.preventDefault();
        $('#products .item').removeClass('list-group-item');
        $('#products .item').addClass('grid-group-item');

    });
});