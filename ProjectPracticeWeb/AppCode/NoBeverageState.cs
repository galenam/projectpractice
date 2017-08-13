using ProjectPracticeWeb.Models;

namespace ProjectPracticeWeb.AppCode
{
    public class NoBeverageState:IState
    {
		public StateName NameOfState { get; } = StateName.NoBeverage;

		public bool InsertCoin(int nominal, IVendingMachine vm)
	    {
		    return ReturnCoins(vm);
	    }

	    public bool ReturnCoins(IVendingMachine vm)
	    {
			vm.ReturnCoins();
			vm.State = new NoUserCoinsState();
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