// Write your JavaScript code.

$("#scroll").click(function () {
    $('html, body').animate({
        scrollTop: $("#perks").offset().top
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
            $('body').css('padding-bottom', '0');
            $('.full-screen').css('padding', '0');
        }
    });

    //when we click select product button on sidenav 
    $(document).on("click", "a.dropdown-link", function () {
        $linktext = $(this).text();
        $('#dropdown-button-text').text($linktext);
        $("#coinLabelId").text($linktext);
        $("#AskOfferCoinId").text($linktext);
        $('#totalCoinSelectedId').text('0.00');
        document.getElementById('enterAmmountId').value = '';
        if (document.getElementById('selectedCoinId').innerHTML === "EUR") {
            $("#enterAmmountIdCoin").text($linktext);         
        } else {
            $("#selectedCoinId").text($linktext);
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

    //Sidenav
    /* Loop through all dropdown buttons to toggle between hiding and showing its dropdown content - This allows the user to have multiple dropdowns without any conflict */
    var dropdown = document.getElementsByClassName("dropdown-btn");
    var i;

    for (i = 0; i < dropdown.length; i++) {
        dropdown[i].addEventListener("click", function () {
            this.classList.toggle("active");
            var dropdownContent = this.nextElementSibling;
            if (dropdownContent.style.display === "block") {
                dropdownContent.style.display = "none";
            } else {
                dropdownContent.style.display = "block";
            }
        });
    }


    
});
