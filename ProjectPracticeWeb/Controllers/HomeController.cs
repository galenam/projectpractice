using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ProjectPracticeWeb.AppCode;
using ProjectPracticeWeb.Models;

namespace ProjectPracticeWeb.Controllers
{
    public class HomeController : ApiController
    {
		public static VendingMachine VMachine { get; }

	    static HomeController()
	    {
		    var tmp = CommonMethods.DeserializeJson<List<Beverage>>("BeverageJson");
			var beverages = tmp.Any() ? tmp.ToDictionary(val => val.Name, val=> val) : null;
			VMachine = new VendingMachine(beverages, 
				CommonMethods.DeserializeJson<SortedDictionary<int, int>>("UserPurseJson"),
				CommonMethods.DeserializeJson<SortedDictionary<int, int>>("VMPurseJson"));
	    }
		
		public IHttpActionResult Get()
	    {
			return Ok(VMachine);
	    }
		
		public IHttpActionResult Put(int nominal)
	    {
		// uncomment FromUrl
		    //var nominal = 0;
		    var res = VMachine.InsertCoin(nominal);
		    return Ok(new
			{
				Success = res,
				VMachine.InsertedSum,
				VMachine.UserCoins,
				VMachine.MachineCoins
			});
	    }

	    public IHttpActionResult ReturnCoins()
	    {
			var res = VMachine.ReturnCoins();
			return Ok(new
			{
				Success = res,
				VMachine.UserCoins,
				VMachine.MachineCoins
			});
		}

	    public IHttpActionResult Post(Beverage bev)
	    {
			var res = VMachine.TurnCrank(bev);
			return Ok(new
			{
				Success = res,
				VMachine.Beverages,
				VMachine.InsertedSum
			});
		}
    }
}
