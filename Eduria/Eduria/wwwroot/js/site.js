
// Flexslider

function responsiveToggle() {
    var x = document.getElementById("myTopnav");
    if (x.className === "topnav") {
        x.className += " responsive";
    } else {
        x.className = "topnav";
    }
}


/* Load slider and show/hide it */
window.addEventListener('load', function () {
    document.querySelector('.glider').addEventListener('glider-slide-visible', function (event) {
        var glider = Glider(this);
        console.log('Slide Visible %s', event.detail.slide)
    });
    document.querySelector('.glider').addEventListener('glider-slide-hidden', function (event) {
        console.log('Slide Hidden %s', event.detail.slide)
    });
    document.querySelector('.glider').addEventListener('glider-refresh', function (event) {
        console.log('Refresh')
    });
    document.querySelector('.glider').addEventListener('glider-loaded', function (event) {
        console.log('Loaded')
    });

   /* Default settings slider */
    window._ = new Glider(document.querySelector('.glider'), {
        slidesToShow: 1, //'auto',
        slidesToScroll: 2,
        itemWidth: 400,
        dragVelocity: 0.7,
        draggable: true,
        scrollLock: true,
        dots: '#dots',
        rewind: true,
        arrows: {
            prev: '.glider-prev',
            next: '.glider-next'
        },

        /* Responsive for different devices */
        responsive: [
            {
                breakpoint: 800,
                settings: {
                    slidesToScroll: 2,
                    itemWidth: 400,
                    slidesToShow: 'auto',
                    exactWidth: true
                }
            },
            {
                breakpoint: 700,
                settings: {
                    slidesToScroll: 2,
                    slidesToShow: 1,
                    dots: false,
                    arrows: false,
                }
            },
            {
                breakpoint: 600,
                settings: {
                    slidesToScroll: 1,
                    slidesToShow: 1
                }
            },
            {
                breakpoint: 500,
                settings: {
                    slidesToScroll: 1,
                    slidesToShow: 1,
                    dots: false,
                    arrows: false,
                    scrollLock: true
                }
            }
        ]
    });
});

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

