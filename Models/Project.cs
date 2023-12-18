namespace Kanban_RMR.Models;

public class Project
{
    public int Id { get; set; }
    public required int CustomerId { get; set; } // Foreign key
    public required string Name { get; set; }
    public string? Description { get; set; }
    public bool Deleted { get; set; } = false;
    // Add other properties as needed
}