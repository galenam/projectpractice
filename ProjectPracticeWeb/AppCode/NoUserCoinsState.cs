using ProjectPracticeWeb.Models;

namespace ProjectPracticeWeb.AppCode
{
    public class NoUserCoinsState:IState
    {
	    public bool InsertCoin(int nominal, IVendingMachine vm)
	    {
			//return vm.InsertCoin(nominal);
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
	}
}