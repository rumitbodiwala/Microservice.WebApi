using Microsoft.EntityFrameworkCore;
using Ordering.Microservice.Entities;
using System.Threading.Tasks;
using OM = Ordering.Microservice.Entities;

namespace Ordering.Microservice.Data
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions)
             : base(dbContextOptions)
        {
        }

        public virtual DbSet<OM.Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }

        public virtual DbSet<CartItem> ShoppingCartItems { get; set; }


        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}
