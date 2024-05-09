using FoodShareNet.Domain.Entities;
using FoodShareNet.Repository.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodShareNetAPI.DTO.Beneficiary;
using FoodShareNetAPI.DTO.Donor;

namespace FoodShareNetAPI.Controllers;

using FoodShareNet.Application.Exceptions;
using FoodShareNet.Application.Interfaces;
using System.Net;

[ApiController]
[Route("api/[controller]/[action]")]
public class BeneficiaryController : ControllerBase
{
    private readonly IBeneficiaryService _beneficiaryService;

    public BeneficiaryController(IBeneficiaryService beneficiaryService)
    {
        _beneficiaryService = beneficiaryService;
    }

    [HttpGet]
    public async Task<ActionResult<IList<BeneficiaryDTO>>> GetAll()
    {

        try
        {
            var beneficiaries = await _beneficiaryService.GetAllAsync();

            var beneficiariesDTO = beneficiaries.Select(beneficiary => new BeneficiaryDTO
            {
                Id = beneficiary.Id,
                Name = beneficiary.Name,
                Address = beneficiary.Address,
                CityName = beneficiary.City.Name
            }).ToList();

            return Ok(beneficiariesDTO);

        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<BeneficiaryDTO>> GetBeneficiary(int? id)
    {
        try
        {
            var beneficiary = await _beneficiaryService.GetAsync(id);

            var beneficiaryDTO = new BeneficiaryDTO
            {
                Id = beneficiary.Id,
                Name = beneficiary.Name,
                Address = beneficiary.Address,
                CityName = beneficiary.City.Name,
            };

            return Ok(beneficiaryDTO);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<BeneficiaryDetailDTO>>
        Create(CreateBeneficiaryDTO createBeneficiaryDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var beneficiary = new Beneficiary
        {
            Name = createBeneficiaryDTO.Name,
            Address = createBeneficiaryDTO.Address,
            CityId = createBeneficiaryDTO.CityId,
            Capacity = createBeneficiaryDTO.Capacity
        };

        try
        {
            var createBeneficiary = await _beneficiaryService.CreateAsync(beneficiary);

            var beneficiaryEntityDTO = new BeneficiaryDetailDTO
            {
                Id = createBeneficiary.Id,
                Name = createBeneficiary.Name,
                Address = createBeneficiary.Address,
                CityId = createBeneficiary.CityId,
                Capacity = createBeneficiary.Capacity
            };
            
            return Ok(beneficiaryEntityDTO);

        } catch(NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult>
        Edit(int id, EditBeneficiaryDTO editBeneficiaryDTO)
    {
        var beneficiary = new Beneficiary
        {
            Id = editBeneficiaryDTO.Id,
            Name = editBeneficiaryDTO.Name,
            Address = editBeneficiaryDTO.Address,
            CityId = editBeneficiaryDTO.CityId,
            Capacity = editBeneficiaryDTO.Capacity,
        };

        try
        {
            await _beneficiaryService.EditAsync(id, beneficiary);
            
            return NoContent();
        }
        catch (NotFoundException ex) {
            return NotFound(ex.Message);
        } catch (BeneficiaryException ex) {
            return BadRequest(ex.Message);
        }
    }
}






//[HttpPut]
//public async Task<IActionResult>
//    EditAsync(int id, EditBeneficiaryDTO editBeneficiaryDTO)
//{
//    if (id != editBeneficiaryDTO.Id)
//    {
//        return BadRequest("Mismatched Beneficiary ID");
//    }

//    var beneficiary = await _context.Beneficiaries
//        .FirstOrDefaultAsync(b => b.Id == editBeneficiaryDTO.Id);

//    if (beneficiary == null)
//    {
//        return NotFound();
//    }
//    var city = await _context.Cities
//      .FirstOrDefaultAsync(d => d.Id == editBeneficiaryDTO.CityId);

//    if (city == null)
//    {
//        return NotFound($"City with ID {editBeneficiaryDTO.CityId} not found.");
//    }

//    beneficiary.Name = editBeneficiaryDTO.Name;
//    beneficiary.Address = editBeneficiaryDTO.Address;
//    beneficiary.CityId = editBeneficiaryDTO.CityId;
//    beneficiary.Capacity = editBeneficiaryDTO.Capacity;


//    await _context.SaveChangesAsync();

//    return NoContent();
//}

//[HttpDelete]
//public async Task<IActionResult> DeleteAsync(int id)
//{
//    var beneficiary = await _context.Beneficiaries.FindAsync(id);

//    if (beneficiary == null)
//    {
//        return NotFound();
//    }

//    _context.Beneficiaries.Remove(beneficiary);
//    await _context.SaveChangesAsync();
//    return NoContent();
//}
