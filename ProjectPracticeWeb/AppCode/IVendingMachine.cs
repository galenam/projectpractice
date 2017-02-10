using System.Collections.Generic;
using ProjectPracticeWeb.Models;

namespace ProjectPracticeWeb.AppCode
{
	public interface IVendingMachine
	{
		IList<Beverage> Beverages { get; set; }
		IDictionary<int, int> MachineCoins { get; set; }
		IDictionary<int, int> UserCoins { get; set; }
		int InsertedSum { get; set; }
		IState State { get; set; }

		bool InsertCoin(int nominal);
		bool ReturnCoins();
		bool TurnCrank();

		bool SellBeverage(Beverage bev);
	}
}