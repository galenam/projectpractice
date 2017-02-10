namespace ProjectPracticeWeb.AppCode
{
    public class NoUserCoinsState:IState
    {
	    public bool InsertCoin(int nominal, IVendingMachine vm)
	    {
		    return vm.InsertCoin(nominal);
	    }

	    public bool ReturnCoins(IVendingMachine vm)
	    {
		    return false;
	    }

	    public bool TurnCrank(IVendingMachine vm)
	    {
			return false;
		}

		public bool Dispense(IVendingMachine vm)
	    {
			return false;
		}
	}
}