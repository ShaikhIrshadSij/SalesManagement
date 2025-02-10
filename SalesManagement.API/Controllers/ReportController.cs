using Microsoft.AspNetCore.Mvc;
using SalesManagement.API.Interfaces;
using SalesManagement.API.Models;

namespace SalesManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = "Admin")]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("salesman-commission")]
        public async Task<ActionResult<IEnumerable<SalesmanCommissionReport>>> GetSalesmanCommissionReport()
        {
            var report = await _reportService.GenerateSalesmanCommissionReportAsync();
            return Ok(report);
        }

        [HttpGet("sales-by-brand")]
        public async Task<ActionResult<IEnumerable<SalesByBrandReport>>> GetSalesByBrandReport()
        {
            var report = await _reportService.GenerateSalesByBrandReportAsync();
            return Ok(report);
        }

        [HttpGet("sales-by-class")]
        public async Task<ActionResult<IEnumerable<SalesByClassReport>>> GetSalesByClassReport()
        {
            var report = await _reportService.GenerateSalesByClassReportAsync();
            return Ok(report);
        }

        [HttpGet("top-selling-models")]
        public async Task<ActionResult<IEnumerable<TopSellingModelReport>>> GetTopSellingModelsReport([FromQuery] int top = 10)
        {
            var report = await _reportService.GenerateTopSellingModelsReportAsync(top);
            return Ok(report);
        }
    }
}
