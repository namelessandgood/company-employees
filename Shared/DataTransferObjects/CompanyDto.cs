namespace Shared.DataTransferObjects;
// [Serializable]
public record CompanyDto
{
    public Guid Id { get; init; }
    public string? Name { get; init; }
    public string? FullAddress { get; init; }

    public CompanyDto(string name, string fullAddress)
    {
        Name = name;
        FullAddress = fullAddress;
    }
    public CompanyDto() { }

}