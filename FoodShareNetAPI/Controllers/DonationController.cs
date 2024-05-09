using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodShareNet.Domain.Entities;
using FoodShareNet.Repository.Data;
using FoodShareNetAPI.DTO.Donation;
using FoodShareNetAPI.DTO.Order;
using FoodShareNet.Application.Interfaces;
using FoodShareNet.Domain.Enums;
using FoodShareNet.Application.Exceptions;

namespace FoodShareNetAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class DonationController : ControllerBase
{
    private readonly IDonationService _donationService;

    public DonationController(IDonationService donationService)
    {
        _donationService = donationService;
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

        var donation = new Donation
        {
            DonorId = createDonationDTO.DonorId,
            ProductId = createDonationDTO.ProductId,
            Quantity = createDonationDTO.Quantity,
            ExpirationDate = createDonationDTO.ExpirationDate,
            StatusId = createDonationDTO.StatusId
        };

        try
        {
            var createDonation = await _donationService.CreateDonation(donation);

            var donationDetails = new DonationDetailDTO
            {
                DonorId = createDonation.DonorId,
                Product = createDonation.Product.Name,
                Quantity = createDonation.Quantity,
                ExpirationDate = createDonation.ExpirationDate,
                StatusId = createDonation.StatusId,
                Status = createDonation.Status.Name,
            };
            return Ok(donationDetails);
        } catch (DonationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<ActionResult<DonationDetailDTO>> GetDonation(int id)
    {
        try
        {
            var donation = await _donationService.GetDonation(id);

            var donationDetailDTO = new DonationDetailDTO
            {
                Id = donation.Id,
                DonorId = donation.DonorId,
                Product = donation.Product.Name,
                Quantity = donation.Quantity,
                ExpirationDate = donation.ExpirationDate,
                StatusId = donation.StatusId,
                Status = donation.Status.Name
            };

            return Ok(donationDetailDTO);

        }catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet()]
    public async Task<ActionResult<IList<DonationDetailDTO>>> GetDonationsByCityId(int cityId)
    {
        try
        {
            var donations = await _donationService.GetDonationsByCityId(cityId);
            
            var donationsDetailDTO = donations.Select(d => new DonationDetailDTO
            {
                Id = d.Id,
                DonorId = d.DonorId,
                Product = d.Product.Name,
                Quantity = d.Quantity,
                ExpirationDate = d.ExpirationDate,
                StatusId = d.StatusId,
                Status = d.Status.Name
            }).ToList();

            return Ok(donationsDetailDTO);
        } catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }   
    }
}



