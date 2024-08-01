using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using NaviCoffee.WebApi.Models;
using Newtonsoft.Json;

namespace NaviCoffee.WebApi.Services
{    
    public class JsonOrderService : IOrderService
    {
        private readonly string _fileLocation = @"./allTheCoffee.json";
        private readonly ILogger<JsonOrderService> _logger;

        public JsonOrderService(ILogger<JsonOrderService> logger, IWebHostEnvironment env)
        {
            _logger = logger;
        }

        public async Task<bool> AddAsync(Order order)
        {
            var orders = GetAllOrdersFromFile(_fileLocation);
            if (orders == null)
            {
                orders = new List<Order>();
            }
            orders.Add(order);

            Save(orders);

            return true;
        }

        public Task<bool> DeleteAsync(int id)
        {
            var orders = GetAllOrdersFromFile(_fileLocation);

            var orderToDelete = orders.FirstOrDefault(o => o.OrderId == id.ToString());

            if (orderToDelete == null)
            {
                throw new Exception($"Order to delete not found: {id}");
            }

            orders.Remove(orderToDelete);

            Save(orders);

            return Task.FromResult(true);
        }

        public Order Get(int id)
        {
            // Get order by order id
            var orders = GetAllOrdersFromFile(_fileLocation);
            var order = orders.FirstOrDefault(o => o.OrderId == id.ToString());

            return order;
        }

        public IEnumerable<Order> Get()
        {
            return GetAllOrdersFromFile(_fileLocation);
        }

        public IEnumerable<Order> Get(Order.OrderStatus status)
        {
            var orders = GetAllOrdersFromFile(_fileLocation);
            return orders.Where(o => o.Status == status);
        }

        public Order Update(Order order)
        {
            var orders = GetAllOrdersFromFile(_fileLocation);

            var orderToUpdate = orders.FirstOrDefault(o => o.OrderId == order.OrderId);

            if (orderToUpdate == null)
            {
                throw new Exception($"Order not found: {order.OrderId}");
            }

            orders.Remove(orderToUpdate);
            orders.Add(order);

            Save(orders);

            return order;
        }

        private List<Order> GetAllOrdersFromFile(string file)
        {
            List<Order> orders;
            try
            {
                string fileContents = File.ReadAllText(file);
                orders = JsonConvert.DeserializeObject<List<Order>>(fileContents);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to get orders from file. {ex.Message}");
                orders = new List<Order>();
            }

            return orders;
        }

        private void Save(List<Order> orders)
        {
            var serializedOrders = JsonConvert.SerializeObject(orders);

            using (StreamWriter writer = new StreamWriter(_fileLocation, false))
            {
                writer.Write(serializedOrders);
            }
        }
    }
}
