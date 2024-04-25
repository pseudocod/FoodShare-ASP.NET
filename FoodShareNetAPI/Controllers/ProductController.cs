using Microsoft.AspNetCore.Mvc;
using FoodShareNetAPI.DTO.Product;
using FoodShareNet.Repository.Data;
using Microsoft.EntityFrameworkCore;


[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly FoodShareNetDbContext _context;
    public ProductController(FoodShareNetDbContext context)
    {
        _context = context;
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<ActionResult<IList<ProductDTO>>> GetAllAsync()
    {
        var products = await _context.Products
            .Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name
            }).ToListAsync();

        return Ok(products);
    }

}
