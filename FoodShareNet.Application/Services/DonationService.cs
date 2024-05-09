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
    public class DonationService : IDonationService
    {
        private readonly IFoodShareDbContext _context;

        public DonationService(IFoodShareDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<Donation> CreateDonation(Donation donation)
        { 
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == donation.ProductId);

            var donor = await _context.Donors
                .FirstOrDefaultAsync(d => d.Id == donation.DonorId);

            var donationStatus = await _context.DonationStatuses
                .FirstOrDefaultAsync(ds => ds.Id == donation.StatusId);

            if (product == null)
            {
                throw new DonationException($"Product with ID {donation.ProductId} not found.");
            }

            if (donor == null)
            {
                throw new DonationException($"Donor with ID {donation.DonorId} not found.");
            }

            if (donationStatus == null)
            {
                throw new DonationException($"Status with ID {donation.StatusId} is invalid.");
            }
            
            _context.Donations.Add(donation);
            await _context.SaveChangesAsync();

            return donation;
        }

        public async Task<Donation> GetDonation(int id)
        {
            var donation = await _context.Donations
                        .Include(d => d.Donor)
                        .Include(d => d.Product)
                        .Include(d => d.Status)
                        .Where(d => d.Id == id)
                        .FirstOrDefaultAsync();

            if (donation == null)
            {
                throw new NotFoundException($"Could not retrieve donation with id {id}.");
            }

            return donation;
        }

        public async Task<List<Donation>> GetDonationsByCityId(int cityId)
        {
            var donations = await _context.Donations
                        .Include(d => d.Donor)
                        .Include(d => d.Product)
                        .Include(d => d.Status)
                        .Where(d => d.Donor.CityId == cityId)
                        .ToListAsync();

            if (donations.Count == 0)
            {
                throw new NotFoundException($"Could not find donation by city id: {cityId}.");
            }

            return donations;
        }
    }
}
