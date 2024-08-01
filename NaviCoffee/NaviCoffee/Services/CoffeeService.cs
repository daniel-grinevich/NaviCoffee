using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NaviCoffee.Models;
using Newtonsoft.Json;

namespace NaviCoffee.Services
{
    // Implements the IDataStore interface for Coffee objects.
    public class CoffeeService : IDataStore<Coffee>
    {
        private List<Coffee> _coffees; // List to store coffee objects.
        private Cart _cart; // Cart object to manage coffee orders.

        // Constructor initializes the Coffee list with predefined items.
        public CoffeeService()
        {            
            _coffees = new List<Coffee>
            {
                new Coffee { CoffeeId = 1, Name = "Black", CoffeeType = CoffeeType.Black, Label = "Black Coffee", Ingredients = Ingredients.Coffee, Image = "whitecoffeecup600.jpg", Price = 2.0m },
                new Coffee { CoffeeId = 2, Name = "Latte", CoffeeType = CoffeeType.Latte, Label = "Latte", Ingredients = Ingredients.Espresso | Ingredients.Milk, Image = "browncoffeecup600.jpg", Price = 2.5m },
                new Coffee { CoffeeId = 3, Name = "Espresso", CoffeeType = CoffeeType.Espresso, Label = "Espresso", Ingredients = Ingredients.Espresso, Image = "blackcoffeecup@600.jpg", Price = 2.5m },
            };

            _cart = new Cart();
        }

        // Asynchronously adds an order to the web server and updates its status based on the operation success.
        public async Task<Order> AddOrderAsync(Order order)
        {
            Debug.WriteLine($"CoffeeService.AddOrderAsync {order}");

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(App.AzureBackendUrl); // Set the base URL for the HTTP client.

                var serializedItem = JsonConvert.SerializeObject(order); // Serialize the order object to JSON.

                HttpResponseMessage response = null;
                try
                {
                    // Post the serialized order to the server using the API endpoint.
                    response = await client.PostAsync($"api/Coffee", new StringContent(serializedItem, Encoding.UTF8, "application/json"));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"CoffeeService.AddOrderAsync error adding order {order}: {ex.Message}");
                    response = new HttpResponseMessage(statusCode: System.Net.HttpStatusCode.BadRequest); // Handle exceptions by setting response to BadRequest.
                }

                if (response.IsSuccessStatusCode)
                {
                    order.Status = Order.OrderStatus.SentToWebServer; // Set order status to sent if the response is successful.
                }
                else
                {
                    // Optionally handle failed send here.
                    // order.Status = Order.OrderStatus.SendToWebServerFailed;
                }
            }

            return order;
        }

        // Not implemented method for deleting an item by ID.
        public Task<bool> DeleteItemAsync(int id)
        {
            throw new NotImplementedException();
        }

        // Retrieves a Coffee item by its ID.
        public async Task<Coffee> GetItemAsync(int id)
        {
            // Return the first coffee that matches the given ID or null if not found.

            // This is used because FindAsync would be more related to a DB call not in mem.
            return await Task.FromResult(_coffees.FirstOrDefault(s => s.CoffeeId == id));
        }

        // Retrieves all Coffee items.
        public async Task<IEnumerable<Coffee>> GetItemsAsync(bool forceRefresh = false)
        {
            // Return all coffees. The forceRefresh parameter can be used to re-fetch data if implemented.
            return await Task.FromResult(_coffees);            
        }

        // Not implemented method for updating a coffee item.
        public Task<bool> UpdateItemAsync(Coffee item)
        {
            throw new NotImplementedException();
        }

        // Adds a coffee item to the cart and returns success.
        public async Task<bool> AddItemToCartAsync(Coffee item)
        {
            _cart.Coffee.Add(item); // Add the coffee item to the cart.
            return await Task.FromResult(true); // Return true indicating the item was added successfully.
        }        
    }
}