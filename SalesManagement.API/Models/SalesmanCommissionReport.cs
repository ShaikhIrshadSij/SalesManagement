namespace SalesManagement.API.Models
{
    public class SalesmanCommissionReport
    {
        public int SalesmanId { get; set; }
        public string SalesmanName { get; set; }
        public decimal LastYearSales { get; set; }
        public decimal TotalCommission { get; set; }
        public Dictionary<string, decimal> CommissionByBrand { get; set; }
        public Dictionary<string, int> SalesByClass { get; set; }
    }
}
