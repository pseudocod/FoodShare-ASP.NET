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
    public class DonorService : IDonorService
    {
        private readonly IFoodShareDbContext _context;

        public DonorService(IFoodShareDbContext dbContext)
        {
            _context = dbContext;
        }
        public async Task<List<Donor>> GetAllAsync()
        {
            var donors = await _context.Donors
                .Include(d => d.City)
                .Include(d => d.Donations)
                    .ThenInclude(donation => donation.Product)
                .Include(d => d.Donations)
                    .ThenInclude(donation => donation.Status)
                .ToListAsync();

            if (donors.Count == 0)
            {
                throw new NotFoundException($"Could not retrieve donors.");
            }

            return donors;
        }

        public async Task<Donor> GetAsync(int id)
        {
            var donor = await _context.Donors
                .Include(d => d.City)
                .Include(d => d.Donations)
                    .ThenInclude(donation => donation.Product)
                .Include(d => d.Donations)
                    .ThenInclude(donation => donation.Status)
                .Where(d => d.Id == id)
                .FirstOrDefaultAsync();

            if (donor == null)
            {
                throw new NotFoundException($"Could not retrieve donor with id: {id}");
            }

            return donor;
        }

        public async Task<Donor> CreateAsync(Donor donor)
        {
            var city = await _context.Cities
                       .FirstOrDefaultAsync(d => d.Id == donor.CityId);

            if (city == null)
            {
                throw new DonorException($"City with ID {donor.CityId} not found.");
            }

            _context.Donors.Add(donor);
            await _context.SaveChangesAsync();

            return donor;
        }

        public async Task<bool> EditAsync(Donor editDonor)
        {
            var donorDB = await _context.Donors.FirstOrDefaultAsync(ed => ed.Id == editDonor.Id);

            if (donorDB == null)
            {
                throw new NotFoundException($"Can't find donor with id {editDonor.Id}");
            }

            var city = await _context.Cities
               .FirstOrDefaultAsync(d => d.Id == editDonor.CityId);

            if (city == null)
            {
                throw new DonorException($"City with ID {editDonor.CityId} not found.");
            }

            donorDB.Name = editDonor.Name;
            donorDB.CityId = editDonor.CityId;
            donorDB.Address = editDonor.Address;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var donor = await _context.Donors.FindAsync(id);

            if (donor == null)
            {
                throw new NotFoundException($"Can't select donor with id {id}.");
            }

            _context.Donors.Remove(donor);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
