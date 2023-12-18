namespace Kanban_RMR.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int TicketId { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool Deleted {  get; set; } = false;


    }
}
