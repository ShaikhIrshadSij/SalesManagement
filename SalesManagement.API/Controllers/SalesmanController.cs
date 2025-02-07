using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesManagement.API.Interfaces;
using SalesManagement.API.Models;

namespace SalesManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SalesmanController : ControllerBase
    {
        private readonly ISalesmanService _salesmanService;

        public SalesmanController(ISalesmanService salesmanService)
        {
            _salesmanService = salesmanService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Salesman>>> GetAllSalesmen()
        {
            var salesmen = await _salesmanService.GetAllSalesmenAsync();
            return Ok(salesmen);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Salesman")]
        public async Task<ActionResult<Salesman>> GetSalesmanById(int id)
        {
            var salesman = await _salesmanService.GetSalesmanByIdAsync(id);
            if (salesman == null)
            {
                return NotFound();
            }
            return Ok(salesman);
        }

        [HttpPost("sale")]
        [Authorize(Roles = "Salesman")]
        public async Task<ActionResult<Sale>> AddSale([FromBody] Sale sale)
        {
            var addedSale = await _salesmanService.AddSaleAsync(sale);
            return CreatedAtAction(nameof(GetSalesmanById), new { id = addedSale.SalesmanId }, addedSale);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Salesman>> CreateSalesman([FromBody] CreateSalesmanModel model)
        {
            var salesman = new Salesman
            {
                Name = model.Name,
                LastYearSales = model.LastYearSales
            };

            var createdSalesman = await _salesmanService.CreateSalesmanAsync(salesman, model.Username, model.Password);
            return CreatedAtAction(nameof(GetSalesmanById), new { id = createdSalesman.SalesmanId }, createdSalesman);
        }
    }
}
