using System;
using System.Linq;
using System.Net;
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
		public void TestMethodGet()
		{
			var initialVm = GetVM();
			var controller = new HomeController(initialVm);

			var result = controller.Get();
			Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<IVendingMachine>));
			var vm = ((OkNegotiatedContentResult<IVendingMachine>)result).Content;
			Assert.AreEqual(vm, GetVM());
		}

		// todo : разобраться с инициализацией значений (напр. initialVM) общим методом
		[TestMethod]
		public void TestMethodPut()
		{
			var initialVm = GetVM();
			var nominal = 1;
			initialVm.InsertedSum += nominal;
			if (initialVm.UserCoins.ContainsKey(nominal))
			{
				initialVm.UserCoins[nominal]++;
			}

			var controller = new HomeController(initialVm);
			var result = controller.Put(nominal);
			
			var putAnonimusResult = new PrivateObject(result);
			var content = new PrivateObject(putAnonimusResult.GetFieldOrProperty("Content"));
			var success = (bool)content.GetProperty("Success");
			var insertedSum = (int)content.GetProperty("InsertedSum");
			var userCoins = (System.Collections.Generic.SortedDictionary<int, int>)content.GetProperty("UserCoins");
			var machineCoins = (System.Collections.Generic.SortedDictionary<int, int>)content.GetProperty("MachineCoins");

			
			//var tmp = new VendingMachine(initialVm.Beverages, );





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
			var machineP = new System.Collections.Generic.SortedDictionary<int, int> {
				{ 1, 100},
				{2, 100 },
				{5, 100 }
			};
			return new VendingMachine(bev, userPurse, machineP);
		}
	}
}
