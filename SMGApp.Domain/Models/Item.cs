using System;

namespace SMGApp.Domain.Models
{
    public class Item
    {
        public int ID { get; set; }
        public string PartSerial { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public DateTime DateAdded { get; set; }
        public StockCategory Category { get; set; }
    }

    public enum StockCategory
    {
        Category1,
        Category2,
        Category3
    }
}