namespace SalesManagement.API.Models
{
    public class SalesByBrandReport
    {
        public string BrandName { get; set; }
        public int TotalSales { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
