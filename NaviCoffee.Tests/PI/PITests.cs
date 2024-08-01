using Microsoft.VisualStudio.TestTools.UnitTesting;
using NaviCoffee.PI;
using NaviCoffee.PI.Models;
using NaviCoffee.PI.Services;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NaviCoffee.Tests
{
    [TestClass]
    public class PITest
    {
        const string BARCODES_READ_PATH = "barcodes.txt";

        [TestMethod]
        public void PI_CoffeeMakerService_Can_MakeCoffee()
        {
            CoffeeMakerService service = new CoffeeMakerService();
            Assert.IsTrue(service.MakeCoffee(new Coffee { Name = "Black" }), "Should be able to make coffee");
        }

        [TestMethod]
        public void PI_OrderService_CheckforBarcodeScan_ReturnsExpectedResult()
        {
            OrderService orderService = new OrderService();

            string barcode = "good barcode";
            FakeBarcodeScanResult(barcode);

            List<Order> orders = new List<Order>
            {
                new Order { Coffees = new List<Coffee> { new Coffee { Barcode = "bad barcode" } } }
            };
            Assert.IsNull(orderService.CheckForBarcodeScan(orders), "Should not find bad barcode");

            orders.Clear();
            orders.Add(new Order { Coffees = new List<Coffee> { new Coffee { Barcode = "good barcode" } } });
            Assert.IsNotNull(orderService.CheckForBarcodeScan(orders), "Should find good barcode");
        }

        /// <summary>
        /// Fake results of barcodeScanner.py scanning a barcode
        /// </summary>
        /// <param name="barcode"></param>
        private void FakeBarcodeScanResult(string barcode)
        {
            using (StreamWriter sw = new StreamWriter(BARCODES_READ_PATH, true))
            {
                sw.WriteLine(barcode);
            }
        }
    }
}
