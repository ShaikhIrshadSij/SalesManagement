namespace SalesManagement.API.Models
{
    public class TopSellingModelReport
    {
        public string ModelName { get; set; }
        public string BrandName { get; set; }
        public string ClassName { get; set; }
        public int TotalSales { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
