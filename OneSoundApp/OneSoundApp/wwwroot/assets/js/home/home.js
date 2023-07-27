$(document).ready(function () {
 
  
 
  //The wowjs
  new WOW().init();

  //Sidebar
  let sidebar = document.querySelector(".sidebar");
  let closeButton = document.querySelector(".sidebar .close-part .close");
  let openButton = document.querySelector("#nav-phone .container .logo i");

  openButton.addEventListener("click", function () {
    sidebar.classList.add("active-bar");
    sidebar.classList.remove("hide");
  });

  closeButton.addEventListener("click", function () {
    sidebar.classList.add("hide");
    sidebar.classList.remove("active-bar");
  });

  //Carousel
  $(".slides").owlCarousel({
    loop: true,
    margin: 10,
    nav: false,
    responsiveClass: true,
    autoplay: true,
    autoplayTimeout: 3000,
    smartSpeed: 1000,
    animateIn: "fadeIn",
    animateOut: "fadeOut",
    responsive: {
      0: {
        items: 1,
        nav: false,
      },
      768: {
        items: 1,
        nav: false,
      },
      992: {
        items: 1,
        nav: false,
      },
      1200: {
        items: 1,
        nav: false,
      },
    },
  });
 
  
  //Sticky navbar
  window.onscroll = function () {
    myFunction();
  };

  var navbar = document.querySelector("#nav");
  var sticky = navbar.offsetTop;

  function myFunction() {
    if (window.pageYOffset >= sticky) {
      navbar.classList.add("sticky");
    } else {
      navbar.classList.remove("sticky");
    }
  }

  //  //Albums-Carousel
  $(".albums").owlCarousel({
    loop: true,
    margin: 10,
    nav: false,
    responsiveClass: true,
    autoplay: true,
    autoplayTimeout: 6000,
    smartSpeed: 3000,
    animateIn: "fadeIn",
    animateOut: "fadeOut",
    responsive: {
      0: {
        items: 2,
        nav: false,
      },
      576: {
        items: 2,
        nav: false,
      },
      768: {
        items: 2,
        nav: false,
      },
      992: {
        items: 4,
        nav: false,
      },
      1200: {
        items: 5,
      },
    },
  });

  //Scroll to top button
  let scrollBtn = document.querySelector("#scrollBtn .btn");
  scrollBtn.addEventListener("click", function (e) {
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
  });

  window.addEventListener("scroll", function (e) {
    if (this.window.scrollY >= 376.5) {
      scrollBtn.style.opacity = 1;
    } else {
      scrollBtn.style.opacity = 0;
    }
  });

      //Albums hover
      let all = document.querySelectorAll(".people");
      all.forEach(each => {
          each.addEventListener("mouseenter", function (e) {
              e.preventDefault();
  
              each.classList.add("backgroundC")
              each.children[3].classList.remove("d-none");
              each.children[3].classList.add("slider-animation");
              each.children[2].classList.add("hide-animation");
              each.children[1].classList.add("hide-animation");
          })
      });
  
      all.forEach(each => {
          each.addEventListener("mouseleave", function (e) {
              e.preventDefault();
  
              each.classList.remove("backgroundC")
              each.children[3].classList.add("d-none");
              each.children[3].classList.remove("slider-animation");
              each.children[2].classList.remove("hide-animation");
              each.children[1].classList.remove("hide-animation");
          })
      });


   
});
