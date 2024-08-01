using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NaviCoffee.WebApi.Models;

namespace NaviCoffee.WebApi.Services
{
    //TODO: No db.
    public class DbOrderService : IOrderService
    {
        public Task<bool> AddAsync(Order order)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Order Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Order> Get()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Order> Get(Order.OrderStatus status)
        {
            throw new NotImplementedException();
        }

        public Order Update(Order order)
        {
            throw new NotImplementedException();
        }
    }
}
