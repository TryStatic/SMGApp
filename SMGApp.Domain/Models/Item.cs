namespace SMGApp.Domain.Models
{
    internal class Item
    {
        public int ID { get; set; }
        public string PartSerial { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public StockCategory Category { get; set; }
    }

    internal enum StockCategory
    {
        Category1,
        Category2,
        Category3
    }
}