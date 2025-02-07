using Dapper;
using Microsoft.Data.SqlClient;
using SalesManagement.API.Interfaces;
using SalesManagement.API.Models;

namespace SalesManagement.API.Services
{
    public class SalesmanService : ISalesmanService
    {
        private readonly string _connectionString;

        public SalesmanService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Salesman>> GetAllSalesmenAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<Salesman>("SELECT * FROM Salesmen");
        }

        public async Task<Salesman> GetSalesmanByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QuerySingleOrDefaultAsync<Salesman>("SELECT * FROM Salesmen WHERE SalesmanId = @Id", new { Id = id });
        }

        public async Task<Sale> AddSaleAsync(Sale sale)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = @"
                INSERT INTO Sales (SalesmanId, ModelId, SaleDate, Quantity)
                OUTPUT INSERTED.SaleId
                VALUES (@SalesmanId, @ModelId, @SaleDate, @Quantity)";

            var saleId = await connection.ExecuteScalarAsync<int>(query, sale);
            sale.SaleId = saleId;
            return sale;
        }

        public async Task<Salesman> CreateSalesmanAsync(Salesman salesman, string username, string password)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
                var userId = await connection.ExecuteScalarAsync<int>(
                    "INSERT INTO Users (Username, PasswordHash, Role) VALUES (@Username, @PasswordHash, 'Salesman'); SELECT SCOPE_IDENTITY();",
                    new { Username = username, PasswordHash = passwordHash },
                    transaction);

                var query = @"
                    INSERT INTO Salesmen (UserId, Name, LastYearSales)
                    OUTPUT INSERTED.SalesmanId
                    VALUES (@UserId, @Name, @LastYearSales)";

                var salesmanId = await connection.ExecuteScalarAsync<int>(query,
                    new { UserId = userId, salesman.Name, salesman.LastYearSales },
                    transaction);

                transaction.Commit();

                salesman.SalesmanId = salesmanId;
                salesman.UserId = userId;
                return salesman;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<Salesman> UpdateSalesmanAsync(int id, Salesman salesman)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = @"
                UPDATE Salesmen
                SET Name = @Name, LastYearSales = @LastYearSales
                WHERE SalesmanId = @SalesmanId";

            await connection.ExecuteAsync(query, new
            {
                SalesmanId = id,
                salesman.Name,
                salesman.LastYearSales
            });

            return await GetSalesmanByIdAsync(id);
        }

        public async Task<bool> DeleteSalesmanAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = "DELETE FROM Salesmen WHERE SalesmanId = @Id";
            var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
            return affectedRows > 0;
        }
    }
}
