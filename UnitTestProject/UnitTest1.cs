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

			var vendingMachineWithCoins = new VendingMachine(initialVm.Beverages, userCoins , machineCoins){InsertedSum = insertedSum};

			var testVendingMachine = GetVM();
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
		public void TestMethodPutMoreThatCan()
		{
			var initialVm = GetVM();
			var nominal = 1;

			var controller = new HomeController(initialVm);
			IHttpActionResult result=null;
			for (var i = 0; i++ < 12;)
			{
				result = controller.Put(nominal);
			}
			var putAnonimusResult = new PrivateObject(result);
			var content = new PrivateObject(putAnonimusResult.GetFieldOrProperty("Content"));
			var success = (bool)content.GetProperty("Success");
			Assert.AreEqual(success, false, "Operation insert success, it's not right");

			var insertedSum = (int)content.GetProperty("InsertedSum");
			var userCoins = (System.Collections.Generic.SortedDictionary<int, int>)content.GetProperty("UserCoins");
			var machineCoins = (System.Collections.Generic.SortedDictionary<int, int>)content.GetProperty("MachineCoins");

			var vendingMachineWithCoins = new VendingMachine(initialVm.Beverages, userCoins, machineCoins) { InsertedSum = insertedSum };

			var testVendingMachine = GetVM();
			testVendingMachine.InsertedSum += 10;
			if (testVendingMachine.UserCoins.ContainsKey(nominal))
			{
				testVendingMachine.UserCoins[nominal]=0;
			}
			if (testVendingMachine.MachineCoins.ContainsKey(nominal))
			{
				testVendingMachine.MachineCoins[nominal]+=10;
			}

			Assert.AreEqual(vendingMachineWithCoins, testVendingMachine);
		}


		[TestMethod]
		public void TestMethodPutBadQuality()
		{
			var initialVm = GetVM();
			var nominal = 11;

			var controller = new HomeController(initialVm);
			var result = controller.Put(nominal);

			var putAnonimusResult = new PrivateObject(result);
			var content = new PrivateObject(putAnonimusResult.GetFieldOrProperty("Content"));
			var success = (bool)content.GetProperty("Success");
			Assert.AreEqual(success, false, "Operation insert success, it's not right");

			var insertedSum = (int)content.GetProperty("InsertedSum");
			var userCoins = (System.Collections.Generic.SortedDictionary<int, int>)content.GetProperty("UserCoins");
			var machineCoins = (System.Collections.Generic.SortedDictionary<int, int>)content.GetProperty("MachineCoins");

			var vendingMachineWithCoins = new VendingMachine(initialVm.Beverages, userCoins, machineCoins) { InsertedSum = insertedSum };

			var testVendingMachine = GetVM();

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

			var vendingMachineWithCoins = new VendingMachine(initialVm.Beverages, userCoins, machineCoins) { InsertedSum = insertedSum};

			// автомат должен вернуть максимально крупными
			var testVendingMachine = GetVM();
			testVendingMachine.UserCoins[1] = 0 ;
			testVendingMachine.UserCoins[5] += 2;
			testVendingMachine.MachineCoins[1] += 10;
			testVendingMachine.MachineCoins[5]-=2;

			Assert.AreEqual(vendingMachineWithCoins, testVendingMachine);
		}

		[TestMethod]
		public void TestMethodDeleteUnsuccessReturnMoneyBeforePut()
		{
			var initialVm = GetVM();
			
			var controller = new HomeController(initialVm);

			var result = controller.Delete();

			var putAnonimusResult = new PrivateObject(result);
			var content = new PrivateObject(putAnonimusResult.GetFieldOrProperty("Content"));
			var success = (bool)content.GetProperty("Success");
			Assert.AreEqual(success, true, "Operation Delete success, it's not right");

			var insertedSum = (int)content.GetProperty("InsertedSum");
			var userCoins = (System.Collections.Generic.SortedDictionary<int, int>)content.GetProperty("UserCoins");
			var machineCoins = (System.Collections.Generic.SortedDictionary<int, int>)content.GetProperty("MachineCoins");

			var vendingMachineWithCoins = new VendingMachine(initialVm.Beverages, userCoins, machineCoins) { InsertedSum = insertedSum };

			// автомат должен вернуть максимально крупными
			var testVendingMachine = GetVM();

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

			var vendingMachineWithCoins = new VendingMachine(beverages, initialVm.UserCoins, initialVm.MachineCoins) { InsertedSum = insertedSum};
			
			var testVendingMachine = GetVM();
			testVendingMachine.Beverages["Tea"].Count--;
			testVendingMachine.MachineCoins[1]++;
			testVendingMachine.UserCoins[1]--;

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

			var vendingMachineWithCoins = new VendingMachine(beverages, initialVm.UserCoins, initialVm.MachineCoins) { InsertedSum = insertedSum};

			var testVendingMachine = GetVM();
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
