$(document).ready(function() {

    datedisplay();
    renderTime();
    $('a[href^="#"]').on('click', function(e) {
        e.preventDefault();
    });

    function datedisplay() {
        var d = new Date()
        var weekday = new Array("रविवार", "सोमवार", "मंगलवार", "बुधवार", "गुरूवार", "शुक्रवार", "शनिवार")
        var monthname = new Array("जनवरी", "फ़रवरी", "मार्च", "अप्रैल", "मई", "जून", "जुलाई", "अगस्त", "सितम्बर", "अक्टूबर", "नवम्बर", "दिसम्बर")
        $('#datedisplay').text(weekday[d.getDay()] + ", " + d.getDate() + " " + monthname[d.getMonth()] + ", " + d.getFullYear())
    }

    function renderTime() {
        var currentTime = new Date();
        var diem = "सुबह";
        var h = currentTime.getHours();
        var m = currentTime.getMinutes();
        var s = currentTime.getSeconds();
        setTimeout(renderTime, 1000);
        if (h == 0) {
            h = 12;
        } else if (h > 12) {
            h = h - 12;
            diem = "मध्याह्न";
        }
        if (h < 10) {
            h = "0" + h;
        }
        if (m < 10) {
            m = "0" + m;
        }
        if (s < 10) {
            s = "0" + s;
        }
        var myClock = document.getElementById('clockDisplay');
        myClock.textContent = h + ":" + m + ":" + s + " " + diem;
        myClock.innerText = h + ":" + m + ":" + s + " " + diem;
    }

});