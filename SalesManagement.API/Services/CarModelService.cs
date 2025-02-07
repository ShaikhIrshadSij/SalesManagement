using Dapper;
using Microsoft.Data.SqlClient;
using SalesManagement.API.Interfaces;
using SalesManagement.API.Models;

namespace SalesManagement.API.Services
{
    public class CarModelService : ICarModelService
    {
        private readonly string _connectionString;

        public CarModelService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<CarModel>> GetAllModelsAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            var query = @"
                SELECT cm.*, mi.ImagePath
                FROM CarModels cm
                LEFT JOIN ModelImages mi ON cm.ModelId = mi.ModelId
                ORDER BY cm.DateOfManufacturing DESC, cm.SortOrder";
            var modelDict = new Dictionary<int, CarModel>();

            var models = await connection.QueryAsync<CarModel, string, CarModel>(
                query,
                (model, imagePath) =>
                {
                    if (!modelDict.TryGetValue(model.ModelId, out var carModel))
                    {
                        carModel = model;
                        carModel.ImagePaths = new List<string>();
                        modelDict.Add(model.ModelId, carModel);
                    }

                    if (!string.IsNullOrEmpty(imagePath))
                    {
                        carModel.ImagePaths.Add(imagePath);
                    }

                    return carModel;
                },
                splitOn: "ImagePath"
            );

            return modelDict.Values;
        }

        public async Task<CarModel> GetModelByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = @"
                    SELECT cm.*, mi.ImagePath
                    FROM CarModels cm
                    LEFT JOIN ModelImages mi ON cm.ModelId = mi.ModelId
                    WHERE cm.ModelId = @Id";

            var modelDictionary = new Dictionary<int, CarModel>();

            var models = await connection.QueryAsync<CarModel, string, CarModel>(
                query,
                (model, imagePath) =>
                {
                    if (!modelDictionary.TryGetValue(model.ModelId, out var carModel))
                    {
                        carModel = model;
                        carModel.ImagePaths = new List<string>();
                        modelDictionary.Add(model.ModelId, carModel);
                    }
                    if (!string.IsNullOrEmpty(imagePath))
                    {
                        carModel.ImagePaths.Add(imagePath);
                    }
                    return carModel;
                },
                new { Id = id },
                splitOn: "ImagePath"
            );

            return modelDictionary.Values.FirstOrDefault(); // Safe retrieval
        }


        public async Task<CarModel> CreateModelAsync(CarModel model)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = @"
                INSERT INTO CarModels (BrandId, ClassId, ModelName, ModelCode, Description, Features, Price, DateOfManufacturing, IsActive, SortOrder)
                OUTPUT INSERTED.ModelId
                VALUES (@BrandId, @ClassId, @ModelName, @ModelCode, @Description, @Features, @Price, @DateOfManufacturing, @IsActive, @SortOrder)";

            var parameters = new
            {
                BrandId = GetBrandId(model.Brand),
                ClassId = GetClassId(model.Class),
                model.ModelName,
                model.ModelCode,
                model.Description,
                model.Features,
                model.Price,
                model.DateOfManufacturing,
                model.IsActive,
                model.SortOrder
            };

            var modelId = await connection.ExecuteScalarAsync<int>(query, parameters);

            if (model.ImagePaths != null && model.ImagePaths.Any())
            {
                var imageQuery = "INSERT INTO ModelImages (ModelId, ImagePath) VALUES (@ModelId, @ImagePath)";
                await connection.ExecuteAsync(imageQuery, model.ImagePaths.Select(path => new { ModelId = modelId, ImagePath = path }));
            }

            return await GetModelByIdAsync(modelId);
        }

        public async Task<CarModel> UpdateModelAsync(int id, CarModel model)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = @"
                UPDATE CarModels
                SET BrandId = @BrandId, ClassId = @ClassId, ModelName = @ModelName, ModelCode = @ModelCode,
                    Description = @Description, Features = @Features, Price = @Price,
                    DateOfManufacturing = @DateOfManufacturing, IsActive = @IsActive, SortOrder = @SortOrder
                WHERE ModelId = @ModelId";

            var parameters = new
            {
                ModelId = id,
                BrandId = GetBrandId(model.Brand),
                ClassId = GetClassId(model.Class),
                model.ModelName,
                model.ModelCode,
                model.Description,
                model.Features,
                model.Price,
                model.DateOfManufacturing,
                model.IsActive,
                model.SortOrder
            };

            await connection.ExecuteAsync(query, parameters);

            // Update images
            await connection.ExecuteAsync("DELETE FROM ModelImages WHERE ModelId = @ModelId", new { ModelId = id });
            if (model.ImagePaths != null && model.ImagePaths.Any())
            {
                var imageQuery = "INSERT INTO ModelImages (ModelId, ImagePath) VALUES (@ModelId, @ImagePath)";
                await connection.ExecuteAsync(imageQuery, model.ImagePaths.Select(path => new { ModelId = id, ImagePath = path }));
            }

            return await GetModelByIdAsync(id);
        }

        public async Task<bool> DeleteModelAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = "DELETE FROM CarModels WHERE ModelId = @Id";
            var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
            return affectedRows > 0;
        }

        public async Task<IEnumerable<CarModel>> SearchModelsAsync(string searchTerm)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = @"
                SELECT cm.*, mi.ImagePath
                FROM CarModels cm
                LEFT JOIN ModelImages mi ON cm.ModelId = mi.ModelId
                WHERE cm.ModelName LIKE @SearchTerm OR cm.ModelCode LIKE @SearchTerm
                ORDER BY cm.DateOfManufacturing DESC, cm.SortOrder";

            var modelDict = new Dictionary<int, CarModel>();

            var models = await connection.QueryAsync<CarModel, string, CarModel>(
                query,
                (model, imagePath) =>
                {
                    if (!modelDict.TryGetValue(model.ModelId, out var carModel))
                    {
                        carModel = model;
                        carModel.ImagePaths = new List<string>();
                        modelDict.Add(model.ModelId, carModel);
                    }

                    if (!string.IsNullOrEmpty(imagePath))
                    {
                        carModel.ImagePaths.Add(imagePath);
                    }

                    return carModel;
                },
                new { SearchTerm = $"%{searchTerm}%" },
                splitOn: "ImagePath"
            );

            return modelDict.Values;
        }

        private int GetBrandId(string brandName)
        {
            using var connection = new SqlConnection(_connectionString);
            return connection.QuerySingle<int>("SELECT BrandId FROM Brands WHERE BrandName = @BrandName", new { BrandName = brandName });
        }

        private int GetClassId(string className)
        {
            using var connection = new SqlConnection(_connectionString);
            return connection.QuerySingle<int>("SELECT ClassId FROM Classes WHERE ClassName = @ClassName", new { ClassName = className });
        }
    }
}
