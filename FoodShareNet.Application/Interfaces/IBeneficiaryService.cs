using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShareNet.Application.Interfaces
{
    internal interface IBeneficiaryService
    {
        public async Task<ActionResult<IList<BeneficiaryDTO>>> GetAllAsync()
        public async Task<ActionResult<BeneficiaryDTO>> GetAsync(int? id)
                
            
            public async Task<ActionResult<BeneficiaryDetailDTO>> CreateAsync(CreateBeneficiaryDTO createBeneficiaryDTO)
            public async Task<IActionResult> EditAsync(int id, EditBeneficiaryDTO editBeneficiaryDTO)

           public async Task<IActionResult> DeleteAsync(int id)


    }
}
