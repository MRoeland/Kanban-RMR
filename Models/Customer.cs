namespace Kanban_RMR.Models;

public class Customer
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    // Add other properties as needed
}