namespace SalesManagement.API.Models
{
    public class Sale
    {
        public int SaleId { get; set; }
        public int SalesmanId { get; set; }
        public int ModelId { get; set; }
        public DateTime SaleDate { get; set; }
        public int Quantity { get; set; }
    }
}
