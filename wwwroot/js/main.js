$(document).ready(function () {
    let flag = true;
    function toggle() {
        $("#cats_hdn").toggleClass('hdn');
    }
    $("#cat").click(function () {
        if (flag) {
            toggle();
            $('#cats_hdn').animate({
                top: '40px',
                opacity:1
            }, 300);
            flag = false;
            $('#arrowDown').toggleClass('hdn');
            $('#arrowUp').toggleClass('hdn');
        }
        else {
            $('#cats_hdn').animate({
                top: '20px',
                opacity:0
            }, 300);
            flag = true;
            setTimeout(toggle, 300);
            $('#arrowUp').toggleClass('hdn');
            $('#arrowDown').toggleClass('hdn');
        }
    });
    $("#scrollBtn").click(function () {
        $('body,html').animate({
            scrollTop: 0
        }, 800);
    });
    $('.close').click(function () {
        $(this).parent().fadeOut(400);
    });
    let flg = false;
    $('#showPassLogin').click(function () {
        if (!flg) {
            document.getElementById('loginPass').type="text";
            flg = true;
        }
        else {
            document.getElementById('loginPass').type="password";
            flg = false;
        }
        $('.atv').toggleClass('hdn');

    });
    let fl = false;
    $('#showPassRegister').click(function () {
        if (!fl) {
            document.getElementById('registerPass').type = "text";
            fl = true;
        }
        else {
            document.getElementById('registerPass').type = "password";
            fl = false;
        }
        $('.atv1').toggleClass('hdn');

    });
    let fl1 = false;
    $('#showPassConfirm').click(function () {
        if (!fl1) {
            document.getElementById('confirmPass').type = "text";
            fl1 = true;
        }
        else {
            document.getElementById('confirmPass').type = "password";
            fl1 = false;
        }
        $('.atv2').toggleClass('hdn');

    });
    /*$('.fav').click(function () {
        $(this).find('.toFavourite').toggleClass('selected');
    });*/
    $('.filterName').click(function () {
        let hide = $(this).next();
        let arrow = $(this).find('svg');
        if ($(hide).hasClass('hidden')) {
            $(arrow).toggleClass('rotate90');
            $(arrow).toggleClass('rotate0');
        }
        else {
            $(arrow).toggleClass('rotate90');
            if ($(arrow).hasClass('rotate0')) {
                $(arrow).toggleClass('rotate0');
            }
        }
        $(hide).toggleClass('hidden');
    });
    $('.imgMiniimg').mouseover(function () {
        $('#imgFull').attr('src', $(this).attr('src'));
    });
    $('#rating').rating({ displayOnly: true, language: 'ru' });
    $('#ratingComment').rating({ step: 0.1, language: 'ru', showClear: false, size: 'xl' });
    $(window).on('resize', function () {
        let aa = $('.charact').find('.container-fluid').find('.row').find('.col-md-4');
        if ($(window).width() > 1280 && aa.length == 0) {
            $('.fix').toggleClass('fix').toggleClass('col-md-4');
            $('.charact').find('.container-fluid').find('.row').find('.col-md-12').toggleClass('col-md-12').toggleClass('col-md-8');
        }
        else if ($(window).width() <= 1280 && aa.length == 1) {
            $(aa[0]).toggleClass('col-md-4').toggleClass('fix');
            $('.charact').find('.container-fluid').find('.row').find('.col-md-8').toggleClass('col-md-8').toggleClass('col-md-12');
        }
    });
    $('.declineComment').click(function () {
        let input = $(this).parent().find('input');
        for (let i = 0; i < input.length; i++) {
            if ($(input[i]).attr('type') == "text" && !$(input[i]).hasClass('userNameField')) {
                input[i].value = "";
            }
        }
        let textarea = $(this).parent().find('textarea');
        $(textarea).val('');
        let rate = $(this).parent().find('#ratingComment');
        if (rate.length != 0) {
            $(rate[0]).rating('clear');
        }
        parent.jQuery.fancybox.getInstance().close();
    });
    $('.postedCommentRating').rating({ displayOnly: true, size: 's', language: "ru" });
    $('#commentS').click(function () {
        let starsCount = $(this).parent().find('.rating-container').find('.rating-stars').attr('title');
        let arr = starsCount.split('');
        for (let i = 0; i < arr.length; i++) {
            if (arr[i] == " ") {
                arr.splice(i, arr.length - i);
                break;
            }
        }
        starsCount = arr.join('');
        $('#ratingComment').attr('value', starsCount);
    });
});
window.onscroll = function () { scrollFunction() };

function scrollFunction() {
    let mybutton = document.getElementById('scrollBtn');
    if (document.body.scrollTop > 20 || document.documentElement.scrollTop > 20) {
        mybutton.style.display = "block";
    } else {
        mybutton.style.display = "none";
    }
}