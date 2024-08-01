using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;
using NaviCoffee.PI.Models;
using static NaviCoffee.PI.Models.Order;
using Newtonsoft.Json;
using NaviCoffee.PI.Services;

namespace NaviCoffee.PI
{
    public class OrderService : IOrderService
    {
        private readonly IConfiguration _config;
        private readonly AppSettings _appSettings;
        private readonly object __orderLock = new object();
        private List<Order> _orders = new List<Order>();

        public OrderService()
        {
            _config = new ConfigurationBuilder()
             //.SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
             .Build();

            _appSettings = _config.GetSection("AppSettings").Get<AppSettings>();

        }

        public List<Order> CheckForOrders()
        {
            List<Order> orders = GetOrdersFromWebService(Order.OrderStatus.SentToWebServer).Result;
            orders.AddRange(GetOrdersFromWebService(Order.OrderStatus.PickedUpByArduino).Result);

            return orders;
        }

        public void MarkOrdersAsPickedUp(List<Order> orders)
        {
            lock (__orderLock)
            {
                foreach (var order in orders)
                {                    
                    // Only mark the order as picked up once.
                    if (!_orders.Any(o => o.OrderId == order.OrderId) &&
                        // This could be a run with orders that are marked as PickedUpByArduino after a reboot.  Don't lose those.
                        // Get them AND the SentToWebServerOrders
                        (order.Status == OrderStatus.SentToWebServer || order.Status == OrderStatus.PickedUpByArduino))
                    {
                        order.Status = OrderStatus.PickedUpByArduino;
                        UpdateOrderOnWebService(order);
                        _orders.Add(order);
                    }
                }                                
            }
        }

        public void AddBarcodeScan(string barcode)
        {
            using (StreamWriter writer = new StreamWriter(_appSettings.Barcodes_Read_Path, true))
            {
                writer.WriteLine(barcode);
            }
        }

        public void AddSyrupRequest(string barcode)
        {
            if (_appSettings.RunSyrupModule)
            {
                //using (StreamWriter writer = new StreamWriter(_appSettings.Syrup_Read_Path, true))
                //{
                //    writer.WriteLine(barcode);
                //}
                SyrupService syrupService = new SyrupService();
                syrupService.AddSyrup(_appSettings.Syrup_Timer_In_Milliseconds);
            }
        }

        /// <summary>
        /// Checks for barcode in barcodes file.  Removes from file if found in orders list and returns an order
        /// </summary>
        /// <param name="orders"></param>
        /// <returns></returns>
        public Coffee CheckForBarcodeScan(List<Order> orders)
        {
            Coffee ret = null;

            List<string> fileContents = new List<string>();
            //Console.WriteLine("Checking barcode file for new barcode scans...");
            using (StreamReader reader = new StreamReader(_appSettings.Barcodes_Read_Path))
            {
                do
                {
                    fileContents.Add(reader.ReadLine());
                } while (!reader.EndOfStream);
            }

            if (fileContents.Any())
            {
                //Console.WriteLine($"Barcodes found: {fileContents.Count}");

                string barcode = fileContents.First();
                Order order = CheckForOrderWithBarcode(orders, barcode);
                if (order != null)
                {                    
                    ret = order.Coffees.FirstOrDefault(c => c.Barcode.ToUpper() == barcode.ToUpper());

                    Console.WriteLine($"Barcode match found: {ret.Barcode}");

                    if (order.Coffees.Any(c => c.Ingredients.HasFlag(Ingredients.Syrup)))
                    {
                        MarkOrder(order, OrderStatus.SyrupRequested);
                    }
                    else
                    {
                        MarkOrder(order, OrderStatus.RequestedAtMachine);
                    }
                    
                    
                }                    
            }

            fileContents.Remove(fileContents.First());
            if (fileContents.Any())
            {
                using (StreamWriter sw = new StreamWriter(_appSettings.Barcodes_Read_Path, false))
                {
                    fileContents.ForEach(f => sw.WriteLine(f));
                }
            }
            else
            {
                using (StreamWriter sw = new StreamWriter(_appSettings.Barcodes_Read_Path, false))
                {
                    sw.WriteLine();
                }
            }

            return ret;
        }

        public bool MarkOrder(Order order, OrderStatus orderStatus)
        {
            bool success = false;

            Console.WriteLine($"Updating order {order.OrderId} as {orderStatus}");

            order.Status = orderStatus;
            success = UpdateOrderOnWebService(order);

            if (!success)
            {
                Console.WriteLine($"Unable to update order {order.OrderId} as {orderStatus}");
            }

            return success;
        }

        private async Task<List<Order>> GetOrdersFromWebService(Order.OrderStatus orderStatus)
        {
            List<Order> ret = new List<Order>();

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_appSettings.WebApi);

                    // Grab orders that haven't been picked up by arduino yet
                    var response = await client.GetAsync($"api/coffee/status/{orderStatus.ToString()}");

                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        ret = JsonConvert.DeserializeObject<List<Order>>(result);
                    }
                    else
                    {
                        Console.WriteLine($"Get order from {_appSettings.WebApi} failed with code {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetOrdersFromWebService: {ex.Message}");
            }
            return ret;
        }

        private bool UpdateOrderOnWebService(Order order)
        {
            bool ret = false;

            try
            {

                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_appSettings.WebApi);

                    var serializedItem = JsonConvert.SerializeObject(order);
                    var response = client.PutAsync($"api/Coffee", new StringContent(serializedItem, Encoding.UTF8, "application/json")).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"Update order succeeded: {order.ToString()}");
                        ret = true;
                    }
                    else
                    {
                        Console.WriteLine($"Update order failed: {order.ToString()}");
                        ret = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UpdateOrderOrWebService: {ex.Message}");
            }
            return ret;
        }

        private Order CheckForOrderWithBarcode(List<Order> orders, string barcode)
        {
            return orders.FirstOrDefault(o => o.Coffees.Any(c => c.Barcode.ToUpper() == barcode.ToUpper()));
        }

        
    }
}
