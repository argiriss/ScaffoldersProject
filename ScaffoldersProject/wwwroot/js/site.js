﻿// Write your JavaScript code.

$("#scroll").click(function () {
    $('html, body').animate({
        scrollTop: $("#myCarousel").offset().top
    }, 1000);
});

$(document).ready(function () {
    $('#list').click(function (event) {
        event.preventDefault();
        $('#products .item').addClass('list-group-item');
        $('.list-group-wrapper').css('margin-right', '10px');
    });
    $('#grid').click(function (event) {
        event.preventDefault();
        $('#products .item').removeClass('list-group-item');
        $('#products .item').addClass('grid-group-item');
        $('.list-group-wrapper').css('margin-right', '0');
    });

    $('a[data-confirm]').click(function (ev) {
        var href = $(this).attr('href');
        if (!$('#dataConfirmModal').length) {
            $('body').append('<div id="dataConfirmModal" class="modal confirm-modal" role="dialog" aria-labelledby="dataConfirmLabel" aria-hidden="true"><div class="modal-header"><button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button><h3 id="dataConfirmLabel">Please Confirm</h3></div><div class="modal-body"></div><div class="modal-footer"><button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button><a class="btn btn-primary" id="dataConfirmOK">OK</a></div></div>');
        }
        $('#dataConfirmModal').find('.modal-body').text($(this).attr('data-confirm'));
        $('#dataConfirmOK').attr('href', href);
        $('#dataConfirmModal').modal({ show: true });
        return false;
    });

    $('#login-button').click(function (event) {
        event.preventDefault();
        $("#modal").load("Account/Login #myModal", function () {
            $('#myModal').appendTo("body").modal('show');
        });
    });

    $('#register-button').click(function (event) {
        event.preventDefault();
        $("#Registermodal").load("Account/Register #myRegisterModal", function () {
            $('#myRegisterModal').appendTo("body").modal('show');
        });
    });

    $('.navbar-toggle').on("click", function () {
        $('#myNavbar').toggle("slide");
    });

    $(function () {
        if ($(".orderbook").length) {
            $('body').css('overflow-y', 'hidden');
            $('.full-screen').css('padding', '0');
        }
    });

    $(function () {
        if ($("#scroll").length) {
            $('body').css('overflow-x', 'hidden');
            document.onkeydown = function (e) {

                switch (e.keyCode) {
                    case 38:
                        $('#second-anchor').trigger('click');
                        break;
                    case 40:
                        $('#fourth-anchor').trigger('click');
                        break;
                }
            };
        }
    });
    
});
