namespace Kanban_RMR.Models;

public class Reward
{
    public int Id { get; set; }
    public required string Action { get; set; }
    public required int Points { get; set; }
    public required bool Enabled { get; set; }
    public bool Deleted { get; set; } = false;

    // Add other properties as needed
}