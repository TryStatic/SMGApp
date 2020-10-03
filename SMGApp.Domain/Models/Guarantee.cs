using System;

namespace SMGApp.Domain.Models
{
    public class Guarantee : DomainObject
    {
        public int VirtualID { get; set; }
        public Customer Customer { get; set; }
        public string ProductDesc { get; set; }
        public string ProductIMEI { get; set; }
        public string ProductNotes { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public GuaranteeType GuaranteeType { get; set; }

        public string CustomerDetails => Customer != null ? $"{Customer.LastName} {Customer.FirstName} (ID: {Customer.ID})" : "";
    }

    public enum GuaranteeType
    {
        Cellphone,
        Desktop,
        Laptop,
        Other
    }
}