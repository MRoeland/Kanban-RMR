namespace Kanban_RMR.Models;

public class Priority
{
    public int Id { get; set; }
    public required string Description { get; set; }
    // Add other properties as needed
    public required string Color { get; set; }
}