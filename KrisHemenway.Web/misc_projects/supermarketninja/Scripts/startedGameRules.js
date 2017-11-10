
function startedGameRules(gameContext) {
	var self = this,
		startButton = document.getElementById("startButton");

	self.update = function(timeSinceLastFrame) {
	};

	self.render = function(canvas) {
	};

	self.handleTouchStart = function(x, y) {
	};

	self.handleTouchMove = function(x, y) {
	};
	
	self.handleTouchEnd = function() {
	};

	startButton.addEventListener("click", function() {
		gameContext.startGame();
	});

	return self;
}