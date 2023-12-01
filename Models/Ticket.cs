namespace Kanban_RMR.Models;

public class Ticket
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public int Type { get; set; } // Foreign key  // can be bug, feature, improvement, ...
    public int Project { get; set; } // Foreign key
    public int Status { get; set; } // Foreign key
    public int Priority { get; set; } // Foreign key    
    public int Customer { get; set; } // Foreign key
    public string CreatedBy { get; set; }  // Foreign key
    public DateTime CreatedOn { get; set; }
    public int? Effort { get; set; }
    public DateTime? StartDate { get; set; }
    // Add other properties as needed
}