using Microsoft.AspNetCore.Identity;

namespace Kanban_RMR.Models
{
    public class KanbanUser: IdentityUser
    {
        public string Name { get; set; }
        public int Customer { get; set; }

    }
}
