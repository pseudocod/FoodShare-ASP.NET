﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodShareNet.Domain.Entities;
using FoodShareNet.Repository.Data;
using FoodShareNetAPI.DTO.Order;
using OrderStatusEnum = FoodShareNet.Domain.Enums.OrderStatus;
using FoodShareNet.Application.Interfaces;
using FoodShareNet.Application.Exceptions;
using FoodShareNet.Domain.Enums;

[Route("api/[controller]/[action]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<ActionResult<OrderDTO>> CreateOrder([FromBody] CreateOrderDTO createOrderDTO)
    {
        var order = new Order
        {
            Quantity = createOrderDTO.Quantity,
            BeneficiaryId = createOrderDTO.BeneficiaryId,
            DonationId = createOrderDTO.DonationId,
            CourierId = createOrderDTO.CourierId,
            CreationDate = createOrderDTO.CreationDate,
            OrderStatusId = createOrderDTO.OrderStatusId,
        };

        try
        {
            var createOrder = await _orderService.CreateOrderAsync(order);

            var orderDetails = new OrderDetailsDTO
            {
                Id = createOrder.Id,
                BeneficiaryId = createOrder.BeneficiaryId,
                Quantity = createOrder.Quantity,
                DonationId = createOrder.DonationId,
                CourierId = createOrder.CourierId,
                CreationDate = createOrder.CreationDate,
                DeliveryDate = createOrder.DeliveryDate,
                OrderStatusId = createOrder.OrderStatusId,
            };

            return Ok(orderDetails);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (OrderException ex) {
            return BadRequest(ex.Message);
        }
    }

    // GET: api/Order/5
    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDTO>> GetOrder(int id)
    {
        try
        {
            var order = await _orderService.GetOrderAsync(id);

            var orderDTO = new OrderDTO
            {
                Id = order.Id,
                BeneficiaryId = order.BeneficiaryId,
                BeneficiaryName = order.Beneficiary.Name,
                DonationId = order.DonationId,
                DonationProduct = order.Donation.Product.Name,
                CourierId = order.CourierId,
                CourierName = order.Courier.Name,
                CreationDate = order.CreationDate,
                DeliveryDate = order.DeliveryDate,
                OrderStatusId = order.OrderStatusId,
                OrderStatusName = order.OrderStatus.Name
            };

            return Ok(orderDTO);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }


    [HttpPatch("{orderId:int}/status")]
    public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] UpdateOrderStatusDTO updateStatusDTO)
    {
        try
        {
            await _orderService.UpdateOrderStatusAsync(orderId, (OrderStatusEnum)updateStatusDTO.NewStatusId);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}






// POST: api/Order
//[HttpPost]
//public async Task<ActionResult<OrderDTO>> CreateOrder([FromBody] CreateOrderDTO createOrderDTO)
//{
//    var donation = await _context.Donations
//        .FirstOrDefaultAsync(d => d.Id == createOrderDTO.DonationId);

//    // Check if the donation exists
//    if (donation == null)
//    {
//        return NotFound($"Donation with ID {createOrderDTO.DonationId} not found.");
//    }

//    // Check if the requested quantity is available
//    if (donation.Quantity < createOrderDTO.Quantity)
//    {
//        return BadRequest($"Requested quantity exceeds available quantity for Donation ID {createOrderDTO.DonationId}.");
//    }

//    donation.Quantity -= createOrderDTO.Quantity;

//    var order = new Order
//    {
//        BeneficiaryId = createOrderDTO.BeneficiaryId,
//        DonationId = createOrderDTO.DonationId,
//        CourierId = createOrderDTO.CourierId,
//        CreationDate = createOrderDTO.CreationDate,
//        OrderStatusId = createOrderDTO.OrderStatusId,
//    };

//    _context.Orders.Add(order);
//    await _context.SaveChangesAsync();

//    var orderDetails = new OrderDetailsDTO
//    {
//        Id = order.Id,
//        BeneficiaryId = order.BeneficiaryId,
//        DonationId = order.DonationId,
//        CourierId = order.CourierId,
//        CreationDate = order.CreationDate,
//        DeliveryDate = order.DeliveryDate,
//        OrderStatusId = order.OrderStatusId,
//    };

//    return Ok(orderDetails);
//}

//// GET: api/Order/5
//[HttpGet("{id}")]
//public async Task<ActionResult<OrderDTO>> GetOrder(int id)
//{
//    var order = await _context.Orders
//        .Include(o => o.Beneficiary)
//        .Include(o => o.Donation)
//        .Include(o => o.OrderStatus)
//        .Where(o => o.Id == id)
//        .Select(o => new OrderDTO
//        {
//            Id = o.Id,
//            BeneficiaryId = o.BeneficiaryId,
//            BeneficiaryName = o.Beneficiary.Name,
//            DonationId = o.DonationId,
//            DonationProduct = o.Donation.Product.Name,
//            CourierId = o.CourierId,
//            CourierName = o.Courier.Name,
//            CreationDate = o.CreationDate,
//            DeliveryDate = o.DeliveryDate,
//            OrderStatusId = o.OrderStatusId,
//            OrderStatusName = o.OrderStatus.Name
//        })
//        .FirstOrDefaultAsync();

//    if (order == null)
//    {
//        return NotFound();
//    }

//    return Ok(order);
//}

//[HttpPatch("{orderId:int}/status")]
//public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] UpdateOrderStatusDTO updateStatusDTO)
//{
//    if (orderId != updateStatusDTO.OrderId)
//    {
//        return BadRequest("Mismatched Order ID");
//    }

//    var order = await _context.Orders.FindAsync(orderId);
//    if (order == null)
//    {
//        return NotFound($"Order with ID {orderId} not found.");
//    }

//    if (!_context.OrderStatuses.Any(s => s.Id == updateStatusDTO.NewStatusId))
//    {
//        return NotFound($"Status with ID {updateStatusDTO.NewStatusId} not found.");
//    }

//    order.OrderStatusId = updateStatusDTO.NewStatusId;
//    order.DeliveryDate = updateStatusDTO.NewStatusId == (int)OrderStatusEnum.Delivered ? DateTime.UtcNow : order.DeliveryDate;

//    await _context.SaveChangesAsync();

//    return NoContent();
//}






























//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using FoodShareNet.Domain.Entities;
//using FoodShareNet.Repository.Data;
//using FoodShareNetAPI.DTO.Order;
//using OrderStatusEnum = FoodShareNet.Domain.Enums.OrderStatus;

//[Route("api/[controller]/[action]")]
//[ApiController]
//public class OrderController : ControllerBase
//{
//    private readonly FoodShareNetDbContext _context;

//    public OrderController(FoodShareNetDbContext context)
//    {
//        _context = context;
//    }

//    // POST: api/Order
//    [HttpPost]
//    public async Task<ActionResult<OrderDTO>> CreateOrder([FromBody] CreateOrderDTO createOrderDTO)
//    {
//        var donation = await _context.Donations
//            .FirstOrDefaultAsync(d => d.Id == createOrderDTO.DonationId);

//        // Check if the donation exists
//        if (donation == null)
//        {
//            return NotFound($"Donation with ID {createOrderDTO.DonationId} not found.");
//        }

//        // Check if the requested quantity is available
//        if (donation.Quantity < createOrderDTO.Quantity)
//        {
//            return BadRequest($"Requested quantity exceeds available quantity for Donation ID {createOrderDTO.DonationId}.");
//        }

//        donation.Quantity -= createOrderDTO.Quantity;

//        var order = new Order
//        {
//            BeneficiaryId = createOrderDTO.BeneficiaryId,
//            DonationId = createOrderDTO.DonationId,
//            CourierId = createOrderDTO.CourierId,
//            CreationDate = createOrderDTO.CreationDate,
//            OrderStatusId = createOrderDTO.OrderStatusId,
//        };

//        _context.Orders.Add(order);
//        await _context.SaveChangesAsync();

//        var orderDetails = new OrderDetailsDTO
//        {
//            Id = order.Id,
//            BeneficiaryId = order.BeneficiaryId,
//            DonationId = order.DonationId,
//            CourierId = order.CourierId,
//            CreationDate = order.CreationDate,
//            DeliveryDate = order.DeliveryDate,
//            OrderStatusId = order.OrderStatusId,
//        };

//        return Ok(orderDetails);
//    }

//    // GET: api/Order/5
//    [HttpGet("{id}")]
//    public async Task<ActionResult<OrderDTO>> GetOrder(int id)
//    {
//        var order = await _context.Orders
//            .Include(o => o.Beneficiary)
//            .Include(o => o.Donation)
//            .Include(o => o.OrderStatus)
//            .Where(o => o.Id == id)
//            .Select(o => new OrderDTO
//            {
//                Id = o.Id,
//                BeneficiaryId = o.BeneficiaryId,
//                BeneficiaryName = o.Beneficiary.Name,
//                DonationId = o.DonationId,
//                DonationProduct = o.Donation.Product.Name,
//                CourierId = o.CourierId,
//                CourierName = o.Courier.Name,
//                CreationDate = o.CreationDate,
//                DeliveryDate = o.DeliveryDate,
//                OrderStatusId = o.OrderStatusId,
//                OrderStatusName = o.OrderStatus.Name
//            })
//            .FirstOrDefaultAsync();

//        if (order == null)
//        {
//            return NotFound();
//        }

//        return Ok(order);
//    }

//    [HttpPatch("{orderId:int}/status")]
//    public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] UpdateOrderStatusDTO updateStatusDTO)
//    {
//        if (orderId != updateStatusDTO.OrderId)
//        {
//            return BadRequest("Mismatched Order ID");
//        }

//        var order = await _context.Orders.FindAsync(orderId);
//        if (order == null)
//        {
//            return NotFound($"Order with ID {orderId} not found.");
//        }

//        if (!_context.OrderStatuses.Any(s => s.Id == updateStatusDTO.NewStatusId))
//        {
//            return NotFound($"Status with ID {updateStatusDTO.NewStatusId} not found.");
//        }

//        order.OrderStatusId = updateStatusDTO.NewStatusId;
//        order.DeliveryDate = updateStatusDTO.NewStatusId == (int)OrderStatusEnum.Delivered ? DateTime.UtcNow : order.DeliveryDate;

//        await _context.SaveChangesAsync();

//        return NoContent();
//    }
//}
