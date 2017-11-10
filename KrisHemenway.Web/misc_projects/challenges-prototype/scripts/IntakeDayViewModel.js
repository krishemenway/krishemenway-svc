
define(['knockout', 'date'], function(ko) {
    return function IntakeDateViewModel(date, intakeViewModel) {
        var self = this;

        self.date = date;

        self.humanDate = function() {
            var diff = (((new Date()).getTime() - date.getTime()) / 1000),
                day_diff = Math.floor(diff / 86400);

            if (isNaN(day_diff) || day_diff < 0)
                return "";

            return day_diff === 0 && "Today" ||
                   day_diff < 7 && self.date.weekdayShortName() ||
                   self.date.monthShortName() + " " + self.date.getDate();
        };

        self.gotFocus = function() {
            self.intakeViewModel().gotFocus();
        };

        self.intakeViewModel = ko.observable(intakeViewModel);

        self.score = function() {
            if(self.intakeViewModel() === undefined)
                return 0;
            
            return self.intakeViewModel().score();
        };

        self.hasData = ko.computed(function() {
            return self.intakeViewModel() !== undefined
                   && self.intakeViewModel().hasData();
        });

        return self;
    };
});
;