namespace SalesManagement.API.Models
{
    public class Salesman
    {
        public int SalesmanId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public decimal LastYearSales { get; set; }
    }
}
