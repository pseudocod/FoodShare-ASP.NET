namespace FoodShareNet.Domain.Entities;

public class OrderStatus
{
    public int Id { get; set; }
    public string Name { get; set; } // Unconfirmed, Confirmed, In Delivery, Delivered
}