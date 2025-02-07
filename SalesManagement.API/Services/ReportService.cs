using Dapper;
using Microsoft.Data.SqlClient;
using SalesManagement.API.Interfaces;
using SalesManagement.API.Models;

namespace SalesManagement.API.Services
{
    public class ReportService : IReportService
    {
        private readonly string _connectionString;

        public ReportService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<SalesmanCommissionReport>> GenerateSalesmanCommissionReportAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            var query = @"
                SELECT 
                    s.SalesmanId,
                    sm.Name AS SalesmanName,
                    sm.LastYearSales,
                    b.BrandName,
                    c.ClassName,
                    COUNT(*) AS NumberOfCarsSold,
                    SUM(cm.Price) AS TotalSalesAmount
                FROM 
                    Sales s
                    JOIN Salesmen sm ON s.SalesmanId = sm.SalesmanId
                    JOIN CarModels cm ON s.ModelId = cm.ModelId
                    JOIN Brands b ON cm.BrandId = b.BrandId
                    JOIN Classes c ON cm.ClassId = c.ClassId
                GROUP BY 
                    s.SalesmanId, sm.Name, sm.LastYearSales, b.BrandName, c.ClassName";

            var salesData = await connection.QueryAsync(query);

            var report = new List<SalesmanCommissionReport>();

            foreach (var salesman in salesData.GroupBy(s => s.SalesmanId))
            {
                var salesmanReport = new SalesmanCommissionReport
                {
                    SalesmanId = salesman.Key,
                    SalesmanName = salesman.First().SalesmanName,
                    LastYearSales = salesman.First().LastYearSales,
                    TotalCommission = 0,
                    CommissionByBrand = new Dictionary<string, decimal>(),
                    SalesByClass = new Dictionary<string, int>()
                };

                foreach (var sale in salesman)
                {
                    var commission = CalculateCommission(sale.BrandName, sale.ClassName, sale.TotalSalesAmount, sale.NumberOfCarsSold);
                    salesmanReport.TotalCommission += commission;

                    if (!salesmanReport.CommissionByBrand.ContainsKey(sale.BrandName))
                        salesmanReport.CommissionByBrand[sale.BrandName] = 0;
                    salesmanReport.CommissionByBrand[sale.BrandName] += commission;

                    if (!salesmanReport.SalesByClass.ContainsKey(sale.ClassName))
                        salesmanReport.SalesByClass[sale.ClassName] = 0;
                    salesmanReport.SalesByClass[sale.ClassName] += sale.NumberOfCarsSold;

                    if (sale.ClassName == "A-Class" && sale.LastYearSales > 500000)
                    {
                        var additionalCommission = sale.TotalSalesAmount * 0.02m;
                        salesmanReport.TotalCommission += additionalCommission;
                        salesmanReport.CommissionByBrand[sale.BrandName] += additionalCommission;
                    }
                }

                report.Add(salesmanReport);
            }

            return report;
        }

        public async Task<IEnumerable<SalesByBrandReport>> GenerateSalesByBrandReportAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            var query = @"
                SELECT 
                    b.BrandName,
                    COUNT(*) AS TotalSales,
                    SUM(cm.Price) AS TotalRevenue
                FROM 
                    Sales s
                    JOIN CarModels cm ON s.ModelId = cm.ModelId
                    JOIN Brands b ON cm.BrandId = b.BrandId
                GROUP BY 
                    b.BrandName";

            return await connection.QueryAsync<SalesByBrandReport>(query);
        }

        public async Task<IEnumerable<SalesByClassReport>> GenerateSalesByClassReportAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            var query = @"
                SELECT 
                    c.ClassName,
                    COUNT(*) AS TotalSales,
                    SUM(cm.Price) AS TotalRevenue
                FROM 
                    Sales s
                    JOIN CarModels cm ON s.ModelId = cm.ModelId
                    JOIN Classes c ON cm.ClassId = c.ClassId
                GROUP BY 
                    c.ClassName";

            return await connection.QueryAsync<SalesByClassReport>(query);
        }

        public async Task<IEnumerable<TopSellingModelReport>> GenerateTopSellingModelsReportAsync(int top)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = @"
                SELECT TOP (@Top)
                    cm.ModelName,
                    b.BrandName,
                    c.ClassName,
                    COUNT(*) AS TotalSales,
                    SUM(cm.Price) AS TotalRevenue
                FROM 
                    Sales s
                    JOIN CarModels cm ON s.ModelId = cm.ModelId
                    JOIN Brands b ON cm.BrandId = b.BrandId
                    JOIN Classes c ON cm.ClassId = c.ClassId
                GROUP BY 
                    cm.ModelName, b.BrandName, c.ClassName
                ORDER BY 
                    TotalSales DESC";

            return await connection.QueryAsync<TopSellingModelReport>(query, new { Top = top });
        }

        private decimal CalculateCommission(string brand, string carClass, decimal totalSalesAmount, int numberOfCarsSold)
        {
            var fixedCommission = GetFixedCommission(brand, totalSalesAmount / numberOfCarsSold);
            var percentageCommission = GetPercentageCommission(brand, carClass);

            return (fixedCommission * numberOfCarsSold) + (totalSalesAmount * percentageCommission);
        }

        private decimal GetFixedCommission(string brand, decimal carPrice)
        {
            return brand switch
            {
                "Audi" when carPrice > 25000 => 800,
                "Jaguar" when carPrice > 35000 => 750,
                "Land Rover" when carPrice > 30000 => 850,
                "Renault" when carPrice > 20000 => 400,
                _ => 0
            };
        }

        private decimal GetPercentageCommission(string brand, string carClass)
        {
            return (brand, carClass) switch
            {
                ("Audi", "A-Class") => 0.08m,
                ("Audi", "B-Class") => 0.06m,
                ("Audi", "C-Class") => 0.04m,
                ("Jaguar", "A-Class") => 0.06m,
                ("Jaguar", "B-Class") => 0.05m,
                ("Jaguar", "C-Class") => 0.03m,
                ("Land Rover", "A-Class") => 0.07m,
                ("Land Rover", "B-Class") => 0.05m,
                ("Land Rover", "C-Class") => 0.04m,
                ("Renault", "A-Class") => 0.05m,
                ("Renault", "B-Class") => 0.03m,
                ("Renault", "C-Class") => 0.02m,
                _ => 0
            };
        }
    }
}
