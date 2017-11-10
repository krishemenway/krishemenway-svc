
require(['knockout', 'ChallengeIntakeViewModel', 'StepsIntakeViewModel', 'ChallengeViewModel', 'BindingHandlers/CountingValue', 'BindingHandlers/AllowNumbersOnly'], function(ko, ChallengeIntakeViewModel, StepsIntakeViewModel, ChallengeViewModel) {
	
	var stepsData = JSON.parse(document.getElementById('ChallengeStepsData').innerHTML);

	var buildStepsViewModelFunction = function(date) {
		
		var stepCountForDay = stepsData[date.getFullYear() + '-' + (date.getMonth() + 1) + '-' + date.getDate()];
		return new StepsIntakeViewModel(date, stepCountForDay);
	};

	var challengeConfiguration = JSON.parse(document.getElementById('ChallengeConfiguration').innerHTML);
	var challengeViewModel = new ChallengeViewModel(challengeConfiguration);

	var challengeIntakeViewModel = new ChallengeIntakeViewModel(challengeViewModel, buildStepsViewModelFunction);
	ko.applyBindings(challengeIntakeViewModel, document.getElementById('intake'));
});
