using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ProjectPracticeWeb.AppCode;
using ProjectPracticeWeb.Models;

namespace ProjectPracticeWeb.Controllers
{
    public class HomeController : ApiController
    {
		public VendingMachine VMachine { get; }

	    public HomeController()
	    {
			VMachine = new VendingMachine(CommonMethods.DeserializeJson<List<Beverage>>("BeverageJson"), 
			CommonMethods.DeserializeJson<SortedDictionary<int, int>>("UserPurseJson"),
			CommonMethods.DeserializeJson<SortedDictionary<int, int>>("VMPurseJson"));
	    }

	    public VendingMachine GetInitialScreen()
	    {
			return VMachine;
	    }

	    public bool InsertCoins(int nominal)
	    {

	    }
    }
}
