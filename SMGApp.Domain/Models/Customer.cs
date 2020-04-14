using System;

namespace SMGApp.Domain.Models
{
    public class Customer : DomainObject
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.Now;
        public string Notes { get; set; }
    }
}