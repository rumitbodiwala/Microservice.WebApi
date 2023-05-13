using System;
using System.Collections.Generic;

namespace Ordering.Microservice.Entities
{
    public class Order
    {
        public Order()
        {
            OrderDetails = new List<OrderDetail>();
        }
        public int Id { get; set; }

        public DateTime OrderDate { get; set; } = new DateTime();

        public string Username { get; set; }

        //public int CustomerId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string PostalCode { get; set; }
        public string Country { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public decimal GrossValue { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalPrice { get; set; }
        public string OrderStatus { get; set; }

        public string PaymentTransactionId { get; set; }

        public bool HasBeenShipped { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }

    }
}
