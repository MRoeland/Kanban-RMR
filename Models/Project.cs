namespace Kanban_RMR.Models;

public class Project
{
    public int Id { get; set; }
    public required int CustomerId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public bool Deleted { get; set; } = false;

    public virtual Customer? Customer { get; set; }
    // Add other properties as needed
}