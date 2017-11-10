function finishedGameRules(gameContext) {
	var self = this,
		replayButton = document.getElementById("replayButton");

	self.update = function(timeSinceLastFrame) {
	};

	self.render = function(canvas) {
		var text = "Score: " + gameContext.score;
        canvas.font="32px Arial";
        canvas.fillStyle = "#FFF";
        canvas.textAlign = "left";
		canvas.shadowColor = "#000";
		canvas.shadowOffsetX = 0;
		canvas.shadowOffsetY = 0;
		canvas.shadowBlur = 5;
        canvas.fillText(text, 10, 30);
	};

	self.handleTouchStart = function(x, y) {
	};

	self.handleTouchMove = function(x, y) {
	};
	
	self.handleTouchEnd = function() {
	};

	replayButton.addEventListener("click", function() {
		gameContext.startGame();
	});

	return self;
}