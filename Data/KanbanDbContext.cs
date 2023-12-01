using Kanban_RMR.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Emit;

namespace Kanban_RMR.Data
{
    public class KanbanDbContext : IdentityDbContext<KanbanUser>
    {
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Priority> Priorities { get; set; }
        public DbSet<TicketType> TicketTypes { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Reward> Rewards { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        public KanbanDbContext(DbContextOptions<KanbanDbContext> options) : base(options)
        {
        }

        // any unique string id
        string ADMIN_ID = Guid.NewGuid().ToString("D");
        string ADMINROLE_ID = Guid.NewGuid().ToString("D");
        string USERROLE_ID = Guid.NewGuid().ToString("D");

        string EMPL1_ID = Guid.NewGuid().ToString("D");
        string EMPL2_ID = Guid.NewGuid().ToString("D");
        string GARVIS1_ID = Guid.NewGuid().ToString("D");

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
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<KanbanUser>()
                .Property(e => e.Name)
                .HasMaxLength(250);

            this.SeedHelperTables(modelBuilder);
            this.SeedCustomers(modelBuilder);

            this.SeedRoles(modelBuilder);
            this.SeedUsers(modelBuilder);
            this.SeedUserRoles(modelBuilder);

            this.SeedTickets(modelBuilder);
        }

        private void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole
            {
                Id = ADMINROLE_ID,
                Name = "admin",
                NormalizedName = "admin"
            },
            new IdentityRole
            {
                Id = USERROLE_ID,
                Name = "user",
                NormalizedName = "user"
            });
        }
        private void SeedUsers(ModelBuilder modelBuilder)
        {
            var hasher = new PasswordHasher<KanbanUser>();
            modelBuilder.Entity<KanbanUser>().HasData(
            new KanbanUser
            {
                Id = ADMIN_ID,
                UserName = "admin",
                NormalizedUserName = "admin",
                Email = "admin@testemail.com",
                NormalizedEmail = "admin@testemail.com",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Start123#"),
                SecurityStamp = Guid.NewGuid().ToString("D"),
                PhoneNumberConfirmed = true,
                Name = "admin",
                Customer = 1
            },
            new KanbanUser
            {
                Id = EMPL1_ID,
                UserName = "empl1",
                NormalizedUserName = "empl1",
                Email = "empl1@testemail.com",
                NormalizedEmail = "empl1@testemail.com",
                EmailConfirmed = true,//false,
                PasswordHash = hasher.HashPassword(null, "Start123#"),
                SecurityStamp = Guid.NewGuid().ToString("D"),//string.Empty,
                Name = "empl1",
                Customer = 1
            },
            new KanbanUser
            {
                Id = EMPL2_ID,
                UserName = "empl2",
                NormalizedUserName = "empl2",
                Email = "empl2@testemail.com",
                NormalizedEmail = "empl2@testemail.com",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Start123#"),
                SecurityStamp = Guid.NewGuid().ToString("D"),
                Name = "empl2",
                Customer = 1
            },
            new KanbanUser
            {
                Id = GARVIS1_ID,
                UserName = "garvis1",
                NormalizedUserName = "garvis1",
                Email = "garvis1@testemail.com",
                NormalizedEmail = "garvis1@testemail.com",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Start123#"),
                SecurityStamp = Guid.NewGuid().ToString("D"),
                Name = "garvis1",
                Customer = 2
            });
        }
        private void SeedUserRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            {
                RoleId = ADMINROLE_ID,
                UserId = ADMIN_ID
            },
            new IdentityUserRole<string>
            {
                RoleId = ADMINROLE_ID,
                UserId = EMPL1_ID
            },
            new IdentityUserRole<string>
            {
                RoleId = USERROLE_ID,
                UserId = EMPL2_ID
            },
            new IdentityUserRole<string>
            {
                RoleId = USERROLE_ID,
                UserId = GARVIS1_ID
            });
        }

        private void SeedHelperTables(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TicketType>().HasData(
                new TicketType { Id = 1, Description = "Bug", },
                new TicketType { Id = 2, Description = "Feature", },
                new TicketType { Id = 3, Description = "Improvement", }
                // Add more tickettypes as needed
            );
            modelBuilder.Entity<Priority>().HasData(
                new Priority { Id = 1, Description = "Minor", Color = "#f7f7ed" },
                new Priority { Id = 2, Description = "Major", Color = "#fcfc03" },
                new Priority { Id = 3, Description = "Critical", Color = "#fcba03" },
                new Priority { Id = 4, Description = "Blocking", Color = "#fc5a03" }
                // Add more priorities as needed
            );
            modelBuilder.Entity<Status>().HasData(
                new Status { Id = 1, Description = "To Do", },
                new Status { Id = 2, Description = "Analysis", },
                new Status { Id = 3, Description = "In progress", },
                new Status { Id = 4, Description = "In review", },
                new Status { Id = 5, Description = "Done", }
                // Add more statuses as needed
            );
            modelBuilder.Entity<Reward>().HasData(
                new Reward { Id = 1, Action = "CreatedTicket", Points = 1, Enabled = true },
                new Reward { Id = 2, Action = "CreatedDuplicateTicket", Points = -1, Enabled = true },
                new Reward { Id = 3, Action = "MoveToDone", Points = 1, Enabled = true }
                // Add more rewards as needed
            );
        }
        private void SeedCustomers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().HasData(
                new Customer { Id = 1, Name = "Intern" },
                new Customer { Id = 2, Name = "Garvis" }
                // Add more customers as needed
            );
            modelBuilder.Entity<Project>().HasData(
                new Project { Id = 1, Name = "Intern1", Customer = 1 },
                new Project { Id = 2, Name = "Intern2", Customer = 1 },
                new Project { Id = 3, Name = "Project1", Customer = 2 },
                new Project { Id = 4, Name = "Project2", Customer = 2 }
                // Add more projects as needed
            );
        }
        private void SeedTickets(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ticket>().HasData(
                new Ticket
                {
                    Id = 1,
                    Title = "IntTicket1",
                    Description = "internal ticket1",
                    Type = 3,
                    Priority = 1,
                    Status = 1,
                    Project = 1,
                    Customer = 1,
                    CreatedBy = EMPL1_ID,
                    CreatedOn = DateTime.Now
                },
                new Ticket
                {
                    Id = 2,
                    Title = "IntTicket2",
                    Description = "internal ticket2",
                    Type = 2,
                    Priority = 2,
                    Status = 1,
                    Project = 1,
                    Customer = 1,
                    CreatedBy = EMPL1_ID,
                    CreatedOn = DateTime.Now
                },
                new Ticket
                {
                    Id = 3,
                    Title = "IntTicket3",
                    Description = "internal ticket3",
                    Type = 1,
                    Priority = 3,
                    Status = 1,
                    Project = 1,
                    Customer = 1,
                    CreatedBy = EMPL2_ID,
                    CreatedOn = DateTime.Now
                },
                new Ticket
                {
                    Id = 4,
                    Title = "Ticket1",
                    Description = "Ticket Description1",
                    Type = 2,
                    Priority = 1,
                    Status = 1,
                    Project = 2,
                    Customer = 2,
                    CreatedBy = GARVIS1_ID,
                    CreatedOn = DateTime.Now
                },
                new Ticket
                {
                    Id = 5,
                    Title = "Ticket2",
                    Description = "Ticket Description2",
                    Type = 1,
                    Priority = 5,
                    Status = 1,
                    Project = 2,
                    Customer = 2,
                    CreatedBy = GARVIS1_ID,
                    CreatedOn = DateTime.Now
                },
                new Ticket
                {
                    Id = 6,
                    Title = "Ticket3",
                    Description = "Ticket Description3",
                    Type = 1,
                    Priority = 2,
                    Status = 1,
                    Project = 4,
                    Customer = 2,
                    CreatedBy = GARVIS1_ID,
                    CreatedOn = DateTime.Now
                }
                // Add more tickets as needed
            );
        }
    }
}