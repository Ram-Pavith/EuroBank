using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EuroBankAPI.Migrations
{
    public partial class FluentAPIFixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "Accounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 4, 3, 15, 21, 23, 102, DateTimeKind.Local).AddTicks(7275),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 4, 3, 15, 16, 37, 205, DateTimeKind.Local).AddTicks(178));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "Accounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 4, 3, 15, 16, 37, 205, DateTimeKind.Local).AddTicks(178),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 4, 3, 15, 21, 23, 102, DateTimeKind.Local).AddTicks(7275));
        }
    }
}
