using Microsoft.EntityFrameworkCore.Migrations;

namespace EventApp.Migrations
{
    public partial class SecondMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Messages_MessageId1",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_MessageId1",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "MessageId1",
                table: "Messages");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MessageId1",
                table: "Messages",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_MessageId1",
                table: "Messages",
                column: "MessageId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Messages_MessageId1",
                table: "Messages",
                column: "MessageId1",
                principalTable: "Messages",
                principalColumn: "MessageId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
