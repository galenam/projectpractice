$(function () {
	$.getJSON("/api/home/GetInitialScreen").done(function(data) {
		console.log(data);
		var vm = new VendingMachine(data);
		vm.Initialize();
	});
});


function VendingMachine(vm) {
	this.Beverages = vm.Beverages;
	this.InsertedSum = vm.InsertedSum;
	this.MachineCoins = vm.MachineCoins;
	this.UserCoins = vm.UserCoins;

	this.BeveragesDiv = $("#Beverages");
	this.UserPurseDiv = $("#UserPurse");
	this.MachinePurseDiv = $("#MachinePurse");
	this.InsertedCoinsSpan = $("#InsertedCoins span");

	var that = this;
	this.Initialize = function() {
		$.each(this.Beverages,
			function() {
				$("<div>" + this.Name + " = " + this.Cost + " руб, " + this.Count + " порций </div>" ).appendTo(that.BeveragesDiv);
			});
		$.each(this.MachineCoins,
			function() {
				that.DisplayMoney(this, that.MachinePurseDiv);
			});
		$.each(this.UserCoins,
			function() {
				that.DisplayMoney(this, that.UserPurseDiv);
			});

		that.UserPurseDiv.find("div span.nominalEntry").on("click", function () {
			var countSpan = $(this).parent().find(".count");
			var count = parseInt(countSpan.text());
			if (isNaN(count) || count === 0) return false;

			var nominal = parseInt($(this).find("span.nominal").text());
			that.InsertedCoinsSpan.text(parseInt(that.InsertedCoinsSpan.text()) + nominal);
			countSpan.text(count - 1);
			return true;
		});
		that.InsertedCoinsSpan.text(that.InsertedSum);
	}

	this.DisplayMoney=function(element, parentDiv) {
		$("<div><span class='nominalEntry'><span class='nominal'>" + element.Nominal + "</span> руб</span> = <span class='count'>" + element.Count + "</span> штук</div>").appendTo(parentDiv);
	};

}