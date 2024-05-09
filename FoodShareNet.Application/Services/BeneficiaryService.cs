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
    public class BeneficiaryService : IBeneficiaryService
    {

        private readonly IFoodShareDbContext _context;

        public BeneficiaryService(IFoodShareDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<Beneficiary> CreateAsync(Beneficiary beneficiary)
        {
            var city = await _context.Cities
              .FirstOrDefaultAsync(d => d.Id == beneficiary.CityId);

            if (city == null)
            {
                throw new NotFoundException($"City with ID {beneficiary.CityId} not found.");
            }
            
            _context.Beneficiaries.Add(beneficiary);
            await _context.SaveChangesAsync();

            return beneficiary;
        }

    public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> EditAsync(int id, Beneficiary beneficiary)
        {
            if (id != beneficiary.Id)
            {
                throw new BeneficiaryException("Mismatched Beneficiary ID");
            }

            var beneficiaryDB = await _context.Beneficiaries
                .FirstOrDefaultAsync(b => b.Id == beneficiary.Id);

            if (beneficiaryDB == null)
            {
                throw new NotFoundException($"Beneficiary with ID {beneficiary.CityId} not found.");
            }
            var city = await _context.Cities
              .FirstOrDefaultAsync(d => d.Id == beneficiary.CityId);

            if (city == null)
            {
                throw new NotFoundException($"City with ID {beneficiary.CityId} not found.");
            }

            beneficiaryDB.Name = beneficiary.Name;
            beneficiaryDB.Address = beneficiary.Address;
            beneficiaryDB.CityId = beneficiary.CityId;
            beneficiaryDB.Capacity = beneficiary.Capacity;


            await _context.SaveChangesAsync();

            return true;
        }

    public Task<List<Beneficiary>> GetAllAsync()
        {
            var beneficiaries = _context.Beneficiaries
                                            .Include(b => b.City)
                                            .ToListAsync();

            if (beneficiaries == null)
            {
                throw new NotFoundException($"Could not retrieve beneficiaries.");
            }

            return beneficiaries;
        }

        public async Task<Beneficiary> GetAsync(int? id)
        {
            var beneficiary = await _context.Beneficiaries
                                                .Include(b => b.City)
                                                .FirstOrDefaultAsync(m => m.Id == id);

            if (beneficiary == null)
            {
                throw new NotFoundException($"Could not retrieve beneficiary with id {id}.");
            }

            return beneficiary;
        }
    }
}
