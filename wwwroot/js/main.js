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
    $('.decline').click(function () {
        let input = $(this).parent().parent().find('input');
        for (let i = 0; i < input.length; i++) {
            if ($(input[i]).attr('type') == "text" && !$(input[i]).hasClass('userNameField')) {
                input[i].value = "";
            }
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
    $('.rateItem').rating({ displayOnly: true, size: 'xs', showCaption: false, language: "ru" });
    $('.cart').hover(
        function () {
            $('.inCart').animate({
                top: '28px',
                opacity: "1"
            }, 300);
            $('.inCart').toggleClass('hidden');
        }, function () {
            $('.inCart').toggleClass('hidden');
            $('.inCart').css('top','23px');
            $('.inCart').css('opacity','0');
        }
    );
    $('.continue').click(function () {
        parent.jQuery.fancybox.getInstance().close();
    });
    $('.helloAuth').hover(
        function () { 
            $('.logedDropdown').animate({
                top: '27px',
                opacity: "1"
            }, 300);
            $('.logedDropdown').toggleClass('hidden');
        },
        function () {
            $('.logedDropdown').toggleClass('hidden');
            $('.logedDropdown').css('top', '22px');
            $('.logedDropdown').css('opacity', '0');
        }
    )
    $('.removeCharact').click(function () {
        $(this).parent().remove();
    });
    $('.payment').change(function () {
        if ($(this).is(':checked')) {
            $('.paymMethod').remove();
            if ($(this).hasClass('byCard')) {
                $('.paymentMethod').append('<div class="paymMethod"><label>Номер карты</label><br/><input name="CardNum" class="cardNumber" type="text"/></div>');
                $('.payment').attr('value', 'false');
            }
            else {
                $('.payment').attr('value', 'true');
            }
        }
    });
    if ($('.byCard').is(':checked')) {
        $('.paymentMethod').append('<div class="paymMethod"><label>Номер карты</label><br/><input class="cardNumber" name="CardNum" type="text"/></div>');
        $('.payment').attr('value', 'false');
    }
    else if ($('.byCash').is(':checked')) {
        $('.payment').attr('value', 'true');
    }
    $('.shippingRadio').change(function () {
        if ($(this).is(':checked')) {
            $('.shipMethod').remove();
            if ($(this).hasClass('toDepartment')) {
                if ($('.city').hasClass('hidden')) {
                    $('.city').prev().removeClass('hidden');
                    $('.city').prev().prev().removeClass('hidden');
                    $('.city').next().removeClass('hidden');
                    $('.city').removeClass('hidden');
                    $('.city').attr('name', 'Address');
                }
                $('.shippingMethod').append('<div class="shipMethod"><input type="radio" name="DepartmentName" class="post ukr"/><label>&nbsp &nbsp Укр. почта</label><br/><input type="radio" name="DepartmentName" class="post new"/><label>&nbsp &nbsp Новая почта</label><br/><label>Номер отделения</label><br/><input type="text" class="depNumber" name="DepartmentNum"></div>');
                $('.shippingRadio').attr('value', 'toDepartment');
                $('.post').change(function () {
                    if ($(this).is(':checked')) {
                        if ($(this).hasClass('ukr')) {
                            $('.post').attr('value', 'Укрпочта');
                        }
                        else {
                            $('.post').attr('value', 'Новая почта');
                        }
                    }
                });
            }
            else if ($(this).hasClass('toAddress')) {
                $('.city').next().addClass('hidden');
                $('.city').prev().addClass('hidden');
                $('.city').prev().prev().addClass('hidden');
                $('.city').addClass('hidden');
                $('.city').removeAttr('name');
                $('.shippingMethod').append('<div class="shipMethod"><input type="radio" name="DepartmentName" class="post ukr"/><label>&nbsp &nbsp Укр. почта</label><br/><input type="radio" name="DepartmentName" class="post new"/><label>&nbsp &nbsp Новая почта</label><br/><label>Ваш адрес</label><br/><input type="text" class="address" name="Address"/></div>');
                $('.shippingRadio').attr('value', 'toAdress');
                $('.post').change(function () {
                    if ($(this).is(':checked')) {
                        if ($(this).hasClass('ukr')) {
                            $('.post').attr('value', 'Укрпочта');
                        }
                        else {
                            $('.post').attr('value', 'Новая почта');
                        }
                    }
                });
            }
            else {
                $('.city').next().addClass('hidden');
                $('.city').prev().addClass('hidden');
                $('.city').prev().prev().addClass('hidden');
                $('.city').addClass('hidden');
                $('.shippingRadio').attr('value', 'pickup');
            }
        }
    });
    if ($('.toDepartment').is(':checked')) {
        if ($('.city').hasClass('hidden')) {
            $('.city').next().removeClass('hidden');
            $('.city').prev().removeClass('hidden');
            $('.city').prev().prev().removeClass('hidden');
            $('.city').removeClass('hidden');
            $('.city').attr('name', 'Address');
        }
        $('.shippingMethod').append('<div class="shipMethod"><input type="radio" name="DepartmentName" class="post urk"/><label>&nbsp &nbsp Укр. почта</label><br/><input type="radio" name="DepartmentName" class="post new"/><label>&nbsp &nbsp Новая почта</label><br/><label>Номер отделения</label><br/><input type="text" class="depNumber" name="DepartmentNum"></div>');
        $('.shippingRadio').attr('value', 'toDepartment');
        $('.post').change(function () {
            if ($(this).is(':checked')) {
                if ($(this).hasClass('ukr')) {
                    $('.post').attr('value', 'Укрпочта');
                }
                else {
                    $('.post').attr('value', 'Новая почта');
                }
            }
        });
    }
    else if ($('.toAddress').is(':checked')) {
        $('.city').next().addClass('hidden');
        $('.city').prev().addClass('hidden');
        $('.city').prev().prev().addClass('hidden');
        $('.city').addClass('hidden');
        $('.city').removeAttr('name');
        $('.shippingMethod').append('<div class="shipMethod"><input type="radio" name="DepartmentName" class="post ukr"/><label>&nbsp &nbsp Укр. почта</label><br/><input type="radio" name="DepartmentName" class="post new"/><label>&nbsp &nbsp Новая почта</label><br/><label>Ваш адрес</label><br/><input type="text" class="address" name="Address"/></div>');
        $('.shippingRadio').attr('value', 'toAdress');
        $('.post').change(function () {
            if ($(this).is(':checked')) {
                if ($(this).hasClass('ukr')) {
                    $('.post').attr('value', 'Укрпочта');
                }
                else {
                    $('.post').attr('value', 'Новая почта');
                }
            }
        });
    }
    else if ($('.pickup').is(':checked')) {
        $('.city').next().addClass('hidden');
        $('.city').prev().addClass('hidden');
        $('.city').prev().prev().addClass('hidden');
        $('.city').addClass('hidden');
        $('.shippingRadio').attr('value', 'pickup');
    };
    $('.showOrder').click(function () {
        $(this).find('.fa-chevron-up').toggleClass('hidden');
        $(this).find('.fa-chevron-down').toggleClass('hidden');
        let parent = $(this).parent().parent().parent().parent().parent();
        let table = $(this).next();
        let height;
        function tgl() {
            table.toggleClass('hidden')
        }
        if ($(table).hasClass('hidden')) {
            table.toggleClass('hidden');
            height= table.innerHeight();
            table.animate({
                opacity: "1"
            }, 300);
            parent.css("margin-bottom", 10+height + "px");
        }
        else {
            setTimeout(tgl, 300);
            table.animate({
                opacity: "0"
            }, 200);
            parent.css("margin-bottom", "20px");
        }
    });
    $('#sort').select2({
        language: "ru",
    })
    $('.adminOption-wrap').click(function () {
        $(this).find('svg').toggleClass('hidden');
        $(this).parent().next().find('.adminOption').toggleClass('hidden');
    });
});
window.onscroll = function () { scrollFunction() };
$(document).ready(function () {
    $('.openSide').click(function () {
        $('.side-menu').animate({
            left: "0px",
        }, 300);
        $('.zatem').css('display', 'block');
        $('body').css('overflow-y', 'hidden');
        $('html').css('overflow-y', 'hidden');
    });
    $('.closeSide').click(function () {
        $('.side-menu').animate({
            left: "-300px",
        }, 300);
        $('body').css('overflow-y', 'unset');
        $('html').css('overflow-y', 'unset');
        $('.zatem').css('display', 'none');
    });
    $('.zatem').click(function () {
        $('.side-menu').animate({
            left: "-300px",
        }, 300);
        $('body').css('overflow-y', 'unset');
        $('html').css('overflow-y', 'unset');
        $('.zatem').css('display', 'none');
    });
});
function scrollFunction() {
    let mybutton = document.getElementById('scrollBtn');
    if (document.body.scrollTop > 20 || document.documentElement.scrollTop > 20) {
        mybutton.style.display = "block";
    } else {
        mybutton.style.display = "none";
    }
}