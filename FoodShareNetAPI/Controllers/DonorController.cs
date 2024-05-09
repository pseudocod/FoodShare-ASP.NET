using FoodShareNet.Domain.Entities;
using FoodShareNet.Repository.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodShareNetAPI.DTO.Donor;
using FoodShareNetAPI.DTO.Donation;
using FoodShareNet.Application.Interfaces;
using FoodShareNet.Application.Exceptions; // Ensure you have the corresponding DTO namespace

namespace FoodShareNetAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class DonorController : ControllerBase
{
    private readonly IDonorService _donorService;

    public DonorController(IDonorService donorService)
    {
        _donorService = donorService;
    }

    [ProducesResponseType(type: typeof(List<DonorDTO>) ,StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<ActionResult<IList<DonorDTO>>> GetAllAsync()
    {
        try
        {
            var donors = await _donorService.GetAllAsync();

            var donorsDTO = donors.Select(d => new DonorDTO
            {
                Id = d.Id,
                Name = d.Name,
                CityName = d.City?.Name,
                Address = d.Address,
                Donations = d.Donations?.Select(don => new DonationDTO
                {
                    Id = don.Id,
                    Product = don.Product?.Name, 
                    Quantity = don.Quantity,
                    ExpirationDate = don.ExpirationDate,
                    Status = don.Status?.Name 
                }).ToList() ?? new List<DonationDTO>() 
            }).ToList();

            return Ok(donorsDTO);

        } catch(NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [ProducesResponseType(type: typeof(List<DonorDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet()]
    public async Task<ActionResult<DonorDTO>> GetAsync(int id)
    {
        try
        {
            var donor = await _donorService.GetAsync(id);

            var donorDTO = new DonorDTO
            {
                Id = donor.Id,
                Name = donor.Name,
                CityName = donor.City.Name,
                Address = donor.Address,
                Donations = donor.Donations.Select(
                    don => new DonationDTO
                    {
                        Id = don.Id,
                        Product = don.Product.Name,
                        Quantity = don.Quantity,
                        ExpirationDate = don.ExpirationDate,
                        Status = don.Status.Name
                    }).ToList()
            };

            return Ok(donorDTO);

        } catch(NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public async Task<ActionResult<DonorDetailDTO>> CreateAsync([FromBody] CreateDonorDTO createDonorDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var donor = new Donor
            {
                Name = createDonorDTO.Name,
                CityId = createDonorDTO.CityId,
                Address = createDonorDTO.Address
            };

            var createDonor = await _donorService.CreateAsync(donor);

            var donorDetails = new DonorDetailDTO
            {
                Id = createDonor.Id,
                Name = createDonor.Name,
                CityId = createDonor.CityId,
                Address = createDonor.Address
            };

            return Ok(donorDetails);
        } catch(DonorException ex)
        {
            return BadRequest(ex.Message);
        }   
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPut()]
    public async Task<IActionResult> EditAsync([FromBody] EditDonorDTO editDonorDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var donor = new Donor
        {
            Id = editDonorDTO.Id,
            Name = editDonorDTO.Name,
            CityId = editDonorDTO.CityId,
            Address = editDonorDTO.Address
        };

        try
        {
            await _donorService.EditAsync(donor);

            return NoContent();
        } catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        } catch(DonorException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpDelete()]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        try
        {
            await _donorService.DeleteAsync(id);

            return NoContent();
        } catch(NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
