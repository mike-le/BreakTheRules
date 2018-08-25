using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class CommentFKPartII : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApiComments_ApiIdeas_IdeaId",
                table: "ApiComments");

            migrationBuilder.DropIndex(
                name: "IX_ApiComments_IdeaId",
                table: "ApiComments");

            migrationBuilder.DropColumn(
                name: "IdeaId",
                table: "ApiComments");

            migrationBuilder.AddColumn<int>(
                name: "ParentCommentId",
                table: "ApiComments",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParentIdeaId",
                table: "ApiComments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApiComments_ParentCommentId",
                table: "ApiComments",
                column: "ParentCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_ApiComments_ParentIdeaId",
                table: "ApiComments",
                column: "ParentIdeaId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApiComments_ApiComments_ParentCommentId",
                table: "ApiComments",
                column: "ParentCommentId",
                principalTable: "ApiComments",
                principalColumn: "CommentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApiComments_ApiIdeas_ParentIdeaId",
                table: "ApiComments",
                column: "ParentIdeaId",
                principalTable: "ApiIdeas",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApiComments_ApiComments_ParentCommentId",
                table: "ApiComments");

            migrationBuilder.DropForeignKey(
                name: "FK_ApiComments_ApiIdeas_ParentIdeaId",
                table: "ApiComments");

            migrationBuilder.DropIndex(
                name: "IX_ApiComments_ParentCommentId",
                table: "ApiComments");

            migrationBuilder.DropIndex(
                name: "IX_ApiComments_ParentIdeaId",
                table: "ApiComments");

            migrationBuilder.DropColumn(
                name: "ParentCommentId",
                table: "ApiComments");

            migrationBuilder.DropColumn(
                name: "ParentIdeaId",
                table: "ApiComments");

            migrationBuilder.AddColumn<int>(
                name: "IdeaId",
                table: "ApiComments",
                nullable: false,
                defaultValue: 0);

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
        }
    }
}
