using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KanbanRMR.Migrations
{
    /// <inheritdoc />
    public partial class CreateTicketsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2023, 11, 27, 18, 57, 42, 992, DateTimeKind.Local).AddTicks(8933));

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2023, 11, 27, 18, 57, 42, 992, DateTimeKind.Local).AddTicks(8991));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2023, 11, 27, 18, 56, 0, 473, DateTimeKind.Local).AddTicks(7815));

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2023, 11, 27, 18, 56, 0, 473, DateTimeKind.Local).AddTicks(7873));
        }
    }
}
