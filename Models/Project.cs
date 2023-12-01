namespace Kanban_RMR.Models;

public class Project
{
    public int Id { get; set; }
    public required int Customer { get; set; } // Foreign key
    public required string Name { get; set; }
    public string? Description { get; set; }
    // Add other properties as needed
}