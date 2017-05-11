using System.Collections.Generic;
using System.Linq;
using ProjectPracticeWeb.AppCode;

namespace ProjectPracticeWeb.Models
{
    public class VendingMachine:IVendingMachine
    {
        public Dictionary<string, Beverage> Beverages { get; set; }
        public SortedDictionary<int, int> MachineCoins { get; set; }
        public SortedDictionary<int, int> UserCoins { get; set; }
        public int InsertedSum { get; set; }
		public IState State { get; set; }

	    public VendingMachine(Dictionary<string, Beverage> _bev, SortedDictionary<int, int> _userP, SortedDictionary<int, int> _machineP)
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
		    MachineCoins[nominal]++;
			InsertedSum += nominal;
		    State.InsertCoin(nominal, this);
		    return true;
	    }

	    public bool ReturnCoins()
	    {
		    var sumToReturn = InsertedSum;

		    foreach (var nominal in MachineCoins.Keys.Reverse())
		    {
			    var countHasWithNominal = MachineCoins[nominal];
			    var countNeededWithNominal = sumToReturn / nominal;
			    var countToReturn = countNeededWithNominal >= countHasWithNominal ? countHasWithNominal : countNeededWithNominal;
			    UserCoins[nominal] +=  countToReturn;
			    MachineCoins[nominal] -= countToReturn;
			    sumToReturn -= countToReturn * nominal;
			    if (sumToReturn <= 0) break;
		    }
		    if (sumToReturn > 0) return false;

		    InsertedSum = 0;
		    State.ReturnCoins(this);
		    return true;
	    }

		public bool TurnCrank(string bevName)
	    {
			var bev = Beverages.ContainsKey(bevName) ? Beverages[bevName] : null;
			if (bev == null) { return false; }
		    State.TurnCrank(this, bev);
		    State.Dispense(this, bev);
			return true;
		}


	    public bool IsEnoughMoneyToSellBeverage(Beverage bev)
	    {
		    return InsertedSum >= bev.Cost;
	    }

	    public bool ReleaseBeverage(Beverage bev)
	    {
			if (!IsEnoughMoneyToSellBeverage(bev)) return false;
		    if (Beverages[bev.Name] == null) return false;
		    Beverages[bev.Name].Count--;
			InsertedSum -= bev.Cost;
		    if (!Beverages.Any(b => b.Value.Count > 0)) State = new NoBeverageState();
		    if (InsertedSum == 0) State = new InsertedUserCoinsState();
		    State = new NoUserCoinsState();
		    return true;
	    }

	    public bool HasMoneyInserted()
	    {
		    return InsertedSum>0;
	    }
    }
}