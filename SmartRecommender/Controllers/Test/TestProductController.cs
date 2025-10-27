using Microsoft.AspNetCore.Mvc;
using SmartRecommender.Application.Abstractions.Repositories;

namespace SmartRecommender.Controllers.Test
{
    [ApiController]
    [Route("api/test/[controller]")]
    public class TestProductController : Controller
    {
        private readonly IProductRepository _productRepo;
        public TestProductController(IProductRepository productRepo)
        {
            _productRepo = productRepo;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var result = await _productRepo.GetAllAsync(ct);
            return Ok(result);
        }
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var item = await _productRepo.GetByIdAsync(id, ct);
            if (item == null)
                return NotFound($"Product with id {id} not found.");
            return Ok(item);

        }
    }
}