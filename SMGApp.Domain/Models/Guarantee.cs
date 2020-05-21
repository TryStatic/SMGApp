using System;
using System.Diagnostics.CodeAnalysis;

namespace SMGApp.Domain.Models
{
    public class Guarantee : DomainObject
    {
        public Customer Customer { get; set; }
        public string ProductDesc { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public GuaranteeType GuaranteeType { get; set; }
    }

    public enum GuaranteeType
    {
        Cellphone,
        Desktop,
        Laptop,
        Other
    }
}