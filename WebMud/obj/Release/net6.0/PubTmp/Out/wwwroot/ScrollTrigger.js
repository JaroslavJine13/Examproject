
    window.onscroll = function() {myFunction()};

function myFunction() {
    try {
        if (document.documentElement.scrollTop > 50) {
            document.getElementById("slider").className = "slideLeft";
        }

        if (document.documentElement.scrollTop > 770) {
            document.getElementById("slider1").className = "slideUp";
        }


        if (document.documentElement.scrollTop > 1300) {
            document.getElementById("slider2").className = "fade";
        }
        //if (document.documentElement.scrollTop > 1400) {
        //    document.getElementById("slider3").className = "slideUp";
        //}
        //if (document.documentElement.scrollTop > 1700) {
        //    document.getElementById("slider4").className = "slideLeft";
        //}
        //if (document.documentElement.scrollTop > 2000) {
        //    document.getElementById("slider5").className = "slideLeft";
        //}
    } catch
    {

    }
}
