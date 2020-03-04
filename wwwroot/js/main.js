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