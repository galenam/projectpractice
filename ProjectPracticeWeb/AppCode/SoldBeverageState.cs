using System;
using ProjectPracticeWeb.Models;

namespace ProjectPracticeWeb.AppCode
{
	public class SoldBeverageState : IState
	{
		public bool Dispense(IVendingMachine vm)
		{
			
		}

		public bool InsertCoin(int nominal, IVendingMachine vm)
		{
			return false;
		}

		public bool ReturnCoins(IVendingMachine vm)
		{
			return false;
		}

		public bool TurnCrank(IVendingMachine vm)
		{
			return false;
		}

		public bool TurnCrank(IVendingMachine vm, Beverage bev)
		{
			throw new NotImplementedException();
		}
	}
}