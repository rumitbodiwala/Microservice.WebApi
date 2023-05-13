using System.ComponentModel.DataAnnotations.Schema;

namespace Ordering.Microservice.Entities
{
    public class OrderDetail
    {
        public OrderDetail()
        {
            Order = new Order();
        }

        public int Id { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }
    }
}
