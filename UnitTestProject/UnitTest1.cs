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
		// todo : что будет, если я попытаюсь перевести машине деньги, которых нет у клиента ? напр. 11-ю рублевую монету, при условии, что у него их только 10
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
			testVendingMachine.State = new InsertedUserCoinsState();
			testVendingMachine.InsertedSum += nominal;
			if (testVendingMachine.UserCoins.ContainsKey(nominal))
			{
				testVendingMachine.UserCoins[nominal]--;
			}
			if (testVendingMachine.MachineCoins.ContainsKey(nominal))
			{
				testVendingMachine.MachineCoins[nominal]++;
			}

			Assert.AreEqual(vendingMachineWithCoins, testVendingMachine);
		}
		
		[TestMethod]
		public void TestMethodDelete()
		{
			var initialVm = GetVM();

			// клиент накидал 10 р 1-рублевыми монетами
			var controller = new HomeController(initialVm);
			for (var i = 0; i++ < 10;)
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
			testVendingMachine.UserCoins[1] = 0 ;
			testVendingMachine.UserCoins[5] += 2;
			testVendingMachine.MachineCoins[1] += 10;
			testVendingMachine.MachineCoins[5]-=2;

			Assert.AreEqual(vendingMachineWithCoins, testVendingMachine);
		}
		
		[TestMethod]
		public void TestMethodPostSuccess()
		{
			var initialVm = GetVM();
			
			var controller = new HomeController(initialVm);
			controller.Put(1);
			
			var result = controller.Post("Tea");

			var putAnonimusResult = new PrivateObject(result);
			var content = new PrivateObject(putAnonimusResult.GetFieldOrProperty("Content"));
			var success = (bool)content.GetProperty("Success");
			Assert.AreEqual(success, true, "Operation Post failed");

			var insertedSum = (int)content.GetProperty("InsertedSum");
			var beverages = (System.Collections.Generic.Dictionary<string, Beverage>)content.GetProperty("Beverages");

			var vendingMachineWithCoins = new VendingMachine(beverages, initialVm.UserCoins, initialVm.MachineCoins) { InsertedSum = insertedSum, State = new SoldBeverageState() };
			
			var testVendingMachine = GetVM();
			testVendingMachine.Beverages["Tea"].Count--;
			testVendingMachine.MachineCoins[1]++;
			testVendingMachine.UserCoins[1]--;
			testVendingMachine.State = new SoldBeverageState();

			Assert.AreEqual(vendingMachineWithCoins, testVendingMachine);
		}

		[TestMethod]
		public void TestMethodPostUnsuccess()
		{
			var initialVm = GetVM();
			var nominal = 1;

			var controller = new HomeController(initialVm);
			controller.Put(nominal);

			var result = controller.Post("Coffee");

			var putAnonimusResult = new PrivateObject(result);
			var content = new PrivateObject(putAnonimusResult.GetFieldOrProperty("Content"));
			var success = (bool)content.GetProperty("Success");
			Assert.AreEqual(success, false, "Operation Post failed");

			var insertedSum = (int)content.GetProperty("InsertedSum");
			var beverages = (System.Collections.Generic.Dictionary<string, Beverage>)content.GetProperty("Beverages");

			var vendingMachineWithCoins = new VendingMachine(beverages, initialVm.UserCoins, initialVm.MachineCoins) { InsertedSum = insertedSum, State = new InsertedUserCoinsState() };

			var testVendingMachine = GetVM();
			testVendingMachine.State = new InsertedUserCoinsState();
			testVendingMachine.InsertedSum += nominal;
			testVendingMachine.MachineCoins[1]++;
			testVendingMachine.UserCoins[1]--;

			Assert.AreEqual(vendingMachineWithCoins, testVendingMachine);
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
