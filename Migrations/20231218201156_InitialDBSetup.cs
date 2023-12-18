using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KanbanRMR.Migrations
{
    /// <inheritdoc />
    public partial class InitialDBSetup : Migration
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
                });

            migrationBuilder.CreateTable(
                name: "Customers",
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
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Priorities",
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
                    table.PrimaryKey("PK_Priorities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
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
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rewards",
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
                    table.PrimaryKey("PK_Rewards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TicketTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketTypes", x => x.Id);
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
                name: "Tickets",
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
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tickets_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tickets_Priorities_PriorityId",
                        column: x => x.PriorityId,
                        principalTable: "Priorities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tickets_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tickets_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tickets_TicketTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "TicketTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TicketId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comment_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "a576e28e-d1a7-4ddb-84b9-e0843ad98922", null, "user", "user" },
                    { "bbc0a426-0973-4f3f-9b85-04ce5de3f0ca", null, "admin", "admin" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CustomerId", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "Penalties", "PhoneNumber", "PhoneNumberConfirmed", "Points", "SecurityStamp", "TwoFactorEnabled", "UserName", "deleted" },
                values: new object[,]
                {
                    { "01d5f1e3-8791-4924-bb2c-a4153383492c", 0, "0bb55c3f-6fbc-4853-bd4b-547dc8f683ac", 1, "admin@testemail.com", true, false, null, "admin", "admin@testemail.com", "admin", "AQAAAAIAAYagAAAAEEoP0z7nxxe4wG7GqKB45SOH06TtabCDkNcFePuT4bcPNnssbvf4RGWJFPoCPMGRrA==", 0, null, true, 0, "0858bb38-8c97-4b1e-b650-f46e64727e6c", false, "admin", false },
                    { "21f98a97-5fa3-476a-a783-6d5802491050", 0, "84b464be-e795-4140-8708-6bc66b8696a9", 1, "empl2@testemail.com", true, false, null, "empl2", "empl2@testemail.com", "empl2", "AQAAAAIAAYagAAAAEJxOUuC8T2BdOO+IljsP+lPcklUIdQlrhPxAvKrvq0zNB82La4fh9EMoB+lD1iaLyA==", 0, null, false, 0, "dbcc3ca5-b64b-4753-8c28-bf5d107fba31", false, "empl2", false },
                    { "35f4a031-3b7a-46d8-b13d-34ab024d5a0c", 0, "5e921558-6dcb-4d13-927d-718d6afee4c5", 2, "garvis1@testemail.com", true, false, null, "garvis1", "garvis1@testemail.com", "garvis1", "AQAAAAIAAYagAAAAEOfg6WjspbC5JQrUFDzYeOg+9sDhWGb+tfMsI7UNjsSbphM7h0guJ2yUZTVAxkabLw==", 0, null, false, 0, "b6e8ac3e-035d-4f22-80c9-04746e879440", false, "garvis1", false },
                    { "a275286d-59d2-4060-b047-c1a1d7ccd460", 0, "222e2f66-426f-40b9-be59-3e06f2ab6822", 1, "empl1@testemail.com", true, false, null, "empl1", "empl1@testemail.com", "empl1", "AQAAAAIAAYagAAAAEIRot0fkf1QDnM+Gfzu3io5/WYDzQ5sbaGXqd3yQs1s/zMrQ53f4542gPJhXyyXvSw==", 0, null, false, 0, "a05b2bb5-9187-4101-9f14-bbeb9f9c27f3", false, "empl1", false }
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Deleted", "Description", "Name" },
                values: new object[,]
                {
                    { 1, false, null, "Intern" },
                    { 2, false, null, "Garvis" }
                });

            migrationBuilder.InsertData(
                table: "Priorities",
                columns: new[] { "Id", "Color", "Deleted", "Description" },
                values: new object[,]
                {
                    { 1, "#f7f7ed", false, "Minor" },
                    { 2, "#fcfc03", false, "Major" },
                    { 3, "#fcba03", false, "Critical" },
                    { 4, "#fc5a03", false, "Blocking" }
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "CustomerId", "Deleted", "Description", "Name" },
                values: new object[,]
                {
                    { 1, 1, false, null, "InternProject1" },
                    { 2, 1, false, null, "InternProject2" },
                    { 3, 2, false, null, "Project1" },
                    { 4, 2, false, null, "Project2" }
                });

            migrationBuilder.InsertData(
                table: "Rewards",
                columns: new[] { "Id", "Action", "Deleted", "Enabled", "Points" },
                values: new object[,]
                {
                    { 1, "CreatedTicket", false, true, 1 },
                    { 2, "CreatedDuplicateTicket", false, true, -1 },
                    { 3, "MoveToDone", false, true, 1 }
                });

            migrationBuilder.InsertData(
                table: "Statuses",
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
                table: "TicketTypes",
                columns: new[] { "Id", "Deleted", "Description" },
                values: new object[,]
                {
                    { 1, false, "Bug" },
                    { 2, false, "Feature" },
                    { 3, false, "Improvement" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "bbc0a426-0973-4f3f-9b85-04ce5de3f0ca", "01d5f1e3-8791-4924-bb2c-a4153383492c" },
                    { "bbc0a426-0973-4f3f-9b85-04ce5de3f0ca", "21f98a97-5fa3-476a-a783-6d5802491050" },
                    { "a576e28e-d1a7-4ddb-84b9-e0843ad98922", "35f4a031-3b7a-46d8-b13d-34ab024d5a0c" },
                    { "bbc0a426-0973-4f3f-9b85-04ce5de3f0ca", "a275286d-59d2-4060-b047-c1a1d7ccd460" }
                });

            migrationBuilder.InsertData(
                table: "Tickets",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "CustomerId", "Deleted", "Description", "Effort", "Index", "PriorityId", "ProjectId", "StartDate", "StatusId", "Title", "TypeId" },
                values: new object[,]
                {
                    { 1, "a275286d-59d2-4060-b047-c1a1d7ccd460", new DateTime(2023, 12, 18, 21, 11, 55, 688, DateTimeKind.Local).AddTicks(6932), 1, false, "internal ticket1", null, 1, 1, 1, null, 1, "IntTicket1", 3 },
                    { 2, "a275286d-59d2-4060-b047-c1a1d7ccd460", new DateTime(2023, 12, 18, 21, 11, 55, 688, DateTimeKind.Local).AddTicks(7021), 1, false, "internal ticket2", null, 2, 2, 1, null, 1, "IntTicket2", 2 },
                    { 3, "21f98a97-5fa3-476a-a783-6d5802491050", new DateTime(2023, 12, 18, 21, 11, 55, 688, DateTimeKind.Local).AddTicks(7030), 1, false, "internal ticket3", null, 3, 3, 2, null, 1, "IntTicket3", 1 },
                    { 4, "35f4a031-3b7a-46d8-b13d-34ab024d5a0c", new DateTime(2023, 12, 18, 21, 11, 55, 688, DateTimeKind.Local).AddTicks(7037), 2, false, "Ticket Description1", null, 1, 1, 3, null, 1, "Ticket1", 2 },
                    { 5, "35f4a031-3b7a-46d8-b13d-34ab024d5a0c", new DateTime(2023, 12, 18, 21, 11, 55, 688, DateTimeKind.Local).AddTicks(7044), 2, false, "Ticket Description2", null, 2, 4, 3, null, 1, "Ticket2", 1 },
                    { 6, "35f4a031-3b7a-46d8-b13d-34ab024d5a0c", new DateTime(2023, 12, 18, 21, 11, 55, 688, DateTimeKind.Local).AddTicks(7051), 2, false, "Ticket Description3", null, 3, 2, 4, null, 1, "Ticket3", 1 }
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
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_TicketId",
                table: "Comment",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_CreatedBy",
                table: "Tickets",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_CustomerId",
                table: "Tickets",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_PriorityId",
                table: "Tickets",
                column: "PriorityId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ProjectId",
                table: "Tickets",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_StatusId",
                table: "Tickets",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_TypeId",
                table: "Tickets",
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
                name: "Rewards");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Priorities");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Statuses");

            migrationBuilder.DropTable(
                name: "TicketTypes");
        }
    }
}
