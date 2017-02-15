using ProjectPracticeWeb.Models;

namespace ProjectPracticeWeb.AppCode
{
	public class InsertedUserCoinsState : IState
	{
		public bool Dispense(IVendingMachine vm, Beverage bev)
		{
			return false;
		}

		public bool InsertCoin(int nominal, IVendingMachine vm)
		{
			return vm.InsertCoin(nominal);
		}

		public bool ReturnCoins(IVendingMachine vm)
		{
			vm.State = new NoUserCoinsState();
			return vm.ReturnCoins();
		}

		public bool TurnCrank(IVendingMachine vm, Beverage bev)
		{
			if (!vm.IsEnoughMoneyToSellBeverage(bev)) return false;
			
			vm.State = new SoldBeverageState();
			return true;
		}
	}
}