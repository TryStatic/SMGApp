namespace SMGApp.Domain.Models
{
    internal class ServiceItem
    {
        public int ID { get; set; }
        public Customer Customer { get; set; }
        public string DamageDescription { get; set; }
        public ServiceState State { get; set; }
    }

    internal enum ServiceState
    {
        Stored,
        Fixed,
        Delivered
    }
}
