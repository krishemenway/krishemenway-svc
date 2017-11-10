
require(['knockout', 'ChallengeIntakeViewModel', 'ActivityIntakeViewModel', 'ChallengeViewModel', 'BindingHandlers/CountingValue', 'BindingHandlers/AllowNumbersOnly'], function(ko, ChallengeIntakeViewModel, ActivityIntakeViewModel, ChallengeViewModel) {

	var challengeActivityData = JSON.parse(document.getElementById('ChallengeActivitesData').innerHTML);

	var buildActivityViewModelFunction = function(date) {

		var activitiesJsonForDay = challengeActivityData[date.getFullYear() + '-' + (date.getMonth() + 1) + '-' + date.getDate()];
		return new ActivityIntakeViewModel(date, activitiesJsonForDay);
	};

	var challengeConfiguration = JSON.parse(document.getElementById('ChallengeConfiguration').innerHTML);
	var challengeViewModel = new ChallengeViewModel(challengeConfiguration);

	var challengeIntakeViewModel = new ChallengeIntakeViewModel(challengeViewModel, buildActivityViewModelFunction);
	ko.applyBindings(challengeIntakeViewModel, document.getElementById('intake'));
});
