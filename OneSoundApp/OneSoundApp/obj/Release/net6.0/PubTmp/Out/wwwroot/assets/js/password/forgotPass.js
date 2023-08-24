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
  
  
  
     
  });