using System.ComponentModel.DataAnnotations.Schema;

namespace Kanban_RMR.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Description { get; set; }
        [ForeignKey("Ticket")]
        public int TicketId { get; set; }
        [ForeignKey("KanbanUser")]
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public bool Deleted {  get; set; } = false;

        public virtual KanbanUser? KanbanUser { get; set; }
    }
}
