namespace Kanban_RMR.Models;

public class Ticket
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Status { get; set; } // Foreign key
    public int Priority { get; set; } // Foreign key    
    public int CreatedBy { get; set; }  // Foreign key
    public DateTime CreatedOn { get; set; }
    // Add other properties as needed
}