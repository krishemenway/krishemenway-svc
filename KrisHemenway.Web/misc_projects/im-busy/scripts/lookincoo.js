(function(window, angular) {
	'use strict';

	window.im_busy = angular.module('im_busy', []);

	window.im_busy.directive('scrollIf', function () {
		return function (scope, element, attributes) {
			setTimeout(function () {
				if (scope.$eval(attributes.scrollIf)) {
					element[0].scrollIntoView({block: "end"});
				}
			});
		}
	});

	window.im_busy.controller('im_busy', ['$scope', '$location','$anchorScroll', function($scope, $location, $anchorScroll) {
		$scope.lines = [];
		$scope.type_character_time_in_ms = 45;
		$scope.line_number = 0;
		$scope.settings_opened = true;
		$scope.settingsText = "";

		let currently_rendering_text = '',
			initial_lines = ['Test-Line 1', 'Test-Line 2', 'Test-Line 3'],
			lines_left = initial_lines.slice(0), // clone
			type_character_interval = null,
			current_line = '',
			current_character = 0;

		function save_last_text(text) {
			window.localStorage.setItem('lastText', text);
		}

		function load_last_text() {
			let lastText = window.localStorage.getItem('lastText') || '';

			if (!lastText) {
				reqwest({url: '/im-busy/code/linux-crypto.txt', type: "text/plain"}).then(function(response) { set_text(response.responseText); });
			} else {
				window.setTimeout(function() { set_text(lastText); }, 50);
			}
		}

		function set_text(text) {
			$scope.$apply(function() {
				$scope.settingsText = text;
			});

			initial_lines = text.split(/\r\n/g);
			lines_left = initial_lines.slice(0);
		}

		function complete_line() {
			$scope.$apply(function () {
				$scope.lines.push({number: $scope.line_number++, content: current_line});
				current_line = $scope.current_text = '';
			});

			type_next_line();
		}

		function type_next_character() {
			$scope.$apply(function () {
				$scope.current_text += current_line[current_character++];
			});

			if(current_line.length < current_character) {
				complete_line();
			}
		}

		function type_next_line() {
			if(lines_left.length === 0) {
				lines_left = initial_lines.slice(0);
			}

			current_line = lines_left.shift();
			current_character = 0;
		}

		$scope.reloadSettings = function() {
			if(currently_rendering_text !== $scope.settingsText) {
				save_last_text($scope.settingsText);
				load_last_text();
			}
		};

		$scope.stop = function() {
			$scope.stopped = true;
			window.clearInterval(type_character_interval);
		};

		$scope.start = function() {
			$scope.stopped = false;
			current_line = $scope.current_text = '';
			current_character = 0;

			type_character_interval = window.setInterval(type_next_character, $scope.type_character_time_in_ms);
			type_next_line();
		};

		load_last_text();
	}]);

})(window, window.angular);
