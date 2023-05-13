using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ordering.Microservice.Data;
using Ordering.Microservice.Entities;
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
        public OrderController(IApplicationDbContext context, IBus orderBus)
        {
            _context = context;
            _orderBus = orderBus;
        }

        [HttpGet("{userName}", Name = "GetOrder")]
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

            await _orderBus.Publish<IOrderCreated>(new
            {
                OrderId = order.Id,
                Username = order.Username
            });

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
