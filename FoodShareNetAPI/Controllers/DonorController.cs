using FoodShareNet.Domain.Entities;
using FoodShareNet.Repository.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodShareNetAPI.DTO.Donor;
using FoodShareNetAPI.DTO.Donation; // Ensure you have the corresponding DTO namespace

namespace FoodShareNetAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class DonorController : ControllerBase
{
    private readonly FoodShareNetDbContext _context;

    public DonorController(FoodShareNetDbContext context)
    {
        _context = context;
    }

    [ProducesResponseType(type: typeof(List<DonorDTO>) ,StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<ActionResult<IList<DonorDTO>>> GetAllAsync()
    {
        var donors = await _context.Donors
            .Include(d => d.City)
            .Include(d => d.Donations)
            .Select(d => new DonorDTO
            {
                Id = d.Id,
                Name = d.Name,
                CityName = d.City.Name,
                Address = d.Address,
                Donations = d.Donations.Select(
                    don => new DonationDTO
                {
                    Id = don.Id,
                    Product = don.Product.Name,
                    Quantity = don.Quantity,
                    ExpirationDate = don.ExpirationDate,
                    Status = don.Status.Name
                }).ToList()
            }).ToListAsync();

        return Ok(donors);
    }

    [ProducesResponseType(type: typeof(List<DonorDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet()]
    public async Task<ActionResult<DonorDTO>> GetAsync(int id) 
    {
        var donor = await _context.Donors
            .Include(d => d.City)
            .Include(d => d.Donations)
            .Where(d => d.Id == id)
            .Select(d => new DonorDTO
            {
                Id = d.Id,
                Name = d.Name,
                CityName = d.City.Name,
                Address = d.Address,
                Donations = d.Donations.Select(
                    don => new DonationDTO
                    {
                        Id = don.Id,
                        Product = don.Product.Name,
                        Quantity = don.Quantity,
                        ExpirationDate = don.ExpirationDate,
                        Status = don.Status.Name
                    }).ToList()
            }).FirstOrDefaultAsync();

        if(donor == null)
        {
            return NotFound();
        }

        return Ok(donor);
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

        var city = await _context.Cities
           .FirstOrDefaultAsync(d => d.Id == createDonorDTO.CityId);

        if (city == null)
        {
            return NotFound($"City with ID {createDonorDTO.CityId} not found.");
        }

        var donor = new Donor
        {
            Name = createDonorDTO.Name,
            CityId = createDonorDTO.CityId,
            Address = createDonorDTO.Address
        };

        _context.Donors.Add(donor);
        await _context.SaveChangesAsync();

        var donorDetails = new DonorDetailDTO
        {
            Id = donor.Id,
            Name = donor.Name,
            CityId = donor.CityId,
            Address = donor.Address
        };

        return Ok(donorDetails);
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

        var donor = await _context.Donors.FirstOrDefaultAsync(ed => ed.Id == editDonorDTO.Id);

        if (donor == null)
        {
            return NotFound();
        }

        var city = await _context.Cities
           .FirstOrDefaultAsync(d => d.Id == editDonorDTO.CityId);

        if (city == null)
        {
            return NotFound($"City with ID {editDonorDTO.CityId} not found.");
        }

        donor.Name = editDonorDTO.Name;
        donor.CityId = editDonorDTO.CityId;
        donor.Address = editDonorDTO.Address;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpDelete()]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var donor  = await _context.Donors.FindAsync(id);

        if (donor == null)
        {
            return NotFound();
        }

        _context.Donors.Remove(donor);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
