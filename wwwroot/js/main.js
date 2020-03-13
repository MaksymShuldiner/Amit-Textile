$(document).ready(function () {
    let flag = true;
    function toggle() {
        $("#cats_hdn").toggleClass('hdn');
    }
    $("#cat").click(function () {
        if (flag) {
            toggle();
            $('#cats_hdn').animate({
                top: '40px'
            }, 300);
            flag = false;
            $('#arrowDown').toggleClass('hdn');
            $('#arrowUp').toggleClass('hdn');
        }
        else {
            $('#cats_hdn').animate({
                top: '20px'
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
    $('#close').click(function () {
        $('#regS').fadeOut(400);
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
    $('.1').click(function() {
        $('.11').toggleClass('selected');
    });
    $('.2').click(function () {
        $('.22').toggleClass('selected');
    });
    $('.3').click(function () {
        $('.33').toggleClass('selected');
    });
    $('.4').click(function () {
        $('.44').toggleClass('selected');
    });
    $('.5').click(function () {
        $('.55').toggleClass('selected');
    });
    $('.6').click(function () {
        $('.66').toggleClass('selected');
    });
    $('.7').click(function () {
        $('.77').toggleClass('selected');
    });
    $('.8').click(function () {
        $('.88').toggleClass('selected');
    });
    $('.9').click(function () {
        $('.99').toggleClass('selected');
    });
    $('.filterName').click(function () {
        let hide = $(this).next();
        let arrow = $(this).find('svg');
        if ($(hide).hasClass('hidden')) {
           /*$(arrow).animate({
                '-webkit-transform':'rotate(0deg)'
            }, 100);*/
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