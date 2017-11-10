
define(['knockout', 'IntakeDayViewModel'], function ChallengeIntakeViewModel(ko, IntakeDayViewModel) {
    return function(challengeViewModel, createIntakeForDayFunction) {
        var self = this;

        self.challengeViewModel = challengeViewModel;

        self.daysToShowAtOnce = 7;

        self.today = new Date();

        self.createNewIntakeForDayFunction = createIntakeForDayFunction;

        self.selectedDay = ko.observable();

        self.selectedIndex = ko.observable(0);

        self.intakeDayViewModels = ko.observableArray([]);

        self.totalScore = ko.computed(function() {

            var score = self.challengeViewModel.TotalScore;

            var scores = ko.utils.arrayForEach(self.intakeDayViewModels(), function(intakeDayViewModel) {
                score += intakeDayViewModel.score();
            });

            return score;
        });

        self.daysRecorded = ko.computed(function() {

            return ko.utils.arrayFilter(self.intakeDayViewModels(), function(intakeDayViewModel) {

                return intakeDayViewModel.hasData();
            }).length + self.challengeViewModel.DaysRecorded;
        });

        self.goBackDays = function() {

            if(self.selectedIndex() < self.daysToShowAtOnce) {

                self.createNewWindowOfDays()
                self.selectedIndex(0);
            } else {

                self.selectedIndex(self.selectedIndex() - self.daysToShowAtOnce);
            }

            self.selectDay(self.intakeDayViewModels()[self.selectedIndex() + self.daysToShowAtOnce - 1]);
        };

        self.goForwardDays = function() {

            if(self.selectedIndexIsAtEnd())
                return false;

            if(self.selectedIndex() + self.daysToShowAtOnce > self.intakeDayViewModels().length - self.daysToShowAtOnce) {
                self.selectedIndex(self.intakeDayViewModels().length - self.daysToShowAtOnce);
            } else {
                self.selectedIndex(self.selectedIndex() + self.daysToShowAtOnce);
            }

            self.selectDay(self.intakeDayViewModels()[self.selectedIndex() + self.daysToShowAtOnce - 1]);
        };

        self.selectDay = function(intakeDayViewModel) {
            self.selectedDay(intakeDayViewModel);
            intakeDayViewModel.gotFocus();
        };

        self.createNewWindowOfDays = function() {

            var tomorrow = new Date(new Date(self.today).setDate(self.today.getDate()+1));
            var firstDate = self.intakeDayViewModels().length > 0 ? self.intakeDayViewModels()[0].date : tomorrow;

            for (var i = 1; i < self.daysToShowAtOnce + 1; i++) {

                var date = new Date(firstDate.getFullYear(), firstDate.getMonth(), firstDate.getDate() - i);

                if(date < self.challengeViewModel.ChallengeStartDate) {
                    break;
                }

                var intakeViewModelForDate = self.createNewIntakeForDayFunction(date, intakeViewModelForDate);
                var newIntakeViewModel = new IntakeDayViewModel(date, intakeViewModelForDate);
                self.challengeViewModel.TotalScore -= intakeViewModelForDate.score();

                if(newIntakeViewModel.hasData()) {

                    self.challengeViewModel.DaysRecorded--;
                }

                self.intakeDayViewModels.unshift(newIntakeViewModel);
            }
        };

        self.viewableDays = ko.computed(function() {

            return self.intakeDayViewModels.slice(self.selectedIndex(), self.selectedIndex() + self.daysToShowAtOnce);
        });

        self.selectedIndexIsAtEnd = ko.computed(function() {
            
            return self.intakeDayViewModels().length 
                    <= self.selectedIndex() + self.daysToShowAtOnce;
        });

        self.selectedIndexIsAtBeginning = ko.computed(function() {

            if(self.intakeDayViewModels().length === 0) {
                return false;
            }

            return self.selectedIndex() < self.daysToShowAtOnce
                && self.intakeDayViewModels()[0].date <= self.challengeViewModel.ChallengeStartDate;
        });

        self.init = function() {

            self.createNewWindowOfDays();
            self.intakeDayViewModels(self.intakeDayViewModels());

            var todaysViewModelIndex = self.intakeDayViewModels().length - 1;
            self.selectDay(self.intakeDayViewModels()[todaysViewModelIndex]);
        };

        self.init();

        return self;
    };
});
