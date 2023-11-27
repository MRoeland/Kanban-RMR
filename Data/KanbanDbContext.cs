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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("KanbanDB");
            optionsBuilder.UseSqlServer(connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ticket>().HasData(
                new Ticket { Id = 1, Title = "Ticket 1", Description = "Ticket Description 1", Priority = 1,
                    Status = 1, CreatedBy = 1, CreatedOn = DateTime.Now },
                new Ticket { Id = 2, Title = "Ticket 2", Description = "Ticket Description 1", Priority = 2,
                    Status = 1, CreatedBy = 2, CreatedOn = DateTime.Now }
                // Add more tickets as needed
            );
        }        
    }
}