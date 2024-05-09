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
    public class CourierService : ICourierService
    {
        private readonly IFoodShareDbContext _context;

        public CourierService(IFoodShareDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<List<Courier>> GetAllAsync()
        {
            var couriers = await _context.Couriers.ToListAsync();

            if(couriers == null)
            {
                throw new DirectoryNotFoundException($"Could not retrieve couriers.");
            }

            return couriers;
        }
    }
}
