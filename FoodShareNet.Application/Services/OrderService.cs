using FoodShareNet.Application.Exceptions;
using FoodShareNet.Application.Interfaces;
using FoodShareNet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShareNet.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IFoodShareDbContext _context;

        public OrderService(IFoodShareDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            var donation = await _context.Donations
                .FirstOrDefaultAsync(d => d.Id == order.DonationId);

            // Check if the donation exists
            if (donation == null)
            {
                throw new NotFoundException($"Donation with ID {order.DonationId} not found.");
            }

            // Check if the requested quantity is available
            if (donation.Quantity < order.Quantity)
            {
                throw new OrderException($"Requested quantity exceeds available quantity for Donation ID {order.DonationId}.");
            }

            donation.Quantity -= order.Quantity;

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return order;
        }

        public Task<Order> GetOrderAsync(int id)
        {
         var orders = _context.Orders
                        .Include(o => o.Beneficiary)
                        .Include(o => o.Donation)
                        .Include(o => o.OrderStatus)
                        .Where(o => o.Id == id)
                        .FirstOrDefaultAsync();

            if (orders == null) {
                throw new NotFoundException($"Could not retrieve order by ID: {id}");
            }

            return orders;
        }

        public async Task<bool> updateOrderStatusAsync(int orderId, Domain.Enums.OrderStatus orderStatus)
        { 
            var order = await _context.Orders.FindAsync(orderId);

            if (order == null)
            {
                throw new NotFoundException($"Order with ID {orderId} not found.");
            }

            if (!_context.OrderStatuses.Any(s => s.Id == (int)orderStatus))
            {
                throw new NotFoundException($"Status with ID {(int)orderStatus} not found.");
            }

            order.OrderStatusId = (int)orderStatus;
            order.DeliveryDate = orderStatus == Domain.Enums.OrderStatus.Delivered ? DateTime.UtcNow : order.DeliveryDate;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
