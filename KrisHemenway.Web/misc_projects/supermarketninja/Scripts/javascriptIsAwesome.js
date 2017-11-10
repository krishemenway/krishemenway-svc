// support outdated browsers :(
window.requestAnimationFrame = window.requestAnimationFrame || window.mozRequestAnimationFrame || window.webkitRequestAnimationFrame || window.msRequestAnimationFrame;;

Array.prototype.remove = function() {
    var what, a = arguments, L = a.length, ax;
    while (L && this.length) {
        what = a[--L];
        while ((ax = this.indexOf(what)) !== -1) {
            this.splice(ax, 1);
        }
    }
    return this;
};

function removeClass(elems, className) {
    var i,
    removeClass = function (elem) {
        elem.className = elem.className.replace(className + ' ', '').replace(' ' + className, '').replace(className, '');
    };

    if (elems.length) {
        for (i = elems.length - 1; i >= 0; --i) {
            removeClass(elems[i]);
        }
    } else {
        removeClass(elems);
    }
}

function addClass(elem, className) {
    if (!hasClass(elem, className)) {
        elem.className = elem.className ? (elem.className + ' ' + className) : className;
    }
}

function hasClass(elem, className) {
	if (elem && elem.className) {
		var elementClasses = elem.className.replace(/[\t\r\n]/g, " ").split(" ");
		for (var i=0; i < elementClasses.length; i++) {
			if (elementClasses[i] === className)
				return true
		}
	}
	return false;
}

function randomNumber(max) {
    return Math.round(Math.random() * max);
}