using Microsoft.AspNetCore.Mvc;
using FoodShareNetAPI.DTO.Product;
using FoodShareNet.Repository.Data;
using Microsoft.EntityFrameworkCore;
using FoodShareNet.Application.Interfaces;
using FoodShareNet.Application.Exceptions;


[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<ActionResult<IList<ProductDTO>>> GetAllProducts()
    {
        try
        {
            var products = await _productService.GetAllAsync();

            var productsDTO = products.Select(product => new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
            }).ToList();

            return Ok(productsDTO);

        } catch(NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

}
