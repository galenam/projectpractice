using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectPracticeWeb.Controllers;
using ProjectPracticeWeb.AppCode;
using ProjectPracticeWeb.Models;
using System.Web.Http;
using System.Web.Http.Results;

namespace UnitTestProject
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void TestMethod1()
		{
			var controller = new HomeController(GetVM());
			var result = controller.Get();
			Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<IVendingMachine>));
			// преобразовать тип в результ, посмотреть, что правильно распарсился
		}

		private IVendingMachine GetVM()
		{
			var bev = new System.Collections.Generic.Dictionary<string, Beverage>
			{
				{ "Tea", new Beverage { Cost=1, Count=100, Name="Tea"} },
				{ "Coffee", new Beverage { Cost=10, Count=100, Name="Coffee"} },
				{ "Cacao", new Beverage { Cost=20, Count=100, Name="Cacao"} },

			};
			var userPurse = new System.Collections.Generic.SortedDictionary<int, int> {
				{1,10 },
				{ 2,10},
				{ 5, 10}
			};
			var _machineP = new System.Collections.Generic.SortedDictionary<int, int> {
				{ 1, 100},
				{2, 100 },
				{5, 100 }
			};
			return new VendingMachine(bev, userPurse, _machineP);
		}
	}
}
