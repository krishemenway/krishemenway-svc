function fallingObject(type, imageSrc, scoreModifier) {
	var object = {
		type: type,
		imageSrc: imageSrc,
		imageLoaded: false,
		x: 160,
		y: 0,
		height: 0,
		width: 0,
		scale: 1,
		scoreModifier: scoreModifier
	};

	object.image = new Image();

	object.image.onload = function() {
		object.imageLoaded = true;
		object.width = object.image.width/3;
		object.height = object.image.height/3;
		object.y = -1 * object.height / 2;
		object.x = 160 + -1 * randomNumber(50);
	};

	object.image.src = "/supermarketninja/" + imageSrc;

	object.render = function(canvas) {
		if(object.imageLoaded) {
			canvas.drawImage(object.image, object.x, object.y, object.width*object.scale, object.height*object.scale);

			if (window.debug) {
				canvas.beginPath();
				canvas.strokeStyle = "#FF0000";
				canvas.rect(object.x, object.y, object.width*object.scale, object.height*object.scale);
				canvas.stroke();
			}
		}
	};

	object.update = function(fallingSpeed, timeSinceLastFrame) {
		object.y += fallingSpeed * timeSinceLastFrame;
	};

	object.detectHit = function(x, y) {
		return (x >= object.x && x <= object.x+(object.width*object.scale) && y >= object.y && y <= object.y+(object.height*object.scale));
	};

	return object;
}

function donut() {
	return fallingObject("donut", "donut.png", -50);
}

function coke() {
	return fallingObject("coke", "coke.png", -50);
}

function mm() {
	return fallingObject("mm", "mm.png", -50);
}

function skittles() {
	return fallingObject("skittles", "skittles.png", -50);
}

function apple() {
	return fallingObject("apple", "apple.png", 100);
}

function orange() {
	return fallingObject("orange", "orange.png", 100);
}

function lettuce() {
	return fallingObject("lettuce", "lettuce.png", 100);
}

function peanuts() {
	return fallingObject("peanuts", "peanuts.png", 100);
}