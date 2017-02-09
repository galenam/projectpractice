using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using ProjectPracticeWeb.AppCode;

namespace ProjectPracticeWeb.Models
{
    public class VendingMachine
    {
        public IEnumerable<Beverage> Beverages { get; set; }
        public SortedDictionary<int,int> MachineCoins { get; set; }
        public SortedDictionary<int, int> UserCoins { get; set; }
        public int InsertedSum { get; set; }

	    public VendingMachine()
	    {
		    Beverages = CommonMethods.DeserializeJson<IEnumerable<Beverage>>("BeverageJson");
		    UserCoins = CommonMethods.DeserializeJson<SortedDictionary<int, int>>("UserPurseJson");
			MachineCoins = CommonMethods.DeserializeJson<SortedDictionary<int, int>>("VMPurseJson");
		}
	}
}