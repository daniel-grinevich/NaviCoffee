using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NaviCoffee.Models;
using NaviCoffee.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System;

namespace NaviCoffee.Tests.Phone
{
    [TestClass]
    public class PhoneTests
    {
        [TestInitialize]
        public void Init()
        {
            Mock<IPlatformServices> platformServices = new Mock<IPlatformServices>();            
            Device.PlatformServices = platformServices.Object;
        }

        [TestMethod]
        public void CoffeeCustomizationViewModel_CheckoutCommand_BarcodeShouldBeDifferentEachTime()
        {            
            Cart cart = new Cart { Coffee = new List<Coffee> { new Coffee { } } };
            CoffeeCustomizationViewModel vm = new CoffeeCustomizationViewModel(cart);            
            
            // Barcode should be created
            Assert.IsTrue(string.IsNullOrEmpty(cart.Barcode), "Barcode should be empty until order is completed");
            vm.CheckoutCommand.Execute(this);
            string barcode = cart.Coffee.First().Barcode;
            Assert.IsTrue(!string.IsNullOrEmpty(barcode), "Barcode should not be empty until order is completed");

            // Barcode should be different for each order
            vm.CheckoutCommand.Execute(this);
            string newBarcode = cart.Coffee.First().Barcode;
            Assert.AreNotEqual(barcode, newBarcode, "Barcode should be different for each order");
        }

        [TestMethod]
        public void CoffeeCustomizationViewModel_CheckoutCommand_SendsOrderToWebServer()
        {
            Cart cart = new Cart { Coffee = new List<Coffee> { new Coffee { CoffeeId = 1, Name = "Black", Label = "Black Coffee", Ingredients = Ingredients.Coffee, Image = "whitecoffeecup600.jpg", Price = 2.0m } }
            };
            CoffeeCustomizationViewModel vm = new CoffeeCustomizationViewModel(cart);

            vm.CheckoutCommand.Execute(this);

            DateTime start = DateTime.Now;
            do
            {
                // Wait for result of async operation
            } while (vm.Order.Status == Order.OrderStatus.None && start.AddSeconds(30) > DateTime.Now);

            Assert.AreEqual(Order.OrderStatus.SentToWebServer, vm.Order.Status, "Order should be sent to server");
        }
    }
}
