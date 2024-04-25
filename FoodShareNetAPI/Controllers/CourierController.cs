using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodShareNet.Domain.Entities;
using FoodShareNet.Repository.Data;
using FoodShareNetAPI.DTO.Beneficiary;
using FoodShareNetAPI.DTO.Order;
using OrderStatusEnum = FoodShareNet.Domain.Enums.OrderStatus;

[Route("api/[controller]")]
[ApiController]
public class CourierController : ControllerBase
{
    private readonly FoodShareNetDbContext _context;

    public CourierController(FoodShareNetDbContext context)
    {
        _context = context;
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<ActionResult<IList<CourierDTO>>> GetAllAsync()
    {
        var couriers = await _context.Couriers
            .Select(c => new CourierDTO
            {
                Id = c.Id,
                Name = c.Name,
                Price = c.Price,
            }).ToListAsync();

        return Ok(couriers);
    }

}
