namespace RabbitMQContract
{
    public interface IOrderCreated
    {
        public int OrderId { get; set; }
        public string UserEmail { get; set; }
    }
}
