using Kanban_RMR.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Kanban_RMR.Data
{
    public class KanbanDbContext : DbContext
    {
        public DbSet<Ticket> Tickets { get; set; }

        public KanbanDbContext(DbContextOptions<KanbanDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())//.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ticket>().HasData(
                new Ticket { Id = 1, Title = "Ticket1", Description = "Ticket Description1", Priority = 1,
                    Status = 1, CreatedBy = 1, CreatedOn = DateTime.Now },
                new Ticket { Id = 2, Title = "Ticket2", Description = "Ticket Description2", Priority = 2,
                    Status = 1, CreatedBy = 2, CreatedOn = DateTime.Now }
                // Add more tickets as needed
            );
        }        
    }
}