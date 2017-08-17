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
		// todo : что будет, если я внесу номинал, которого нет в кошельке, например, 11 рублей одним вызовом ?
		// todo : что вернет метод Delete, если пытаться вернуть деньги из автомата  до внесения ?
		[TestMethod]
		public void TestMethodPut()
		{
			var initialVm = GetVM();
			var nominal = 1;
			
			var controller = new HomeController(initialVm);
			var result = controller.Put(nominal);
			
			var putAnonimusResult = new PrivateObject(result);
			var content = new PrivateObject(putAnonimusResult.GetFieldOrProperty("Content"));
			var success = (bool)content.GetProperty("Success");
			Assert.AreEqual(success, true, "Operation insert failed");

			var insertedSum = (int)content.GetProperty("InsertedSum");
			var userCoins = (System.Collections.Generic.SortedDictionary<int, int>)content.GetProperty("UserCoins");
			var machineCoins = (System.Collections.Generic.SortedDictionary<int, int>)content.GetProperty("MachineCoins");

			var vendingMachineWithCoins = new VendingMachine(initialVm.Beverages, userCoins , machineCoins){InsertedSum = insertedSum, State = new InsertedUserCoinsState()};

			var testVendingMachine = GetVM();
			testVendingMachine.InsertedSum += nominal;
			if (testVendingMachine.UserCoins.ContainsKey(nominal))
			{
				testVendingMachine.UserCoins[nominal]++;
			}

			Assert.AreEqual(vendingMachineWithCoins, initialVm);
		}

		[TestMethod]
		public void TestMethodDelete()
		{
			var initialVm = GetVM();

			// клиент накидал 56 р 1-рублевыми монетами
			var controller = new HomeController(initialVm);
			for (var i = 0; i++ < 56;)
			{
				controller.Put(1);
			}

			var result = controller.Delete();

			var putAnonimusResult = new PrivateObject(result);
			var content = new PrivateObject(putAnonimusResult.GetFieldOrProperty("Content"));
			var success = (bool)content.GetProperty("Success");
			Assert.AreEqual(success, true, "Operation Delete failed");

			var insertedSum = (int)content.GetProperty("InsertedSum");
			var userCoins = (System.Collections.Generic.SortedDictionary<int, int>)content.GetProperty("UserCoins");
			var machineCoins = (System.Collections.Generic.SortedDictionary<int, int>)content.GetProperty("MachineCoins");

			var vendingMachineWithCoins = new VendingMachine(initialVm.Beverages, userCoins, machineCoins) { InsertedSum = insertedSum, State = new NoUserCoinsState() };

			// автомат должен вернуть максимально крупными
			var testVendingMachine = GetVM();
			testVendingMachine.UserCoins[5]-=11;
			testVendingMachine.UserCoins[1]-- ;

			Assert.AreEqual(vendingMachineWithCoins, initialVm);
		}

		[TestMethod]
		public void TestMethodPost()
		{

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
