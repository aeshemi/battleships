require.config({
	baseUrl: "scripts",
	paths: {
		domReady: "//cdnjs.cloudflare.com/ajax/libs/require-domReady/2.0.1/domReady.min",
		jquery: "//cdnjs.cloudflare.com/ajax/libs/jquery/3.3.1/jquery.min",
		knockout: "//cdnjs.cloudflare.com/ajax/libs/knockout/3.4.2/knockout-min"
	},
	deps: ["knockout"]
});

require(["knockout", "battleships", "domReady!"],
	function(ko, battleshipsVm) {
		ko.applyBindings(new battleshipsVm());
	});
