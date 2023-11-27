using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KanbanRMR.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2023, 11, 27, 18, 53, 17, 879, DateTimeKind.Local).AddTicks(5439));

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2023, 11, 27, 18, 53, 17, 879, DateTimeKind.Local).AddTicks(5494));
        }
    }
}
