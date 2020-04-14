namespace SMGApp.Domain.Models
{
    public class ServiceItem
    {
        public int ID { get; set; }
        public Customer Customer { get; set; }
        public string DamageDescription { get; set; }
        public ServiceState State { get; set; }
    }

    public enum ServiceState
    {
        Stored,
        Fixed,
        Delivered
    }
}
