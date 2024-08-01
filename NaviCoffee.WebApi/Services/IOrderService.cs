using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NaviCoffee.WebApi.Models;

namespace NaviCoffee.WebApi.Services
{
    public interface IOrderService
    {
        Task<bool> AddAsync(Order order);
        Order Update(Order order);
        Task<bool> DeleteAsync(int id);
        Order Get(int id);
        IEnumerable<Order> Get();
        IEnumerable<Order> Get(Order.OrderStatus status);
    }
}
