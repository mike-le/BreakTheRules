using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class StatusId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Status_ApiIdeas_IdeaId",
                table: "Status");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Status",
                table: "Status");

            migrationBuilder.RenameTable(
                name: "Status",
                newName: "Statuses");

            migrationBuilder.RenameIndex(
                name: "IX_Status_IdeaId",
                table: "Statuses",
                newName: "IX_Statuses_IdeaId");

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "ApiIdeas",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Statuses",
                table: "Statuses",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Statuses_ApiIdeas_IdeaId",
                table: "Statuses",
                column: "IdeaId",
                principalTable: "ApiIdeas",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Statuses_ApiIdeas_IdeaId",
                table: "Statuses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Statuses",
                table: "Statuses");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "ApiIdeas");

            migrationBuilder.RenameTable(
                name: "Statuses",
                newName: "Status");

            migrationBuilder.RenameIndex(
                name: "IX_Statuses_IdeaId",
                table: "Status",
                newName: "IX_Status_IdeaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Status",
                table: "Status",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Status_ApiIdeas_IdeaId",
                table: "Status",
                column: "IdeaId",
                principalTable: "ApiIdeas",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
