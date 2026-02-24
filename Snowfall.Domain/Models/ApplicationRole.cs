namespace Snowfall.Domain.Models;

public class ApplicationRole
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? NormalizedName { get; set; }

}