namespace FoodShareNet.Domain.Enums;

public enum OrderStatus : int
{
    Unconfirmed = 1,
    Confirmed = 2,
    InDelivery = 3,
    Delivered = 4
}
