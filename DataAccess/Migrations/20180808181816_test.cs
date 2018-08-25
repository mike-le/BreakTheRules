using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Votes_ApiComments_CommentId",
                table: "Votes");

            migrationBuilder.DropForeignKey(
                name: "FK_Votes_ApiIdeas_IdeaId",
                table: "Votes");

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_ApiComments_CommentId",
                table: "Votes",
                column: "CommentId",
                principalTable: "ApiComments",
                principalColumn: "CommentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_ApiIdeas_IdeaId",
                table: "Votes",
                column: "IdeaId",
                principalTable: "ApiIdeas",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Votes_ApiComments_CommentId",
                table: "Votes");

            migrationBuilder.DropForeignKey(
                name: "FK_Votes_ApiIdeas_IdeaId",
                table: "Votes");

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_ApiComments_CommentId",
                table: "Votes",
                column: "CommentId",
                principalTable: "ApiComments",
                principalColumn: "CommentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_ApiIdeas_IdeaId",
                table: "Votes",
                column: "IdeaId",
                principalTable: "ApiIdeas",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
