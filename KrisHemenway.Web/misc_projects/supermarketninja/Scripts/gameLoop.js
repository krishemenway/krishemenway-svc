
function gameLoop(canvasElement) {
	var self = {},
		canvasContext,
		canvasContainer = canvasElement.parentNode,
		now,
		then,
		game;

	function loop() {
		var now = new Date();

		update((now - then) / 1000);
		render();

		then = now;
		requestAnimationFrame(loop);
	}

	function update(diff) {
		game.update(diff);
	}

	function render() {
        game.render(canvasContext);
	}
	
	function centerGame() {
		var viewportWidth = window.innerWidth;
		var viewportHeight = window.innerHeight;
		
		if (viewportHeight > 550)
			viewportHeight = 550;
		else if (viewportHeight < 400)
			viewportHeight = 400;


		canvasContainer.style.position = "absolute";
		canvasContainer.style.top = 0;
		canvasContainer.style.left = (viewportWidth - 320) / 2 + "px";

		canvasContainer.height = viewportHeight;
		canvasElement.height = viewportHeight;
		game.resize(viewportHeight);
	}

	self.initialize = function() {
		canvasContext = canvasElement.getContext("2d");
		game = new gameRules();
		then = now = new Date();
		requestAnimationFrame(loop);

	    document.addEventListener("mousedown", function(e) {
			var box = canvasElement.getBoundingClientRect();
	    	game.handleTouchStart(e.pageX - box.left, e.pageY - box.top);
			e.preventDefault();
	    });

	    canvasElement.addEventListener("touchstart", function(e) {
			var box = canvasElement.getBoundingClientRect();
	    	game.handleTouchStart(e.touches[0].pageX - box.left, e.touches[0].pageY - box.top);
			e.preventDefault();
	    });

		document.addEventListener("mousemove", function(e) {
			var box = canvasElement.getBoundingClientRect();
			game.handleTouchMove(e.pageX - box.left, e.pageY - box.top);
			e.preventDefault();
		});

		canvasElement.addEventListener("touchmove", function(e) {
			var box = canvasElement.getBoundingClientRect();
	    	game.handleTouchMove(e.touches[0].pageX - box.left, e.touches[0].pageY - box.top);
			e.preventDefault();
		});

		document.addEventListener("mouseup", function(e) {
			game.handleTouchEnd();
			e.preventDefault();
		});

		canvasElement.addEventListener("touchend", function(e) {
	    	game.handleTouchEnd();
			e.preventDefault();
		});
		
		document.addEventListener("touchmove", function (e) {
			// prevent scrolling on mobile devices
			e.preventDefault();
		});
		
		// Center canvas
		window.addEventListener("resize", centerGame);
		centerGame();
		
		// hide address bar
		setTimeout(function() {
			window.scrollTo(0, 1);
		}, 10);
	};

	return self;
}