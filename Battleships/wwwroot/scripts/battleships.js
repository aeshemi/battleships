define(["jquery", "knockout"],
	function($, ko) {
		var battleshipsVm = function BattleshipsVm() {
			var self = this;

			var showAlert = function(isError, message) {
				self.alertStyle(isError ? "alert-danger" : "alert-success");
				self.alertMessage(message);
				self.hasAlert(true);
			};

			self.gameStarted = ko.observable(false);
			self.gameWon = ko.observable(false);
			self.coordinates = ko.observable("");
			self.history = ko.observableArray();
			self.alertStyle = ko.observable("");
			self.alertMessage = ko.observable("");
			self.hasAlert = ko.observable(false);

			self.startNewGame = function() {
				self.hasAlert(false);
				self.history.removeAll();

				$.post("api/battleships/start")
					.done(function() {
						self.gameStarted(true);
						self.gameWon(false);
					})
					.fail(function() {
							showAlert(true, "Failed starting new game. Please try again.");
						}
					);
			};

			self.fire = function() {
				self.hasAlert(false);

				$.post("api/battleships/fire/" + self.coordinates())
					.done(function(data) {
						self.history.push({
							cssClass: data.isSuccess ? "list-group-item-success" : "list-group-item-danger",
							message: data.message
						});

						if (data.allShipsSunken) {
							self.gameWon(true);
							showAlert(false, "All ships were sunk. You won!");
						}
					})
					.fail(function() {
						showAlert(true, "Failed firing shot. Please try again.");
					});
			};
		};

		return battleshipsVm;
	});
