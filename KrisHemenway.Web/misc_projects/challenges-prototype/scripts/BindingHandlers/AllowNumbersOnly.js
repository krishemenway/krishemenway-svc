define(['knockout','jquery-1.10.2.min'], function(ko){
	ko.bindingHandlers.AllowNumbersOnly = {
		init: function(element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
			$(element).on('keypress', function(event) {
				return event.which >= "0".charCodeAt(0) && event.which <= "9".charCodeAt(0);
			});
	    }
	}
});