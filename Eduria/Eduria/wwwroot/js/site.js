// Flexslider
$(window).load(function () {
    $('.flexslider').flexslider({
        animation: "slide"
    });
});

function responsiveToggle() {
    var x = document.getElementById("myTopnav");
    if (x.className === "topnav") {
        x.className += " responsive";
    } else {
        x.className = "topnav";
    }
}