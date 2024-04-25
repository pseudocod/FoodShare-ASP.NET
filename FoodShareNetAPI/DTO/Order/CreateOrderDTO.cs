namespace FoodShareNetAPI.DTO.Order;
public class CreateOrderDTO
{
    public int BeneficiaryId { get; set; }
    public int DonationId { get; set; }
    public int CourierId { get; set; }
    public int Quantity { get; set; }
    public DateTime CreationDate { get; set; }
    public int OrderStatusId { get; set; }
}