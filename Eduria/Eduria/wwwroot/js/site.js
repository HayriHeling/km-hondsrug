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


/* Toggle Wrong answers */
function toggleWrong() {
    var x = document.getElementById("wrong");
    if (x.style.display === "none") {
        x.style.display = "block";
    }
    else {
        x.style.display = "none";
    }
}

/* Toggle Correct answers */
function toggleCorrect() {
    var y = document.getElementById("correct");
    if (y.style.display === "none") {
        y.style.display = "block";
    } else {
        y.style.display = "none";
    }
}
