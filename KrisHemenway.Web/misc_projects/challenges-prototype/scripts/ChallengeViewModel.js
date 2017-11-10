
define(['date'], function() {
	return function(challengeConfiguration) {

		return {
			ChallengeStartDate: Date.parseDateFromServer(challengeConfiguration.StartDate),
			ChallengeEndDate: Date.parseDateFromServer(challengeConfiguration.EndDate),
			TotalScore: parseInt(challengeConfiguration.TotalScore),
			DaysRecorded: parseInt(challengeConfiguration.DaysRecorded)
		};
	};
});