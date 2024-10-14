namespace Inventory_Management_System.Models
{
    public class Stock
    {
        public int StockId { get; set; }

        public int Hoeveelheid { get; set; }

        public int? ProductId { get; set; }

        public virtual Product? Product { get; set; }

        public DateTime Datum { get; set; }

        public Stock()
        {
            Datum = DateTime.Today;
        }
    }
}
