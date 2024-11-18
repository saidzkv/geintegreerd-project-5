namespace Inventory_Management_System.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        public string Naam { get; set; } = "";

        public string EAN { get; set; } = "";

        public int Stock { get; set; }

        public ICollection<Stock> Stocks { get; } = new List<Stock>();
    }
}
