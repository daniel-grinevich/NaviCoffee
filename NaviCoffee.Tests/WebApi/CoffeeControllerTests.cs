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
    public class CoffeeControllerTests
    {
        const int ORDER_ID = 1;

        [TestMethod]
        public void CoffeeController_GetOrderById_ReturnsAnOrder()
        {
            var orderService = new Mock<IOrderService>();
            orderService.Setup(repo => repo.Get(ORDER_ID))
                .Returns(GetOrders().First(o => o.OrderId == ORDER_ID.ToString()));
            var controller = new CoffeeController(null, null, orderService.Object);

            var result = controller.Get(ORDER_ID.ToString());

            Assert.IsTrue(typeof(Order) == result.Value.GetType());
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CoffeeController_GetOrdersByStatus_ReturnsExpectedResult()
        {
            var orderService = new Mock<IOrderService>();
            orderService.Setup(repo => repo.Get(Order.OrderStatus.SentToWebServer))
                .Returns(GetOrders().Where(o => o.Status == Order.OrderStatus.SentToWebServer).ToList());
            var controller = new CoffeeController(null, null, orderService.Object);

            var result = controller.Get(Order.OrderStatus.SentToWebServer);

            Assert.IsTrue(typeof(List<Order>) == result.GetType());
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.Count());
        }

        [TestMethod]
        public void CoffeeController_UpdateOrder_Works()
        {
            var orderService = new Mock<IOrderService>();
            orderService.Setup(repo => repo.Get(Order.OrderStatus.SentToWebServer))
                .Returns(GetOrders().Where(o => o.Status == Order.OrderStatus.SentToWebServer).ToList());
            orderService.Setup(repo => repo.Update(It.IsAny<Order>()))
                .Returns(new Order { Status = Order.OrderStatus.PickedUpByArduino });
            var controller = new CoffeeController(null, null, orderService.Object);

            var result = controller.Get(Order.OrderStatus.SentToWebServer);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.Count());
            var order = result.First();

            Assert.AreNotEqual(Order.OrderStatus.PickedUpByArduino, order.Status);
            order.Status = Order.OrderStatus.PickedUpByArduino;
            var updateResult = controller.Update(order);
            Assert.IsNotNull(updateResult);
            Assert.AreEqual(Order.OrderStatus.PickedUpByArduino, order.Status);

        }

        private List<Order> GetOrders()
        {
            Coffee coffee = new Coffee
            {
                CoffeeId = 1,
                Ingredients = Ingredients.Coffee | Ingredients.Cold | Ingredients.Milk,
                Name = "Black"
            };

            var orders = new List<Order>()
            {
                new Order
                {
                    OrderId = ORDER_ID.ToString(),
                    CustomerId = 1,
                    OrderDate = DateTime.Now,
                    Status = Order.OrderStatus.SentToWebServer,
                    Coffees = new List<Coffee>{ coffee }
                },
                new Order
                {
                    OrderId = "1",
                    CustomerId = 1,
                    OrderDate = DateTime.Now,
                    Status = Order.OrderStatus.Complete,
                    Coffees = new List<Coffee>{ coffee }
                },
            };

            return orders;
        }
    }
}
