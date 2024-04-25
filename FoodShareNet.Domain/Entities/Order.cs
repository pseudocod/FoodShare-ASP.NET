namespace FoodShareNet.Domain.Entities;

public class Order
{
    public int Id { get; set; }
    public int BeneficiaryId { get; set; }
    public Beneficiary Beneficiary { get; set; }
    public int DonationId { get; set; }
    public Donation Donation { get; set; }
    public int CourierId { get; set; }
    public Courier Courier { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime? DeliveryDate { get; set; } // Nullable in case the delivery date is not yet set
    public int OrderStatusId { get; set; }
    public OrderStatus OrderStatus { get; set; }
}
