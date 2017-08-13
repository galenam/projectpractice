using ProjectPracticeWeb.Models;

namespace ProjectPracticeWeb.AppCode
{
	public interface IState
	{
		bool InsertCoin(int nominal, IVendingMachine vm);
		bool ReturnCoins(IVendingMachine vm);
		bool TurnCrank(IVendingMachine vm, Beverage bev);
		bool Dispense(IVendingMachine vm, Beverage bev);

		StateName NameOfState {get;}
    }

	public enum StateName
	{
		InsertedUserCoins,
		NoBeverage,
		NoUserCoins,
		SoldBeverage
	}
}