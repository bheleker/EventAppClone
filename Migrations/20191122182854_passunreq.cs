using Microsoft.EntityFrameworkCore.Migrations;

namespace EventApp.Migrations
{
    public partial class passunreq : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IDToken",
                table: "Users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IDToken",
                table: "Users",
                nullable: true);
        }
    }
}
