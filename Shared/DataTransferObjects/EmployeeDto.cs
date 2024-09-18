namespace Shared.DataTransferObjects;

public record EmployeeDto
{
    public EmployeeDto(Guid Id, string Name, int Age, string Position)
    {
        this.Id = Id;
        this.Name = Name;
        this.Age = Age;
        this.Position = Position;
    }

    public Guid Id { get; init; }
    public string? Name { get; init; }
    public int Age { get; init; }
    public string? Position { get; init; }

    public EmployeeDto() { }
}