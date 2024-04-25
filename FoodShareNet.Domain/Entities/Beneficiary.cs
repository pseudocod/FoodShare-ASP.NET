namespace FoodShareNet.Domain.Entities;

public class Beneficiary
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int CityId { get; set; }
    public City City { get; set; }
    public string Address { get; set; }
    public int Capacity { get; set; }
}