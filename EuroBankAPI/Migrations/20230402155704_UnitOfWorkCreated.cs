using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EuroBankAPI.Migrations
{
    public partial class UnitOfWorkCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersAuth",
                table: "UsersAuth");

            migrationBuilder.RenameTable(
                name: "UsersAuth",
                newName: "UserAuths");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAuths",
                table: "UserAuths",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAuths",
                table: "UserAuths");

            migrationBuilder.RenameTable(
                name: "UserAuths",
                newName: "UsersAuth");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersAuth",
                table: "UsersAuth",
                column: "Id");
        }
    }
}
