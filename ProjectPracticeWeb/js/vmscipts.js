var apiAddresses = "/api/home/";

$(function () {
	$.getJSON(apiAddresses+"GetInitialScreen").done(function(data) {
		console.log(data);
		var vm = new VendingMachine(data);
		vm.Render();
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
	this.Render = function() {
		this.UpdateBeverages(this.Beverages);
		this.UpdateMachineCoins(this.MachineCoins);
		this.UpdateUsercoins(this.UserCoins);
		this.InsertedCoinsSpan.text(that.InsertedSum);

		this.UserPurseDiv.find("div span.nominalEntry").on("click", function () {
			var countSpan = $(this).parent().find(".count");
			var count = parseInt(countSpan.text());
			if (isNaN(count) || count === 0) return false;
			var nominal = parseInt($(this).find("span.nominal").text());
			// add nominal into url
			$.getJSON(apiAddresses + "InsertCoins/").done(function (data) {
				console.log(data);
				if (!data.Success) {
					return false;
				}
				that.UpdateMachineCoins(data.MachineCoins);
				that.UpdateUsercoins(data.UserCoins);
				that.InsertedCoinsSpan.text(data.InsertedSum);
				return true;
			});

			//that.InsertedCoinsSpan.text(parseInt(that.InsertedCoinsSpan.text()) + nominal);
			//countSpan.text(count - 1);
			return true;
		});

	}



	this.UpdateBeverages = function(beverages) {
		this.Beverages = beverages;
		$.each(this.Beverages,
			function () {
				$("<div>" + this.Name + " = " + this.Cost + " руб, " + this.Count + " порций </div>").appendTo(that.BeveragesDiv);
			});
	};

	this.UpdateMachineCoins = function(machineCoins) {
		this.MachineCoins = machineCoins;
		$.each(this.MachineCoins,
			function(index) {
				that.DisplayMoney(this, index, that.MachinePurseDiv);
			});
	};

	this.UpdateUsercoins = function(userCoins) {
		this.UserCoins = userCoins;
		$.each(this.UserCoins,
		function (index) {
			that.DisplayMoney(this, index, that.UserPurseDiv);
		});
	};


	this.DisplayMoney=function(element, index, parentDiv) {
		$("<div><span class='nominalEntry'><span class='nominal'>" + index + "</span> руб</span> = <span class='count'>" + element + "</span> штук</div>").appendTo(parentDiv);
	};

}