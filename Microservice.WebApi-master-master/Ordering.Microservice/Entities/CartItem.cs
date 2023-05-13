using System;
using System.ComponentModel.DataAnnotations;

namespace Ordering.Microservice.Entities
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        public int CustomerId { get; set; }
        public int Quantity { get; set; }

        public DateTime DateCreated { get; set; }

        public int ProductId { get; set; }

    }
}
