using Microsoft.EntityFrameworkCore.Migrations;

namespace EventApp.Migrations
{
    public partial class ThirdMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_CreatorUserId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Activities_SpecificActivityActivityId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_CreatorUserId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_SpecificActivityActivityId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "SpecificActivityActivityId",
                table: "Messages");

            migrationBuilder.AddColumn<int>(
                name: "ActivityId",
                table: "Messages",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Messages",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ActivityId",
                table: "Messages",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_UserId",
                table: "Messages",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Activities_ActivityId",
                table: "Messages",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "ActivityId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_UserId",
                table: "Messages",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Activities_ActivityId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_UserId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_ActivityId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_UserId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ActivityId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Messages");

            migrationBuilder.AddColumn<int>(
                name: "CreatorUserId",
                table: "Messages",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SpecificActivityActivityId",
                table: "Messages",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_CreatorUserId",
                table: "Messages",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SpecificActivityActivityId",
                table: "Messages",
                column: "SpecificActivityActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_CreatorUserId",
                table: "Messages",
                column: "CreatorUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Activities_SpecificActivityActivityId",
                table: "Messages",
                column: "SpecificActivityActivityId",
                principalTable: "Activities",
                principalColumn: "ActivityId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
