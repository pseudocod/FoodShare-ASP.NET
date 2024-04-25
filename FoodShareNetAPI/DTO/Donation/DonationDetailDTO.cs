namespace FoodShareNetAPI.DTO.Donation;

public class DonationDetailDTO
{
    public int Id { get; set; }
    public int DonorId { get; set; }
    public string Product { get; set; }
    public int Quantity { get; set; }
    public DateTime ExpirationDate { get; set; }
    public int StatusId { get; set; }
    public string Status { get; set; }
}

