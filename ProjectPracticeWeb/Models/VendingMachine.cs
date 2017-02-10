using System.Collections.Generic;
using ProjectPracticeWeb.AppCode;

namespace ProjectPracticeWeb.Models
{
    public class VendingMachine:IVendingMachine
    {
        public IList<Beverage> Beverages { get; set; }
        public IDictionary<int, int> MachineCoins { get; set; }
        public IDictionary<int, int> UserCoins { get; set; }
        public int InsertedSum { get; set; }
		public IState State { get; set; }

	    public VendingMachine(IList<Beverage> _bev, IDictionary<int, int> _userP, IDictionary<int, int> _machineP)
	    {
		    Beverages = _bev;
		    UserCoins = _userP;
		    MachineCoins = _machineP;
		    State = new NoUserCoinsState();
	    }

	    public bool InsertCoin(int nominal)
	    {
		    if (!UserCoins.ContainsKey(nominal) || UserCoins[nominal] == 0) return false;
		    
			UserCoins[nominal]--;
			InsertedSum += nominal;
		    return true;
	    }

	    public bool ReturnCoins()
	    {
		    return false;
	    }

	    public bool TurnCrank()
	    {
			return false;
		}


	    public bool SellBeverage(Beverage bev)
	    {
		    return false;
	    }
    }
}