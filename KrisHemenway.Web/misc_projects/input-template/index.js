/* globals ko */
(function(ko) {

	var textMessagePartType = "Text";
	var inputMessagePartType = "Input";

	function messageTemplatePart(type, content) {
		var self = {
			content: ko.observable(content),
			renderedContent: renderedContent,
			type: type
		};

		function renderedContent() {
			return self.content().replace("{day}", "today");
		}

		function isValid() {
			return self.type === textMessagePartType || (self.content() !== "" && self.content() !== null && self.content() !== undefined);
		}

		self.isValid = ko.computed(isValid);
		return self;
	}

	function messageTemplate(params) {
		var self = {
			messageTemplateFormat: params.Format,
			messageObservable: params.Message,
			messageParts: ko.observableArray()
		};

		function currentMessageMatchesTemplate() {
			return self.messageObservable().match(buildRegex()) !== null;
		}

		function buildRegex() {
			return ko.unwrap(self.messageTemplateFormat).replace(/{_}/g, "(.+)");
		}

		function buildMessage() {
			return ko.utils.arrayMap(self.messageParts(), function(part) { return part.content(); }).join("");
		}

		function initializeMessageParts() {
			self.messageParts([]);
			var textParts = ko.unwrap(self.messageTemplateFormat).split("{_}");

			for (var i = 0; i < textParts.length; i++) {
				self.messageParts.push(messageTemplatePart(textMessagePartType, textParts[i]));

				if(i + 1 < textParts.length) {
					var part = messageTemplatePart(inputMessagePartType, "");
					part.content.subscribe(tryUpdateObservable);

					self.messageParts.push(part);
				}
			}
		}

		function loadContentFromObservable() {
			var existingMessageParts = self.messageObservable().match(buildRegex());
			existingMessageParts.shift();
			var inputMessageParts = ko.utils.arrayFilter(self.messageParts(), function(part) { return part.type === inputMessagePartType; });

			if(existingMessageParts.length !== inputMessageParts.length) {
				throw "total matched parameters does not match format parts";
			}

			for (var i = 0; i < existingMessageParts.length; i++) {
				inputMessageParts[i].content(existingMessageParts[i]);
			}
		}

		function allInputsValid() {
			var firstInvalidInput = ko.utils.arrayFirst(self.messageParts(), function(messagePart) {
				return !messagePart.isValid();
			});

			return firstInvalidInput === null;
		}

		function tryUpdateObservable() {
			if(allInputsValid()) {
				self.messageObservable(buildMessage());
			} else {
				self.messageObservable("");
			}
		}

		function initializeTemplate() {
			initializeMessageParts();

			if(currentMessageMatchesTemplate()) {
				loadContentFromObservable();
			}
		}

		initializeTemplate();

		if(ko.isObservable(params.Format)) {
			params.Format.subscribe(initializeTemplate);
		}

		return self;
	}

	function model() {
		var self = {};

		self.selectedFormat = ko.observable("How many {_} did you {_} {day}?");
		self.resultMessage = ko.observable("");
		self.resultMessage.subscribe(function() {
			console.log(self.resultMessage());
		});

		return self;
	}

	ko.components.register('message-template', { viewModel: messageTemplate, template: { element: "MessageTemplate"} });

	var model = model();
	var container = document.getElementById("TemplateContainer");
	ko.applyBindings(model, container);
})(ko);