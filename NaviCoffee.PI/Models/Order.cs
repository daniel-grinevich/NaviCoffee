using System;
using System.Collections.Generic;

namespace NaviCoffee.PI.Models
{
    public class Order
    {
        public enum OrderStatus { None, SentToWebServer, PickedUpByArduino, RequestedAtMachine, SyrupRequested, SyrupComplete, Complete };

        public Order()
        {
        }

        public string OrderId { get; set; }

        public List<Coffee> Coffees { get; set; }

        public int CustomerId { get; set; }

        public DateTime OrderDate { get; set; }

        public OrderStatus Status { get; set; }

        public override string ToString()
        {
            return $"OrderID: {OrderId} OrderStatus: {Status}";
        }
    }
}
