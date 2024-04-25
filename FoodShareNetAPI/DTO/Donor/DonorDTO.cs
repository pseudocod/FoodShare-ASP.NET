using FoodShareNetAPI.DTO.Donation;

namespace FoodShareNetAPI.DTO.Donor;

public class DonorDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string CityName { get; set; }
    public string Address { get; set; }
    public List<DonationDTO> Donations { get; set; } = new List<DonationDTO>();
}