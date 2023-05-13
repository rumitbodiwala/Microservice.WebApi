using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Microservice.Data;
using Ordering.Microservice.Entities;
using RabbitMQContract;
using System;
using System.Linq;
using System.Threading.Tasks;
using VM = Ordering.Microservice.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Ordering.Microservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private IApplicationDbContext _context;
        private readonly IBus _orderBus;
        private readonly IServiceProvider _serviceProvider;
        public OrderController(IApplicationDbContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }

        [HttpGet("GetOrder")]
        public async Task<IActionResult> GetOrdersByUserNameAsync(string userName)
        {
            var order = await _context.Orders.Where(a => a.Username == userName).FirstOrDefaultAsync();
            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrderAsync(VM.Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            //var _emailBus = _serviceProvider.GetRequiredService<IEmailServiceBus>();
            //await _emailBus.Publish<IOrderCreated>(new
            //{
            //    OrderId = order.Id,
            //    Username = order.Username
            //});

            return Ok(order.Id);
        }

        [HttpDelete("DeleteOrder")]
        public async Task<IActionResult> DeleteOrderAsync(int id)
        {
            var order = await _context.Orders.Where(a => a.Id == id).FirstOrDefaultAsync();
            if (order == null) return NotFound();
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return Ok(order.Id);
        }
    }
}
