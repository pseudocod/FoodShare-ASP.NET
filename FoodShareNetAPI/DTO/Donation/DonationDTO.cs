namespace FoodShareNetAPI.DTO.Donation;

public class DonationDTO
{
    public int Id { get; set; }
    public string Product { get; set; }
    public int Quantity { get; set; }
    public DateTime ExpirationDate { get; set; }
    public string Status { get; set; }
}

