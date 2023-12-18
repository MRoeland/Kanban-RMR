using Microsoft.AspNetCore.Identity;

namespace Kanban_RMR.Models
{
    public class KanbanUser: IdentityUser
    {
        public string Name { get; set; }
        public int CustomerId { get; set; }
        public bool deleted { get; set; } = false;

        public int Points { get; set; } = 0;
        public int Penalties { get; set; } = 0;
    }
}
