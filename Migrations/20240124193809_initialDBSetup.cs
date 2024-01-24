using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KanbanRMR.Migrations
{
    /// <inheritdoc />
    public partial class initialDBSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Priority",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Priority", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reward",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reward", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TicketType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    deleted = table.Column<bool>(type: "bit", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false),
                    Penalties = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Project_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ticket",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    PriorityId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Effort = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Index = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ticket", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ticket_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ticket_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ticket_Priority_PriorityId",
                        column: x => x.PriorityId,
                        principalTable: "Priority",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ticket_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ticket_Status_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ticket_TicketType_TypeId",
                        column: x => x.TypeId,
                        principalTable: "TicketType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TicketId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Likes = table.Column<int>(type: "int", nullable: false),
                    Dislikes = table.Column<int>(type: "int", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comment_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comment_Ticket_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Ticket",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "07cd1428-b34e-45cc-8501-874e1d10255c", null, "employee", "employee" },
                    { "082665b9-de26-40b0-a88b-cb46564edbf6", null, "user", "user" },
                    { "cc5edb92-cf28-4862-a4f1-1a39f1b27761", null, "admin", "admin" }
                });

            migrationBuilder.InsertData(
                table: "Customer",
                columns: new[] { "Id", "Deleted", "Description", "Name" },
                values: new object[,]
                {
                    { 1, false, null, "Intern" },
                    { 2, false, null, "Garvis" }
                });

            migrationBuilder.InsertData(
                table: "Priority",
                columns: new[] { "Id", "Color", "Deleted", "Description" },
                values: new object[,]
                {
                    { 1, "#f7f7ed", false, "Minor" },
                    { 2, "#fcfc03", false, "Major" },
                    { 3, "#fcba03", false, "Critical" },
                    { 4, "#fc5a03", false, "Blocking" }
                });

            migrationBuilder.InsertData(
                table: "Reward",
                columns: new[] { "Id", "Action", "Deleted", "Enabled", "Points" },
                values: new object[,]
                {
                    { 1, "CreatedTicket", false, true, 1 },
                    { 2, "CreatedDuplicateTicket", false, true, -1 },
                    { 3, "MoveToDone", false, true, 1 },
                    { 4, "AddedComment", false, true, 1 },
                    { 5, "RemovedComment", false, true, -1 },
                    { 6, "LikedComment", false, true, 1 },
                    { 7, "DislikedComment", false, true, -1 }
                });

            migrationBuilder.InsertData(
                table: "Status",
                columns: new[] { "Id", "Deleted", "Description" },
                values: new object[,]
                {
                    { 1, false, "To Do" },
                    { 2, false, "Analysis" },
                    { 3, false, "In progress" },
                    { 4, false, "In review" },
                    { 5, false, "Done" }
                });

            migrationBuilder.InsertData(
                table: "TicketType",
                columns: new[] { "Id", "Deleted", "Description" },
                values: new object[,]
                {
                    { 1, false, "Bug" },
                    { 2, false, "Feature" },
                    { 3, false, "Improvement" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CustomerId", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "Penalties", "PhoneNumber", "PhoneNumberConfirmed", "Points", "SecurityStamp", "TwoFactorEnabled", "UserName", "deleted" },
                values: new object[,]
                {
                    { "b79d6b4c-8ff6-42c6-ad76-38247bfc447b", 0, "fcfb0597-0e86-4a09-810f-cf566cce8052", 1, "empl1@testemail.com", true, false, null, "empl1", "empl1@testemail.com", "empl1", "AQAAAAIAAYagAAAAEHMbDAZNVEw9GfzycLyw7xrQAw1i00ktK7dMuSfJ4Tza6TZI40IZREFj9HFrhJqjOQ==", 0, null, false, 2, "488af5c2-2e50-4aad-abed-9161d2b520a9", false, "empl1", false },
                    { "d19c9551-5000-48df-ad83-9e4ea8cba3e8", 0, "d59b72ab-cfeb-4c06-8f9e-3453253d0eb8", 1, "empl2@testemail.com", true, false, null, "empl2", "empl2@testemail.com", "empl2", "AQAAAAIAAYagAAAAEK2FE/q5KJOwNBvjSTSorQH9jorrgyuiLWmTIOeJGvMrlVcp/w85Q7qsI1gnN+WAOw==", 0, null, false, 1, "8a1df346-f03d-4397-bb32-f9c0a52a439a", false, "empl2", false },
                    { "daf93f3f-45dd-40dc-ae46-b17eebf520e5", 0, "96b95441-1512-41f0-807a-7473c5257261", 1, "admin@testemail.com", true, false, null, "admin", "admin@testemail.com", "admin", "AQAAAAIAAYagAAAAECKief5EM2rI7NPE+CYPsUtrGZiYRwRdh/cu+zuN0aTuwmZJRexFBCaNMm21bTcPww==", 0, null, true, 0, "b1b6ed8a-53c7-4ccd-9877-5002e6d7e026", false, "admin", false },
                    { "eeb1bb63-6ee1-48d6-b025-7e1468dc59ca", 0, "1458936e-e39d-445c-b69e-70113937c81b", 2, "garvis1@testemail.com", true, false, null, "garvis1", "garvis1@testemail.com", "garvis1", "AQAAAAIAAYagAAAAEMMDZS6iqZH1oemHdOpLpGxo8k3XzI5mC/zaYf6AVwiOiLN20uXCXzT/uk4dhfQfNw==", 0, null, false, 1, "58f0e328-27c6-4085-bea1-bc537907f04c", false, "garvis1", false },
                    { "fb16d5f6-af58-45c5-836e-282359f8b193", 0, "2dcbc9d3-622b-45fc-8ee0-e4c22ab569ce", 1, "empl3@testemail.com", true, false, null, "empl3", "empl3testemail.com", "empl3", "AQAAAAIAAYagAAAAELRDpoFU8jP2WG43OK5w50U2SKQSMDshesi/KVbGYVVLBFe997PVx6YjlGV9hS5gkw==", 0, null, false, 0, "6ce011ac-e9d5-453c-8d58-c9aa0ed03208", false, "empl3", false }
                });

            migrationBuilder.InsertData(
                table: "Project",
                columns: new[] { "Id", "CustomerId", "Deleted", "Description", "Name" },
                values: new object[,]
                {
                    { 1, 1, false, null, "InternProject1" },
                    { 2, 1, false, null, "InternProject2" },
                    { 3, 2, false, null, "Project1" },
                    { 4, 2, false, null, "Project2" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "07cd1428-b34e-45cc-8501-874e1d10255c", "b79d6b4c-8ff6-42c6-ad76-38247bfc447b" },
                    { "cc5edb92-cf28-4862-a4f1-1a39f1b27761", "b79d6b4c-8ff6-42c6-ad76-38247bfc447b" },
                    { "07cd1428-b34e-45cc-8501-874e1d10255c", "d19c9551-5000-48df-ad83-9e4ea8cba3e8" },
                    { "cc5edb92-cf28-4862-a4f1-1a39f1b27761", "daf93f3f-45dd-40dc-ae46-b17eebf520e5" },
                    { "082665b9-de26-40b0-a88b-cb46564edbf6", "eeb1bb63-6ee1-48d6-b025-7e1468dc59ca" },
                    { "07cd1428-b34e-45cc-8501-874e1d10255c", "fb16d5f6-af58-45c5-836e-282359f8b193" }
                });

            migrationBuilder.InsertData(
                table: "Ticket",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "CustomerId", "Deleted", "Description", "Effort", "Index", "PriorityId", "ProjectId", "StartDate", "StatusId", "Title", "TypeId" },
                values: new object[,]
                {
                    { 1, "b79d6b4c-8ff6-42c6-ad76-38247bfc447b", new DateTime(2024, 1, 24, 20, 38, 8, 621, DateTimeKind.Local).AddTicks(9093), 1, false, "internal ticket1", null, 1, 1, 1, null, 1, "IntTicket1", 3 },
                    { 2, "b79d6b4c-8ff6-42c6-ad76-38247bfc447b", new DateTime(2024, 1, 24, 20, 38, 8, 621, DateTimeKind.Local).AddTicks(9234), 1, false, "internal ticket2", null, 2, 2, 1, null, 1, "IntTicket2", 2 },
                    { 3, "d19c9551-5000-48df-ad83-9e4ea8cba3e8", new DateTime(2024, 1, 24, 20, 38, 8, 621, DateTimeKind.Local).AddTicks(9240), 1, false, "internal ticket3", null, 3, 3, 2, null, 1, "IntTicket3", 1 },
                    { 4, "eeb1bb63-6ee1-48d6-b025-7e1468dc59ca", new DateTime(2024, 1, 24, 20, 38, 8, 621, DateTimeKind.Local).AddTicks(9247), 2, false, "Ticket Description1", null, 1, 1, 3, null, 1, "Ticket1", 2 },
                    { 5, "eeb1bb63-6ee1-48d6-b025-7e1468dc59ca", new DateTime(2024, 1, 24, 20, 38, 8, 621, DateTimeKind.Local).AddTicks(9253), 2, false, "Ticket Description2", null, 2, 4, 3, null, 1, "Ticket2", 1 },
                    { 6, "eeb1bb63-6ee1-48d6-b025-7e1468dc59ca", new DateTime(2024, 1, 24, 20, 38, 8, 621, DateTimeKind.Local).AddTicks(9257), 2, false, "Ticket Description3", null, 3, 2, 4, null, 1, "Ticket3", 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CustomerId",
                table: "AspNetUsers",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_CreatedBy",
                table: "Comment",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_TicketId",
                table: "Comment",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_CustomerId",
                table: "Project",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_CreatedBy",
                table: "Ticket",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_CustomerId",
                table: "Ticket",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_PriorityId",
                table: "Ticket",
                column: "PriorityId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_ProjectId",
                table: "Ticket",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_StatusId",
                table: "Ticket",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_TypeId",
                table: "Ticket",
                column: "TypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "Reward");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Ticket");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Priority");

            migrationBuilder.DropTable(
                name: "Project");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropTable(
                name: "TicketType");

            migrationBuilder.DropTable(
                name: "Customer");
        }
    }
}
