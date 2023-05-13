using Microsoft.EntityFrameworkCore;
using Ordering.Microservice.Entities;
using System.Threading.Tasks;

namespace Ordering.Microservice.Data
{
    public interface IApplicationDbContext
    {
        DbSet<Order> Orders { get; set; }

        DbSet<OrderDetail> OrderDetails { get; set; }

        DbSet<CartItem> ShoppingCartItems { get; set; }

        Task<int> SaveChangesAsync();
    }
}
