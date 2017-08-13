using ProjectPracticeWeb.Models;

namespace ProjectPracticeWeb.AppCode
{
	public class InsertedUserCoinsState : IState
	{
		public StateName NameOfState { get; } = StateName.InsertedUserCoins;


		public bool Dispense(IVendingMachine vm, Beverage bev)
		{
			return false;
		}

		public bool InsertCoin(int nominal, IVendingMachine vm)
		{
			return true;
			//return vm.InsertCoin(nominal);
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

		public override bool Equals(object obj)
		{
			return CommonMethods.EqualsState(obj, this);
		}
	}
}