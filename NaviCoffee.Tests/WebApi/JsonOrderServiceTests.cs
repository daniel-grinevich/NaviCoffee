using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NaviCoffee.WebApi.Controllers;
using NaviCoffee.WebApi.Models;
using NaviCoffee.WebApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace NaviCoffee.Tests
{
    [TestClass]
    public class JsonOrderServiceTests
    {
        const int ORDER_ID = 1;

        [TestMethod]
        public void JsonOrderService_CRUD_Works()
        {
            List<string> orderIds = new List<string>();

            Mock<ILogger<JsonOrderService>> loggerService = new Mock<ILogger<JsonOrderService>>();
            Mock<IWebHostEnvironment> mockEnv = new Mock<IWebHostEnvironment>();

            JsonOrderService orderService = new JsonOrderService(loggerService.Object, mockEnv.Object);

            try
            {

                //// Create 
                Assert.IsTrue(orderService.AddAsync(BuildOrder()).Result);

                //// Get one
                var order = orderService.Get(ORDER_ID);
                Assert.IsNotNull(order);
                orderIds.Add(order.OrderId);

                //// Get all
                var newOrder = BuildOrder();
                newOrder.OrderId = "2";
                Assert.IsTrue(orderService.AddAsync(newOrder).Result);
                var orders = orderService.Get();
                Assert.IsNotNull(order);
                Assert.AreNotEqual(0, orders.Count());
                orderIds.Add(newOrder.OrderId);

                //// Update / Get by status
                var arduinoOrders = orderService.Get(Order.OrderStatus.PickedUpByArduino);
                Assert.AreEqual(0, arduinoOrders.Count(), "There should be no orders with a status of 'PickedUpByArduino'");
                // Update
                var orderToUpdate = orders.First();
                orderToUpdate.Status = Order.OrderStatus.PickedUpByArduino;
                var updatedOrder = orderService.Update(orderToUpdate);
                Assert.IsNotNull(updatedOrder);
                Assert.AreEqual(orderToUpdate.Status, updatedOrder.Status);
                arduinoOrders = orderService.Get(Order.OrderStatus.PickedUpByArduino);
                Assert.AreEqual(1, arduinoOrders.Count(), "There should be one order of 'PickedUpByArduino'");
            }
            finally
            {
                foreach (var id in orderIds)
                {
                    orderService.DeleteAsync(int.Parse(id));
                }
            }
        }

        private Order BuildOrder()
        {
            Coffee coffee = new Coffee
            {
                CoffeeId = 1,
                Ingredients = Ingredients.Coffee | Ingredients.Cold | Ingredients.Milk,
                Name = "Black",
                Image = "fake image",
                Label = "a label",
                Price = 5.5m
            };

            return new Order
            {
                OrderId = ORDER_ID.ToString(),
                CustomerId = 1,
                OrderDate = DateTime.Now,
                Status = Order.OrderStatus.SentToWebServer,
                Coffees = new List<Coffee> { coffee }
            };
        }
    }
}
