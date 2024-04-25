namespace FoodShareNetAPI.DTO.Beneficiary;

public class CreateBeneficiaryDTO
{
    public string Name { get; set; }
    public int CityId { get; set; }
    public string Address { get; set; }
    public int Capacity { get; set; }

}