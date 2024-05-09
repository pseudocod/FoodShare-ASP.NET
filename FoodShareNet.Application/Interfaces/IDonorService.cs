using FoodShareNet.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShareNet.Application.Interfaces
{
    public interface IDonorService
    {
        Task<List<Donor>> GetAllAsync();
        Task<Donor> GetAsync(int id);
        Task<Donor> CreateAsync(Donor donor);
        Task<bool> EditAsync(Donor editDonor);
        Task<bool> DeleteAsync(int id);
    }
}
