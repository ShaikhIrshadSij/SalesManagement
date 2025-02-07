namespace SalesManagement.API.Models
{
    public class CreateSalesmanModel
    {
        public string Name { get; set; }
        public decimal LastYearSales { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
