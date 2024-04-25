using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodShareNet.Domain.Entities;
using FoodShareNet.Repository.Data;
using FoodShareNetAPI.DTO.Donation;
using FoodShareNetAPI.DTO.Order;

namespace FoodShareNetAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class DonationController : ControllerBase
{
    private readonly FoodShareNetDbContext _context;

    public DonationController(FoodShareNetDbContext context)
    {
        _context = context;
    }

    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public async Task<IActionResult> CreateDonation([FromBody] CreateDonationDTO createDonationDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == createDonationDTO.ProductId);

        var donor = await _context.Donors
            .FirstOrDefaultAsync(d => d.Id == createDonationDTO.DonorId);

        var donationStatus = await _context.DonationStatuses
            .FirstOrDefaultAsync(ds => ds.Id == createDonationDTO.StatusId);

        if (product == null)
        {
            return BadRequest($"Product with ID {createDonationDTO.ProductId} not found.");
        }

        if (donor == null)
        {
            return BadRequest($"Donor with ID {createDonationDTO.DonorId} not found.");
        }

        if (donationStatus == null)
        {
            return BadRequest($"Status with ID {createDonationDTO.StatusId} is invalid.");
        }

        var donation = new Donation
        {
            DonorId = createDonationDTO.DonorId,
            ProductId = createDonationDTO.ProductId,
            Quantity = createDonationDTO.Quantity,
            ExpirationDate = createDonationDTO.ExpirationDate,
            StatusId = createDonationDTO.StatusId
        };

        _context.Donations.Add(donation);
        await _context.SaveChangesAsync();

        var donationDetails = new DonationDetailDTO
        {
            DonorId = createDonationDTO.DonorId,
            Product = product.Name,
            Quantity = createDonationDTO.Quantity,
            ExpirationDate = createDonationDTO.ExpirationDate,
            StatusId = createDonationDTO.StatusId,
            Status = donationStatus.Name
        };

        return Ok(donationDetails);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<ActionResult<DonationDetailDTO>> GetDonation(int id)
    {
        var donation = await _context.Donations
            .Include(d => d.Donor)
            .Include(d => d.Product)
            .Include(d => d.Status)
            .Where(d => d.Id == id)
            .Select(d => new DonationDetailDTO
            {
                Id = d.Id,
                DonorId = d.DonorId,
                Product = d.Product.Name,
                Quantity = d.Quantity,
                ExpirationDate = d.ExpirationDate,
                StatusId= d.StatusId,
                Status = d.Status.Name
            }).FirstOrDefaultAsync();

        if (donation == null)
        {
            return NotFound();
        }

        return Ok(donation);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet()]
    public async Task<ActionResult<IList<DonationDetailDTO>>> GetDonationsByCityId(int cityId)
    {
        var donations = await _context.Donations
            .Include(d => d.Donor)
            .Include(d => d.Product)
            .Include(d => d.Status)
            .Where(d => d.Donor.CityId == cityId)
            .Select(d => new DonationDetailDTO
            {
                Id = d.Id,
                DonorId = d.DonorId,
                Product = d.Product.Name,
                Quantity = d.Quantity,
                ExpirationDate = d.ExpirationDate,
                StatusId = d.StatusId,
                Status = d.Status.Name
            }).ToListAsync();

        if (!donations.Any())
        {
            return NotFound();
        }

        return Ok(donations);
    }
}
