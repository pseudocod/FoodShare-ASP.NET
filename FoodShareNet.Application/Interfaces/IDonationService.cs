using FoodShareNet.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShareNet.Application.Interfaces
{
    public interface IDonationService
    {
        Task<Donation> CreateDonation(Donation donation);
        Task<Donation> GetDonation(int id);
        Task<List<Donation>> GetDonationsByCityId(int cityId);
    }
}
