
define(['knockout'], function(ko) {
    return function(activity) {
        var self = this;

        self.name = activity.name;
        self.activityId = activity.activityId;
        self.showOnPicker = activity.showOnPicker;

        self.activityClassName = function() {
            var classes = "";
            classes += " " + self.name.toLowerCase().replace(" ", "");

            return classes;
        };

        return self;
    };
});
