using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodShareNet.Domain.Entities;
using FoodShareNet.Repository.Data;
using FoodShareNetAPI.DTO.Beneficiary;
using FoodShareNetAPI.DTO.Order;
using OrderStatusEnum = FoodShareNet.Domain.Enums.OrderStatus;
using FoodShareNet.Application.Interfaces;
using FoodShareNet.Application.Exceptions;

[Route("api/[controller]")]
[ApiController]
public class CourierController : ControllerBase
{
    private readonly ICourierService _courierService;

    public CourierController(ICourierService courierService)
    {
        _courierService = courierService;
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<ActionResult<IList<CourierDTO>>> GetAllCouriers()
    {
        try
        {
            var couriers = await _courierService.GetAllAsync();

            var couriersDTO = couriers.Select(courier => new CourierDTO
            {
                Id = courier.Id,
                Name = courier.Name,
                Price = courier.Price,
            }).ToList();

            return Ok(couriersDTO);

        } catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
