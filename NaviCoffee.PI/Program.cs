using NaviCoffee.PI.Models;
using NaviCoffee.PI.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace NaviCoffee.PI
{    
    class Program
    {
        //private static HidService _hidService = new HidService();

        static void Main(string[] args)
        {           
            Console.WriteLine(GetMug());

            // HidService Was being used to gather info for a python script...
            // ...but the coffee machine api didn't seem to like linux much.
            //_hidService.DisplayHidDevices();
            //_hidService.Start();

            WatchForOrders();
        }

        private static void WatchForOrders()
        {
            List<Order> orders = new List<Order>();
            
            Console.WriteLine("Watching for updates on Order Service");
            OrderService orderService = new OrderService();

            CoffeeMakerService coffeeMakerService = new CoffeeMakerService(true);

            do
            {
                try
                {
                    Console.Write(".");

                    orders = orderService.CheckForOrders();
                    orderService.MarkOrdersAsPickedUp(orders);
                    Coffee orderScannedAtMachine = orderService.CheckForBarcodeScan(orders);
                    if (orderScannedAtMachine != null)
                    {                        
                        coffeeMakerService.MakeCoffee(orderScannedAtMachine);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in 'WatchForOrders' {ex.Message}");
                }                
            } while (1 == 1);

        }

        /// <summary>
        /// Mug courtesy of Felix Lee @ http://www.ascii-art.de/ascii/c/coffee.txt
        /// </summary>
        /// <returns></returns>
        static string GetMug()
        {
            return @"
         {
      {   }
       }_{ __{
    .-{   }   }-.
   (   }     {   )
   |`-.._____..-'|
   |             ;--.
   |            (__  \
   |             | )  )
   |             |/  /
   |             /  /    -NaviCoffee
   |            (  /
   \             y'
    `-.._____..-'
";
        }
    }
}
