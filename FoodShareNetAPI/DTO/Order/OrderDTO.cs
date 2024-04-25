namespace FoodShareNetAPI.DTO.Order;

public class OrderDTO
{
    public int Id { get; set; }
    public int BeneficiaryId { get; set; }
    public string BeneficiaryName { get; set; } 
    public int DonationId { get; set; }
    public string DonationProduct { get; set; } 
    public int CourierId { get; set; }
    public string CourierName { get; set; } 
    public DateTime CreationDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public int OrderStatusId { get; set; }
    public string OrderStatusName { get; set; } 
}