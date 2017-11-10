define(['knockout','jquery-1.10.2.min'], function(ko){
	ko.bindingHandlers.CountingValue = {
		update: function(element, valueAccessor) {
			var value = ko.utils.unwrapObservable(valueAccessor())

			var from = { number: parseInt($(element).html()) };
			var to = { number: parseInt(value) };

			var animateOptions = {
				duration: 450,
				step: function() {
					$(element).text(Math.ceil(this.number));
				},
				complete: function() {
					$(element).text(value);
				}
			};

			$(from).animate(to, animateOptions);
		}
	}
});