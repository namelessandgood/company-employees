namespace Shared.DataTransferObjects;

public record CompanyForUpdateDto
{
    public CompanyForUpdateDto(string Name,
        string Address,
        string Country,
        IEnumerable<EmployeeForCreationDto> Employees)
    {
        this.Name = Name;
        this.Address = Address;
        this.Country = Country;
        this.Employees = Employees;
    }

    public string Name { get; init; } = null!;
    public string Address { get; init; } = null!;
    public string Country { get; init; } = null!;
    public IEnumerable<EmployeeForCreationDto> Employees { get; init; } = null!;

    public CompanyForUpdateDto() { }
}