//R Disable
window.oncontextmenu = function () {
    return false;
}


$(document).keydown(function (event) {
    if (event.keyCode == 123) {
        window.location = "/404"
        return false;
    }
    else if ((event.ctrlKey && event.shiftKey && event.keyCode == 73) || (event.ctrlKey && event.shiftKey && event.keyCode == 74)) {
        window.location = "/404"
        return false;
    }
});

//c+u disable
document.onkeydown = function (e) {
    if (e.ctrlKey && (e.keyCode === 85)) {
        window.location = "/404"
        return false;
    }
};