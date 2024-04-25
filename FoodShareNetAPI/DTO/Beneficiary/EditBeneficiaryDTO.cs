namespace FoodShareNetAPI.DTO.Beneficiary;

public class EditBeneficiaryDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int CityId { get; set; }
    public string Address { get; set; }
    public int Capacity { get; set; }

}