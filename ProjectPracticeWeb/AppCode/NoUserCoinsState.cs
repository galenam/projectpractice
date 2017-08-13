using System;
using ProjectPracticeWeb.Models;

namespace ProjectPracticeWeb.AppCode
{
    public class NoUserCoinsState:IState
    {
		public StateName NameOfState { get; } = StateName.NoUserCoins;

		public bool InsertCoin(int nominal, IVendingMachine vm)
	    {
			vm.State = new InsertedUserCoinsState();
			return true;
		}

		public bool ReturnCoins(IVendingMachine vm)
	    {
		    return false;
	    }

	    public bool TurnCrank(IVendingMachine vm, Beverage bev)
	    {
			return false;
		}

		public bool Dispense(IVendingMachine vm, Beverage bev)
	    {
			return false;
		}

		public override bool Equals(object obj)
		{
			return CommonMethods.EqualsState(obj, this);
		}
	}
}