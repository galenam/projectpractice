using ProjectPracticeWeb.Models;
using System.Collections.Generic;

namespace ProjectPracticeWeb.AppCode
{
	public interface IVendingMachine
	{
		int InsertedSum { get; set; }
		IState State { get; set; }
		Dictionary<string, Beverage> Beverages { get; set; }
		SortedDictionary<int, int> MachineCoins { get; set; }
		SortedDictionary<int, int> UserCoins { get; set; }

		bool InsertCoin(int nominal);
		bool ReturnCoins();
		bool TurnCrank(string bev);
		bool ReleaseBeverage(Beverage bev);

		bool IsEnoughMoneyToSellBeverage(Beverage bev);
		bool HasMoneyInserted();
	}
}