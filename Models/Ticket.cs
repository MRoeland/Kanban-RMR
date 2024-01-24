using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kanban_RMR.Models;

public class Ticket
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    [ForeignKey("TicketType")]
    public int TypeId { get; set; } // Foreign key  // can be bug, feature, improvement, ...
    [ForeignKey("Project")]
    public int ProjectId { get; set; } // Foreign key

    [Display(Name = "Current Status")]
    [ForeignKey("Status")]
    public int StatusId { get; set; } // Foreign key
    [ForeignKey("Priority")]
    public int PriorityId { get; set; } // Foreign key    
    [ForeignKey("Customer")]
    public int CustomerId { get; set; } // Foreign key
    [ForeignKey("KanbanUser")]
    public string CreatedBy { get; set; }  // Foreign key
    public DateTime CreatedOn { get; set; }
    public int? Effort { get; set; }
    public DateTime? StartDate { get; set; }
    public int? Index { get; set; }
    public bool Deleted { get; set; }


    public virtual ICollection<Comment>? Comments { get; set; }

    public virtual TicketType? Type{ get; set; }
    public virtual Project? Project{ get; set; }
    public virtual Status? Status { get; set; }
    public virtual Priority? Priority { get; set; }
    public virtual Customer? Customer { get; set; }
    public virtual KanbanUser? KanbanUser{ get; set; }
    // Add other properties as needed
}