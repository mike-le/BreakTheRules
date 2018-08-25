using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class AddFKs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "ApiIdeas",
                newName: "ThemeId");

            migrationBuilder.AddColumn<int>(
                name: "IdeaId",
                table: "ApiComments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ApiIdeas_ThemeId",
                table: "ApiIdeas",
                column: "ThemeId");

            migrationBuilder.CreateIndex(
                name: "IX_ApiComments_IdeaId",
                table: "ApiComments",
                column: "IdeaId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApiComments_ApiIdeas_IdeaId",
                table: "ApiComments",
                column: "IdeaId",
                principalTable: "ApiIdeas",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApiIdeas_ApiThemes_ThemeId",
                table: "ApiIdeas",
                column: "ThemeId",
                principalTable: "ApiThemes",
                principalColumn: "ThemeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApiComments_ApiIdeas_IdeaId",
                table: "ApiComments");

            migrationBuilder.DropForeignKey(
                name: "FK_ApiIdeas_ApiThemes_ThemeId",
                table: "ApiIdeas");

            migrationBuilder.DropIndex(
                name: "IX_ApiIdeas_ThemeId",
                table: "ApiIdeas");

            migrationBuilder.DropIndex(
                name: "IX_ApiComments_IdeaId",
                table: "ApiComments");

            migrationBuilder.DropColumn(
                name: "IdeaId",
                table: "ApiComments");

            migrationBuilder.RenameColumn(
                name: "ThemeId",
                table: "ApiIdeas",
                newName: "ParentId");
        }
    }
}
