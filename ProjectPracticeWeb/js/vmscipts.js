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
	this.ErrorDiv = $(".vendingMachine .error");
	this.ReturnButton = $("#InsertedCoins button");

	var that = this;
	this.Render = function() {
		this.CreateBeverages(this.Beverages);
		this.CreateMachineCoins(this.MachineCoins);
		this.CreateUsercoins(this.UserCoins);
		this.UpdateInsertedSum(this.InsertedSum);
		this.ShowHideReturnButton(this.InsertedSum);
		
		this.BeveragesDiv.find("div.checkBeverage").on("click", function ()
		{
			var name = $(this).find("span.name").text();
			if (name == null || name == "") { return false; }

			var cost = $(this).find("span.cost").text();

			if (isNaN(cost) || that.InsertedSum < cost)
			{
				that.ShowHideErrorDiv("Не хватает на выбранный напиток. Пожалуйста, внесите недостающую сумму.");
				return false;
			}

			var count = parseInt($(this).find("span.count").text());

			if (isNaN(count) || count <= 0) { return false; }
			$(this).removeClass().addClass("clickBeverage");
			$.ajax({
				url: apiAddresses + "/Post?bev=" + name,
				dataType: 'json',
				method: 'POST'
			}).done(function (data)
			{
				if (!data.Success)
				{
					return false;
				}
				that.UpdateMachineCoins(data.MachineCoins);
				that.UpdateUsercoins(data.UserCoins);
				that.UpdateInsertedSum(data.InsertedSum);
				that.UpdateBevereges(data.Beverages);

				that.ShowHideReturnButton(data.InsertedSum);
				that.ShowHideErrorDiv();
				return true;
			}).always(function ()
			{
				$(this).removeClass().addClass("checkBeverage");
			});
			return false;
		});

		this.AddOnClickUserPurse();

		$("#InsertedCoins button").on("click", function ()
		{
			$.ajax({
				url: apiAddresses + "/delete",
				dataType: 'json',
				method: 'delete'
			}).done(function (data)
			{
				that.UpdateMachineCoins(data.MachineCoins);
				that.UpdateUsercoins(data.UserCoins);
				that.UpdateInsertedSum(data.InsertedSum);
				that.AddOnClickUserPurse();
				that.ShowHideReturnButton(data.InsertedSum);

			});
		});
	}

	this.ShowHideErrorDiv = function (text)
	{
		if (text == null || text == "")
		{
			this.ErrorDiv.addClass("hide");
		}
		else
		{
			this.ErrorDiv.text(text);
			this.ErrorDiv.removeClass("hide");
		}
	}

	this.ShowHideReturnButton = function(sum)
	{
		if (sum <= 0) { this.ReturnButton.addClass("hide"); }
		else { this.ReturnButton.removeClass(); }
	}

	this.AddOnClickUserPurse = function ()
	{
		var that = this;
		this.UserPurseDiv.find("div span.nominalEntry").off("click");
		// клик вынести в отдельную процедуру
		this.UserPurseDiv.find("div span.nominalEntry").on("click", function ()
		{
			var countSpan = $(this).parent().find(".count");
			var count = parseInt(countSpan.text());
			var nominalElement = $(this).find("span.nominal");
			if (isNaN(count) || count === 0)
			{
				return false;
			}
			var nominal = parseInt($(nominalElement).text());
			// add nominal into url
			$.ajax({
				url: apiAddresses + "/Put?nominal=" + nominal,
				dataType: 'json',
				method: 'PUT'
			}).done(function (data)
			{
				if (!data.Success)
				{
					return false;
				}
				that.UpdateMachineCoins(data.MachineCoins);
				that.UpdateUsercoins(data.UserCoins);
				that.UpdateInsertedSum(data.InsertedSum, that);
				that.ShowHideReturnButton(data.InsertedSum);

				return true;
			});
			return true;
		});
	}

	this.UpdateInsertedSum = function (sum)
	{
		this.InsertedCoinsSpan.text(sum);
		this.InsertedSum = sum;
		if (!isNaN(sum) && sum > 0)
		{
			this.ShowHideErrorDiv();
		}
	};

	this.UpdateBevereges = function (beverages)
	{
		this.Beverages = beverages;
		var that = this;
		$.each(this.Beverages, function (index)
		{
			var elemParent = that.BeveragesDiv.find("div span.name").filter(function ()
			{
				return $(this).text() == index;
			}).parent();

			elemParent.find("span.count").html(this.Count);
			elemParent.removeClass().addClass("checkBeverage");
		})
	};

	this.Update = function (array, div, needClick)
	{
		var that = this;
		$.each(array,
			function (index)
			{
				var elem = div.find("div span span.nominal").filter(function ()
				{					
					return $(this).text() == index;
				}).parent().parent().find("span.count");
				elem.html(this);
				if (needClick)
				{
					if (elem.html() <= 0)
					{
						var nominalentry = $(elem).parent().find("span.nominalEntry");
						nominalentry.removeClass("nominalEntry");
						nominalentry.off("click");
					}
					else
					{
						var nominalentry = $(elem).parent().find("span")[0];
						$(nominalentry).addClass("nominalEntry");
						//that.AddOnClickUserPurse();
					}
				}
			});
	};

	this.UpdateMachineCoins = function (machineCoins) {
		this.MachineCoins = machineCoins;
		this.Update(this.MachineCoins, this.MachinePurseDiv, false);
	};

	this.UpdateUsercoins = function (userCoins) {
		this.UserCoins = userCoins;
		this.Update(this.UserCoins, this.UserPurseDiv, true);
	};

	this.CreateBeverages = function ()
	{
		var classAttr = "class='checkBeverage'";
		var that = this;
		$.each(this.Beverages,
			function ()
			{				
				$("<div "+ (this.Count >0 ? classAttr : "") +"><span class='name'>" + this.Name + "</span> = <span class='cost'>" + this.Cost + "</span> руб, <span class='count'>" + this.Count + "</span> порций </div>").appendTo(that.BeveragesDiv);
			});		
	};

	this.CreateMachineCoins = function () {
		var textForAppend = "";
		$.each(this.MachineCoins,
			function (index) {
				textForAppend+= that.DisplayMoney(this, index, false);
			});
			$(textForAppend).appendTo(that.MachinePurseDiv);
	};

	this.CreateUsercoins = function (userCoins) {
		this.UserCoins = userCoins;
		var textForAppend = "";
		$.each(this.UserCoins,
			function (index) {
				textForAppend+=that.DisplayMoney(this, index,true);
			});
		$(textForAppend).appendTo(that.UserPurseDiv);
	};

	this.DisplayMoney = function (element, index, needAnalyzeCount)
	{
		var spanPart = "";
		if (needAnalyzeCount && element > 0) { spanPart = ' class="nominalEntry"'; }
		return "<div><span"+spanPart+"><span class='nominal'>" + index + "</span> руб</span> = <span class='count'>" + element + "</span> штук</div>";
	};

}