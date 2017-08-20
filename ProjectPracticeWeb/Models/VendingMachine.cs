using System.Collections.Generic;
using System.Linq;
using ProjectPracticeWeb.AppCode;
using System;
using System.Text;

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
		    if (!State.TurnCrank(this, bev))
		    {
			    return false;
			}

		    return State.Dispense(this, bev);
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

		public override bool Equals(object obj)
		{
			if (obj == null || obj.GetType() != typeof(VendingMachine))
			{
				return false;
			}

			var vm = (VendingMachine)obj;
			if (!CompareSequenceAndCycle(Beverages, vm.Beverages, (el1, el2) => el1.Value.Equals(el2.Value)))
			{
				return false;
			}
			if (!CompareSequenceAndCycle(MachineCoins, vm.MachineCoins, (el1, el2) => el1.Value.Equals(el2.Value)))
			{
				return false;
			}
			if (!CompareSequenceAndCycle(UserCoins, vm.UserCoins, (el1, el2) => el1.Value.Equals(el2.Value)))
			{
				return false;
			}
			if (InsertedSum != vm.InsertedSum) { return false; }
			return true;
		}

		private bool CompareSequenceAndCycle<T>(IEnumerable<T> first, IEnumerable<T> second, Func<T,T, bool> func)
		{
			if (!CompareWithNullAndAny(first, second))
			{
				return false;
			}
			foreach (var elementInFirst in first)
			{
				var equalElement = second.FirstOrDefault(elementInSecond => func(elementInFirst,elementInSecond ));
				if (equalElement.Equals(default(T))) { return false; }
			}
			return true;
		}


		private bool CompareWithNullAndAny<T>(IEnumerable<T> firstSequence, IEnumerable<T> secondSequence)
		{
			if (firstSequence == null && secondSequence == null)
			{
				return true;
			}
			if (firstSequence == null && secondSequence != null)
			{
				return false;
			}
			if (firstSequence != null && secondSequence == null)
			{
				return false;
			}
			if (firstSequence.Count()!=secondSequence.Count())
			{
				return false;
			}
			return true;
		}

		public override int GetHashCode()
		{

			var sBuilder = new StringBuilder();
			var stringPresentationBeverages = Beverages != null ? Beverages.Aggregate(new StringBuilder(), (sb, bev2) => sb.Append(bev2.ToString())) : null;
			var stringPresentationMachineCoins = MachineCoins != null ? MachineCoins.Aggregate(new StringBuilder(), (sb, mCoin) => sb.Append(mCoin.ToString())) : null;
			var stringPresentationUserCoins = UserCoins != null ? UserCoins.Aggregate(new StringBuilder(), (sb, uCoin) => sb.Append(uCoin.ToString())) : null;

			var hashString = CommonMethods.GetHashString(stringPresentationBeverages?.ToString()+stringPresentationMachineCoins?.ToString()
				+stringPresentationUserCoins?.ToString());
			return int.Parse(hashString);
		}
	}
}