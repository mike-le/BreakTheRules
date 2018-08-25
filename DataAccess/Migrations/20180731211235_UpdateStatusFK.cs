using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class UpdateStatusFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Statuses_IdeaId",
                table: "Statuses");

            migrationBuilder.CreateIndex(
                name: "IX_Statuses_IdeaId",
                table: "Statuses",
                column: "IdeaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Statuses_IdeaId",
                table: "Statuses");

            migrationBuilder.CreateIndex(
                name: "IX_Statuses_IdeaId",
                table: "Statuses",
                column: "IdeaId",
                unique: true);
        }
    }
}
