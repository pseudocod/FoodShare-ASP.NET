using FoodShareNet.Domain.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShareNet.Application.Interfaces
{
    public interface IBeneficiaryService
    {
        Task<List<Beneficiary>> GetAllAsync();
        Task<Beneficiary> GetAsync(int? id);
        Task<Beneficiary> CreateAsync(Beneficiary beneficiary);
        Task<bool> EditAsync(int id, Beneficiary beneficiary);
        Task<bool> DeleteAsync(int id);
    }
}
