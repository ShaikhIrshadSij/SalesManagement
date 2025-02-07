using SalesManagement.API.Models;

namespace SalesManagement.API.Interfaces
{
    public interface IReportService
    {
        Task<IEnumerable<SalesmanCommissionReport>> GenerateSalesmanCommissionReportAsync();
        Task<IEnumerable<SalesByBrandReport>> GenerateSalesByBrandReportAsync();
        Task<IEnumerable<SalesByClassReport>> GenerateSalesByClassReportAsync();
        Task<IEnumerable<TopSellingModelReport>> GenerateTopSellingModelsReportAsync(int top);
    }
}
