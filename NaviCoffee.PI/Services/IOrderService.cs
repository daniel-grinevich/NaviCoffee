using System.Collections.Generic;
using NaviCoffee.PI.Models;
using static NaviCoffee.PI.Models.Order;

namespace NaviCoffee.PI
{
    public interface IOrderService
    {
        public List<Order> CheckForOrders();

        public void MarkOrdersAsPickedUp(List<Order> orders);

        public bool MarkOrder(Order order, OrderStatus orderStatus);

        public void AddBarcodeScan(string barcode);

        Coffee CheckForBarcodeScan(List<Order> orders);
    }
}
