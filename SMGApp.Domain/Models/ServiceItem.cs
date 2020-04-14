using System;

namespace SMGApp.Domain.Models
{
    public class ServiceItem : DomainObject
    {
        public Customer Customer { get; set; }
        public string DamageDescription { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.Now;
        public DateTime DateUpdated { get; set; }
        public ServiceState State { get; set; }
    }

    public enum ServiceState
    {
        Stored,
        Fixed,
        Delivered
    }
}
