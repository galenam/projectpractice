using System.Web.Http;
using ProjectPracticeWeb.Models;

namespace ProjectPracticeWeb.Controllers
{
    public class HomeController : ApiController
    {
	    public VendingMachine GetInitialScreen()
	    {
		    return new VendingMachine();
	    }
    }
}
