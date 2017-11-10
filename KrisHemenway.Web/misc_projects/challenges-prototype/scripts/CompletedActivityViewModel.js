
define(['knockout'], function(ko) {
    return function(activity, minutes) {
        var self = this;

        self.activity = ko.observable(activity);

        self.hoursCompleted = ko.observable(undefined);

        self.minutesCompleted = ko.observable(undefined);

        self.hasSelectedActivity = function() {

            return self.activity() !== undefined;
        };

        self.totalMinutesCompleted = ko.computed({

            read: function() {

                var minutes = parseInt(self.minutesCompleted()) || 0,
                    minutesFromHours = (parseInt(self.hoursCompleted()) || 0) * 60;

                return minutes + minutesFromHours;
            },
            write: function(value) {

                var minutesFromInput = parseInt(value);
                self.updateWithMinutes(minutesFromInput);
            },
            owner: self
        });

        self.addHoursIfMinutesGreaterThan60 = function() {

            var newTotalMinutes = self.totalMinutesCompleted();

            if(newTotalMinutes > 0)
                self.updateWithMinutes(newTotalMinutes);
        };

        self.minutesCompleted.subscribe(self.addHoursIfMinutesGreaterThan60);

        self.score = function() {

            return self.totalMinutesCompleted();
        };

        self.selectActivity = function(activity) {

            self.activity(activity);
        };

        self.updateWithMinutes = function(minutes) {

            if(!isNaN(minutes) && minutes >= 60) {

                var hours = Math.floor(minutes / 60),
                    minutes = minutes % 60;

                if(self.hoursCompleted() !== hours)
                    self.hoursCompleted(hours);

                if(self.minutesCompleted() !== minutes)
                    self.minutesCompleted(minutes); 
            }
        };

        self.isValid = ko.computed(function() {

            return self.activity() !== undefined 
                   && self.totalMinutesCompleted() > 0
                   && self.totalMinutesCompleted() < 1440;
        });

        self.updateWithMinutes(minutes);

        return self;
    };
});
