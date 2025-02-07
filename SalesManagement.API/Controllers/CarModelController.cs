using Microsoft.AspNetCore.Mvc;
using SalesManagement.API.Interfaces;
using SalesManagement.API.Models;

namespace SalesManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarModelController : ControllerBase
    {
        private readonly ICarModelService _carModelService;

        public CarModelController(ICarModelService carModelService)
        {
            _carModelService = carModelService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarModel>>> GetAllModels()
        {
            var models = await _carModelService.GetAllModelsAsync();
            return Ok(models);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CarModel>> GetModelById(int id)
        {
            var model = await _carModelService.GetModelByIdAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            return Ok(model);
        }

        [HttpPost]
        public async Task<ActionResult<CarModel>> CreateModel([FromBody] CarModel model)
        {
            var createdModel = await _carModelService.CreateModelAsync(model);
            return CreatedAtAction(nameof(GetModelById), new { id = createdModel.ModelId }, createdModel);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CarModel>> UpdateModel(int id, [FromBody] CarModel model)
        {
            var updatedModel = await _carModelService.UpdateModelAsync(id, model);
            if (updatedModel == null)
            {
                return NotFound();
            }
            return Ok(updatedModel);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteModel(int id)
        {
            var result = await _carModelService.DeleteModelAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<CarModel>>> SearchModels([FromQuery] string searchTerm)
        {
            var models = await _carModelService.SearchModelsAsync(searchTerm);
            return Ok(models);
        }
    }
}
