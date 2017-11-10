
define(['knockout'], function(ko) {
	return function StepsIntakeViewModel(date, stepCount) {
		var self = this;

		self.date = date;

		self.stepsCount = ko.observable(stepCount);

		var stepsCountAsInt = function() {
			if(self.stepsCount() === undefined)
				return 0;

			return parseInt(self.stepsCount()) || 0;
		};

		self.hasData = function() {
			return stepsCountAsInt() !== 0;
		};

		self.inputIsFocused = ko.observable(true);

		self.gotFocus = function() {
			self.inputIsFocused(true);
		};

		self.score = function() {
			return self.hasData() ? stepsCountAsInt() : 0;
		};

		return self;
	};
});
