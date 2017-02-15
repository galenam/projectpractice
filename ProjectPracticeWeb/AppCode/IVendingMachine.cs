using System.Collections.Generic;
using ProjectPracticeWeb.Models;

namespace ProjectPracticeWeb.AppCode
{
	public interface IVendingMachine
	{
		int InsertedSum { get; set; }
		IState State { get; set; }

		bool InsertCoin(int nominal);
		bool ReturnCoins();
		bool TurnCrank(Beverage bev);
		bool ReleaseBeverage(Beverage bev);

		bool IsEnoughMoneyToSellBeverage(Beverage bev);
		bool HasMoneyInserted();
	}
}