namespace Shared.DataTransferObjects;

public record EmployeeForUpdateDto
{
    public EmployeeForUpdateDto(string Name, int Age, string Position)
    {
        this.Name = Name;
        this.Age = Age;
        this.Position = Position;
    }

    public string Name { get; init; } = null!;
    public int Age { get; init; }
    public string Position { get; init; } = null!;

    public EmployeeForUpdateDto() { }
}