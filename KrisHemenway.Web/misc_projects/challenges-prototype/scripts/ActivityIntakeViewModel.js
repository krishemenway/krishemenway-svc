
define(['knockout', 'CompletedActivityViewModel', 'ActivityViewModel', 'date'], function(ko, CompletedActivityViewModel, ActivityViewModel) {
    return function ActivityIntakeViewModel(date, activitiesJson) {
        var self = this;

        self.date = date;

        self.getActivitiesFromServer = function() {
            return [
                {name: "Walking", activityId: 1, showOnPicker: true},
                {name: "Running", activityId: 2, showOnPicker: true},
                {name: "House Work", activityId: 3, showOnPicker: true},
                {name: "Bicycling", activityId: 4, showOnPicker: true},
                {name: "Machine Wiggling", activityId: 5, showOnPicker: true},
                {name: "Weight Lifting", activityId: 6, showOnPicker: true},
                {name: "Yoga", activityId: 7, showOnPicker: true},
                {name: "Swimming", activityId: 8, showOnPicker: true},
                {name: "Weird Ones", activityId: 9, showOnPicker: true},
                {name: "Dancing", activityId: 10},
                {name: "Fighting Broncos", activityId: 11},
                {name: "Sailing the Atlantic", activityId: 12},
                {name: "Web Developing", activityId: 13},
                {name: "Eating Pizza", activityId: 14}
            ].sort(function (l, r) {

                return (l.name > r.name) ? 1 : -1; 
            });
        };

        self.activityViewModels = ko.utils.arrayMap(self.getActivitiesFromServer(), function(activity) {
            return new ActivityViewModel(activity);
        });

        self.activityPickerActivities = ko.computed(function() {
            return ko.utils.arrayFilter(self.activityViewModels, function(activityViewModel) {
                return activityViewModel.showOnPicker;
            });
        });

        self.completedActivityViewModels = ko.observableArray(ko.utils.arrayMap(activitiesJson, function(activityJson) {
            
            var activity = ko.utils.arrayFirst(self.activityViewModels, function(activityViewModel) {
                return activityViewModel.activityId === activityJson.activityId;
            });
            
            return new CompletedActivityViewModel(activity, activityJson.minutesCompleted);
        }));

        self.resetNewCompletedActivityViewModel = function() {
            self.newCompletedActivityViewModel(new CompletedActivityViewModel());
            self.forceTimeIntakeView(false);
        };

        self.saveNewCompletedActivityViewModel = function() {
            if(self.newCompletedActivityViewModel().isValid()) {
                self.completedActivityViewModels.push(self.newCompletedActivityViewModel());
                self.resetNewCompletedActivityViewModel();   
            }
        };

        self.removeCompletedActivity = function(completedActivityViewModelToRemove) {
            self.completedActivityViewModels.remove(completedActivityViewModelToRemove);
        };

        self.humanDate = function() {
            var diff = (((new Date()).getTime() - self.date.getTime()) / 1000),
                day_diff = Math.floor(diff / 86400);

            if (isNaN(day_diff) || day_diff < 0)
                return "";

            return day_diff == 0 && "today" ||
                   day_diff < 7 && self.date.weekdayName() ||
                   self.date.monthShortName() + " " + self.date.getDate();
        };

        self.gotFocus = function() {
            if(self.newCompletedActivityViewModel().totalMinutesCompleted() === 0) {
                self.forceTimeIntakeView(false);
                self.viewingLog(false);
                self.resetNewCompletedActivityViewModel();
            }
        };

        self.hasData = function() {
            return self.hasCompletedActivityViewModels();
        };

        self.hasCompletedActivityViewModels = ko.computed(function() {
            return self.completedActivityViewModels().length !== 0;
        });

        self.newCompletedActivityViewModel = ko.observable(undefined);

        self.forceTimeIntakeView = ko.observable(false);

        self.toggleForceTimeIntakeView = function() {
            self.forceTimeIntakeView(!self.forceTimeIntakeView());
        };

        self.viewingTimeIntake = ko.computed(function() {
            return (self.newCompletedActivityViewModel() !== undefined
                   && self.newCompletedActivityViewModel().hasSelectedActivity())
                   || self.forceTimeIntakeView();
        });

        self.viewingLog = ko.observable(false);

        self.toggleViewLog = function() {
            self.viewingLog(!self.viewingLog())
        };

        self.shouldShowLog = ko.computed(function() {
            return self.viewingLog() && self.hasCompletedActivityViewModels();
        });

        self.score = function() {
            var score = 0;

            ko.utils.arrayForEach(self.completedActivityViewModels(), function(completedActivityViewModel) {
                score += completedActivityViewModel.score();
            });

            return score;
        };

        self.resetNewCompletedActivityViewModel();

        return self;
    };
});