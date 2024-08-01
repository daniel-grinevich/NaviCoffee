using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NaviCoffee.WebApi.Models;
using NaviCoffee.WebApi.Services;
using Newtonsoft.Json;

namespace NaviCoffee.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoffeeController : ControllerBase
    {

        private readonly ILogger<CoffeeController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly IOrderService _orderService;


        public CoffeeController(ILogger<CoffeeController> logger, IWebHostEnvironment env, IOrderService orderService)
        {
            _logger = logger;
            _env = env;
            _orderService = orderService;
        }

        [HttpGet("{id}")]
        public ActionResult<Order> Get(string id)
        {
            int orderId;
            if (! int.TryParse(id, out orderId))
            {
                return NotFound();
            }

            var order = _orderService.Get(orderId);

            if (order == null)
                return NotFound();

            return order;
        }

        [HttpGet("Status/{status}")]        
        public IEnumerable<Order> Get(Order.OrderStatus status)
        {
            var orders = _orderService.Get(status);

            return orders;
        }

        [HttpGet]
        [ActionName("Get")]
        public IEnumerable<Order> Get()
        {
            return _orderService.Get();
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody]Order order)
        {
            order.Status = Order.OrderStatus.SentToWebServer;
            await _orderService.AddAsync(order);

            return Ok();            
        }

        [HttpPut]
        public Order Update([FromBody]Order order)
        {            
            order = _orderService.Update(order);

            return order;
        }

        [HttpPut("Status/{status}")]
        public Order UpdateStatus(Order.OrderStatus status)
        {
            Order order = null;

            if (status == Order.OrderStatus.SyrupComplete)
            {
                var orders = _orderService.Get(Order.OrderStatus.SyrupRequested);

                order = orders.FirstOrDefault();

                if (order != null)
                {
                    order.Status = Order.OrderStatus.SyrupComplete;
                    _orderService.Update(order);
                }
            }
            return order;
        }
    }
}
