using SalesManagement.API.Models;

namespace SalesManagement.API.Interfaces
{
    public interface ICarModelService
    {
        Task<IEnumerable<CarModel>> GetAllModelsAsync();
        Task<CarModel> GetModelByIdAsync(int id);
        Task<CarModel> CreateModelAsync(CarModel model);
        Task<CarModel> UpdateModelAsync(int id, CarModel model);
        Task<bool> DeleteModelAsync(int id);
        Task<IEnumerable<CarModel>> SearchModelsAsync(string searchTerm);
    }
}
