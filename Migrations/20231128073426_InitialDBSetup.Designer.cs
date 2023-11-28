﻿// <auto-generated />
using System;
using Kanban_RMR.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace KanbanRMR.Migrations
{
    [DbContext(typeof(KanbanDbContext))]
    [Migration("20231128073426_InitialDBSetup")]
    partial class InitialDBSetup
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Kanban_RMR.Models.Ticket", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Priority")
                        .HasColumnType("int");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Tickets");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedBy = 1,
                            CreatedOn = new DateTime(2023, 11, 28, 8, 34, 26, 686, DateTimeKind.Local).AddTicks(7010),
                            Description = "Ticket Description1",
                            Priority = 1,
                            Status = 1,
                            Title = "Ticket1"
                        },
                        new
                        {
                            Id = 2,
                            CreatedBy = 2,
                            CreatedOn = new DateTime(2023, 11, 28, 8, 34, 26, 686, DateTimeKind.Local).AddTicks(7078),
                            Description = "Ticket Description2",
                            Priority = 2,
                            Status = 1,
                            Title = "Ticket2"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}