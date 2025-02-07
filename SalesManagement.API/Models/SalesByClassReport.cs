namespace SalesManagement.API.Models
{
    public class SalesByClassReport
    {
        public string ClassName { get; set; }
        public int TotalSales { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
