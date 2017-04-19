var apiAddresses = "/api/home/";

$(function () {
	$.getJSON(apiAddresses+"get").done(function(data) {
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
		this.CreateBeverages(this.Beverages);
		this.CreateMachineCoins(this.MachineCoins);
		this.CreateUsercoins(this.UserCoins);
		this.InsertedCoinsSpan.text(that.InsertedSum);
// убрать голубую надпись для клика , есди монет носминала 0.
		this.UserPurseDiv.find("div span.nominalEntry").on("click", function () {
			var countSpan = $(this).parent().find(".count");
			var count = parseInt(countSpan.text());
			var nominalElement = $(this).find("span.nominal");
			if (isNaN(count) || count === 0)
			{
				$(nominalElement).removeClass("nominal");
				return false;
			}
			var nominal = parseInt($(nominalElement).text());			
			// add nominal into url
			$.ajax({
				url: apiAddresses + "/Put?nominal=" + nominal,
				dataType: 'json',
				method: 'PUT'
			}).done(function (data) {
				if (!data.Success) {
					return false;
				}
				that.UpdateMachineCoins(data.MachineCoins);
				that.UpdateUsercoins(data.UserCoins);
				that.InsertedCoinsSpan.text(data.InsertedSum);
				return true;
			});
			return true;
		});
	}

	this.Update = function (array, div)
	{
		var that = this;
		$.each(array,
			function (index)
			{
				div.find("div span.nominalEntry span.nominal").filter(function ()
				{
					return $(this).text() == index;
				}).parent().parent().find("span.count").html(this);
			});
	};

	this.UpdateMachineCoins = function (machineCoins) {
		this.MachineCoins = machineCoins;
		this.Update(this.MachineCoins, this.MachinePurseDiv);
	};

	this.UpdateUsercoins = function (userCoins) {
		this.UserCoins = userCoins;
		this.Update(this.UserCoins, this.UserPurseDiv);
	};

	this.CreateBeverages = function () {
		$.each(this.Beverages,
			function () {
				$("<div>" + this.Name + " = " + this.Cost + " руб, " + this.Count + " порций </div>").appendTo(that.BeveragesDiv);
			});
	};

	this.CreateMachineCoins = function () {
		var textForAppend = "";
		$.each(this.MachineCoins,
			function (index) {
				textForAppend+= that.DisplayMoney(this, index);
			});
			$(textForAppend).appendTo(that.MachinePurseDiv);
	};

	this.CreateUsercoins = function (userCoins) {
		this.UserCoins = userCoins;
		var textForAppend = "";
		$.each(this.UserCoins,
			function (index) {
				textForAppend+=that.DisplayMoney(this, index);
			});
		$(textForAppend).appendTo(that.UserPurseDiv);
	};

	this.DisplayMoney = function (element, index) {
		return "<div><span class='nominalEntry'><span class='nominal'>" + index + "</span> руб</span> = <span class='count'>" + element + "</span> штук</div>";
	};

}