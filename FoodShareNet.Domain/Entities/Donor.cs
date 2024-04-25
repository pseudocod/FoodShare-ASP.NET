namespace FoodShareNet.Domain.Entities;

public class Donor
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int CityId { get; set; }
    public City City { get; set; }
    public string Address { get; set; }
    public List<Donation> Donations { get; set; } = new List<Donation>();
}