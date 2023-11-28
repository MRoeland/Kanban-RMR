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
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Priority> Priorities { get; set; }

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
            modelBuilder.Entity<Status>().HasData(
                new Status { Id = 1, Description = "To Do", },
                new Status { Id = 2, Description = "Analysis", },
                new Status { Id = 3, Description = "In progress", },
                new Status { Id = 4, Description = "In review", },
                new Status { Id = 5, Description = "Done", }
                // Add more statuses as needed
            );
            modelBuilder.Entity<Priority>().HasData(
                new Priority { Id = 1, Description = "Minor", },
                new Priority { Id = 2, Description = "Major", },
                new Priority { Id = 3, Description = "Critical", },
                new Priority { Id = 4, Description = "Blocking", }
                // Add more statuses as needed
            );
        }        
    }
}