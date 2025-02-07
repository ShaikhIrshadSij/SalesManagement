using SalesManagement.API.Models;

namespace SalesManagement.API.Interfaces
{
    public interface ISalesmanService
    {
        Task<IEnumerable<Salesman>> GetAllSalesmenAsync();
        Task<Salesman> GetSalesmanByIdAsync(int id);
        Task<Sale> AddSaleAsync(Sale sale);
        Task<Salesman> CreateSalesmanAsync(Salesman salesman, string username, string password);
        Task<Salesman> UpdateSalesmanAsync(int id, Salesman salesman);
        Task<bool> DeleteSalesmanAsync(int id);
    }
}
