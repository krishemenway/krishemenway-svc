
Date.Formatting = {}
Date.Formatting.weekdayNames = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
Date.Formatting.monthNames = ["January","February","March","April","May","June","July","August","September","October","November","December"];

Date.prototype.monthShortNames = ["Jan","Feb","Mar","Apr","May","June","July","Aug","Sept","Oct","Nov","Dec"];

Date.prototype.weekdayName = function() {
	return Date.Formatting.weekdayNames[this.getDay()];
};

Date.prototype.weekdayShortName = function() {
	return Date.Formatting.weekdayNames[this.getDay()].slice(0,3);
};

Date.prototype.monthName = function() {
	return Date.Formatting.monthNames[this.getMonth()];
};

Date.prototype.monthShortName = function() {
	if(this.getMonth() === 8 || this.getMonth() === 5 || this.getMonth() === 6) {
		return Date.Formatting.monthNames[this.getMonth()].slice(0,4);
	}
	else {
		return Date.Formatting.monthNames[this.getMonth()].slice(0,3);
	}
};

Date.parseDateFromServer = function(dateString) {
	var dateParts = dateString.split('-');
	return new Date(parseInt(dateParts[0]), parseInt(dateParts[1]) - 1, parseInt(dateParts[2]));
};