function runningGameRules(gameContext) {
	var self = this,
		lastFallingObject = new Date();

	function scoreNotification(points, x, y) {
		if (x < 20) x = 20;
		else if (x > (gameContext.width - 80)) x = (gameContext.width - 80);

		if (y < 50) y = 50;

		return {
			points: points,
			x: x,
			y: y,
			age: 0
		};
	}
	
	var createRandomItem = function() {
		var random = Math.round(Math.random() * (gameContext.availableObjects.length-1));
		var item = gameContext.availableObjects[random];

		return new item();
	};

	var addNewFallingObjects = function() {
		var currentTime = new Date();
		var diff = (new Date() - lastFallingObject) / 1000;

		if (diff > gameContext.secondsPerItem || gameContext.fallingObjects.length === 0)
		{
			gameContext.fallingObjects.push(createRandomItem());
			lastFallingObject = currentTime;
		}
	};
	
	var pointNotifications = [];

	self.update = function(timeSinceLastFrame) {
		addNewFallingObjects();

		gameContext.fallingObjects.forEach(function(item, index, array) {
			if (selectedItem !== item) {
				item.update(gameContext.fallingObjectSpeed, timeSinceLastFrame);
			}
			
			var distanceFromScanner = gameContext.scannerPosition - item.y;
			item.scale = 1 + (-0.5 * distanceFromScanner / 600);

			if (selectedItem === item) {
				if (item.x < 1 || item.x+(item.width*item.scale) >= gameContext.width - 1) {
					gameContext.score -= item.scoreModifier;
					pointNotifications.push(new scoreNotification(-1 * item.scoreModifier, item.x, item.y));
					gameContext.fallingObjects.remove(item);
					gameContext.trashSound.play();
					return true;
				}
			}

			if(item.y + item.height > gameContext.scannerPosition) {
				if (item.scoreModifier < 0) {
					gameContext.endGame();
					return false;
				}

				gameContext.score += item.scoreModifier;
				pointNotifications.push(new scoreNotification(item.scoreModifier, item.x, item.y));
				gameContext.fallingObjects.remove(item);
				gameContext.targetSound.play();
			}

			return true;
		});
		
		pointNotifications.forEach(function(item, index, array) {
			item.age += timeSinceLastFrame;
			if (item.age > 1.5) {
				pointNotifications.remove(item);
			}
		});

		gameContext.secondsPerItem -= 0.01 * timeSinceLastFrame;
		gameContext.fallingObjectSpeed += 1.3 * timeSinceLastFrame;
	};

	var renderPointNotifications = function(canvas) {
		canvas.save();
		canvas.font="40px Arial";
		canvas.textAlign = "left";
        for (var j = pointNotifications.length - 1; j >= 0; j--) {
			var notification = pointNotifications[j];
			if (notification.age < 1.5) {
				var style = "rgba(255, 255, 255, " + (1 - (notification.age / 1.5)) + ")";
				canvas.fillStyle = style;
				canvas.fillText((notification.points > 0 ? "+" : "") + notification.points, notification.x, notification.y);
			}
        }
		canvas.restore();
	};
	
	var renderFallingObjects = function(canvas) {
        for(var i = gameContext.fallingObjects.length - 1; i >= 0; i--) {
        	gameContext.fallingObjects[i].render(canvas);
        }
	};
	
	var renderScore = function(canvas) {
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
	self.render = function(canvas) {
		renderFallingObjects(canvas);
		renderPointNotifications(canvas);
		renderScore(canvas);
		
		if (window.debug) {
			canvas.font = "20px Arial";
			canvas.fillStyle = "#F00";
			canvas.fillText("Delay: " + gameContext.secondsPerItem.toFixed(4) + " -- Speed: " + gameContext.fallingObjectSpeed.toFixed(4), 0, gameContext.height - 50);
		}
	};

	var selectedItem = null;

	self.handleTouchStart = function(x, y) {
        for(var i = gameContext.fallingObjects.length - 1; i >= 0; i--) {
        	var item = gameContext.fallingObjects[i];

        	if(item.detectHit(x,y)) {
				selectedItem = item;
        		break;
        	}
        }
	};

	self.handleTouchMove = function(x, y) {
		if (selectedItem != null) {
			selectedItem.x = x - ((selectedItem.width*selectedItem.scale) / 2);
			selectedItem.y = y - ((selectedItem.height*selectedItem.scale) / 2);

			if (selectedItem.y < -1 * ((selectedItem.height*selectedItem.scale)/3)) {
				selectedItem = null;
			}
		}
	};
	
	self.handleTouchEnd = function() {
		selectedItem = null;
	};

	return self;
}