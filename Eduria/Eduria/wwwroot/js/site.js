// Flexslider
function responsiveToggle() {
    var x = document.getElementById("myTopnav");
    if (x.className === "topnav") {
        x.className += " responsive";
    } else {
        x.className = "topnav";
    }
}

// Jquery

//$(document).ready(function () {

//    // Get the button that opens the modal
//    var btn = $('#myBtn');

//    // Get the <span> element that closes the modal
//    var modal = $("#myModal");

//    // Get the <span> element that closes the modal
//    var span = $(".close")[0];

//    //When the user clicks the button, open the modal 
//    btn.click(function () {
//        modal.css("display", "block");
//    });

//    // When the user clicks on <span> (x), close the modal
//    span.click(function () {
//        modal.css("display", "none");
//    });

//    // When the user clicks anywhere outside of the modal, close it
//    window.click(function (event) {
//        if (event.target === modal) {
//            modal.css("display", "none");
//        }
//    });
//});


var modal = document.getElementById("myModal");

// Get the button that opens the modal
var btn = document.getElementById("myBtn");

// Get the <span> element that closes the modal
var span = document.getElementsByClassName("close")[0];

// When the user clicks the button, open the modal 
btn.onclick = function () {
    modal.style.display = "block";
}

// When the user clicks on <span> (x), close the modal
span.onclick = function () {
    modal.style.display = "none";
}

// When the user clicks anywhere outside of the modal, close it
window.onclick = function (event) {
    if (event.target == modal) {
        modal.style.display = "none";
    }
}