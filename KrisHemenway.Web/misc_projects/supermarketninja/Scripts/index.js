"use strict";
(function() {
	// Image preload
	var images = ["background.jpg", "donut.png", "coke.png", "mm.png", "skittles.png", "apple.png", "orange.png", "lettuce.png", "peanuts.png"];
	images.forEach(function(path) {
		var i = new Image();
		i.src = "/supermarketninja/"+path;
	});
})();

window.addEventListener("load", function() {
	var canvas = document.getElementById("game");
	var g = new gameLoop(canvas);
	g.initialize();
});
