using Kanban_RMR.Models;
using Microsoft.EntityFrameworkCore;

namespace Kanban_RMR.Data
{
    public class KanbanDbContext : DbContext
    {
    public DbSet<Ticket> Tickets { get; set; }      
    }
}