
function gameRules() {
	"use strict";

	var self = {},
		body = document.getElementsByTagName("body")[0],
		currentRules,
		backgroundSource = "background.jpg",
		background,
		backgroundLoaded;

	var gameContext = {
		score: 0,
		availableObjects: [donut, apple, orange, coke, lettuce, peanuts, mm, skittles],
		fallingObjects: [],
		fallingObjectSpeed: 0,
		secondsPerItem: 0,
		width: 320,
		height: 500,
		scannerPosition: 455,
		backgroundMusic: document.getElementById("backgroundMusic"),
		targetSound: document.getElementById("scanSound"),
		trashSound: document.getElementById("trashSound")
	};

	var gameStates = {
		started: new startedGameRules(gameContext),
		running: new runningGameRules(gameContext),
		finished: new finishedGameRules(gameContext)
	};

	gameContext.startGame = function() {
		removeClass(body, "gameStarting");
		removeClass(body, "gameEnded");
		currentRules = gameStates.running;
		reset();
		gameContext.targetSound.play();
		gameContext.backgroundMusic.play();
	};

	gameContext.endGame = function() {
		addClass(body, "gameEnded");
		currentRules = gameStates.finished;
	};

	function reset() {
		gameContext.score = 0;
		gameContext.secondsPerItem = 1.5;
		gameContext.fallingObjectSpeed = 150;
		gameContext.fallingObjects = [];
	}

	function loadBackground() {
		background = new Image();

        background.onload = function () {
            backgroundLoaded = true;
        };

        background.src = "/supermarketninja/"+backgroundSource;
	}

	function renderBackground(canvas) {
		if (backgroundLoaded) {
            canvas.drawImage(background, 0, 0, gameContext.width, gameContext.height);
        }
	}

	self.resize = function(height) {
		gameContext.height = height;
		gameContext.scannerPosition = height * 0.9;
	};

	self.update = function(timeSinceLastFrame) {
		currentRules.update(timeSinceLastFrame);
	};

	self.render = function(canvas) {
		renderBackground(canvas);
		currentRules.render(canvas);
	};

	self.handleTouchStart = function(x, y) {
		currentRules.handleTouchStart(x, y);
	};

	self.handleTouchMove = function(x,y) {
		currentRules.handleTouchMove(x, y);
	};

	self.handleTouchEnd = function() {
		currentRules.handleTouchEnd();
	};

	loadBackground();
	addClass(body, "gameStarting");
	currentRules = gameStates.started;
	reset();

	return self;
}