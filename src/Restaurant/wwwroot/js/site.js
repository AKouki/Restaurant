const btnHumburger = document.querySelector('.hamburger');
const header = document.querySelector("header");

btnHumburger.addEventListener('click', function () {
    if (header.classList.contains('open')) {
        header.classList.remove('open');
    } else {
        header.classList.add('open');
    }
});

$(".accept-policy, .reject-policy").on('click', function () {
    $(this).closest(".cookie-consent").fadeOut(200, "linear", function () {
        $(this).remove();
    });
})

const leftNav = '<svg xmlns="http://www.w3.org/2000/svg" width="30" height="30" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2"><path stroke-linecap="round" stroke - linejoin="round" d = "M15 19l-7-7 7-7" /></svg >';
const rightNav = '<svg xmlns="http://www.w3.org/2000/svg" width="30" height="30" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2"><path stroke-linecap="round" stroke - linejoin="round" d = "M9 5l7 7-7 7" /></svg >';

$('.hero-carousel').owlCarousel({
    items: 1,
    lazyLoad: true,
    loop: true,
    margin: 10,
    nav: true,
    navText: [leftNav, rightNav],
    smartSpeed: 500,
    autoplay: false,
    autoplayTimeout: 10000,
    responsiveClass: true,
    responsive: {
        0: {
            dots: false
        },
        992: {
            dots: true
        }
    }
});

$('.reviews-carousel').owlCarousel({
    loop: true,
    margin: 20,
    nav: true,
    navText: [leftNav, rightNav],
    responsive: {
        0: {
            items: 1,
            nav: true
        },
        768: {
            items: 2
        },
        992: {
            items: 3,
            loop: false
        }
    }
});