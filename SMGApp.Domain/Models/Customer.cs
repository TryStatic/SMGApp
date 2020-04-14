using System;

namespace SMGApp.Domain.Models
{
    internal class Customer
    {
        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateAdded { get; set; }
    }
}