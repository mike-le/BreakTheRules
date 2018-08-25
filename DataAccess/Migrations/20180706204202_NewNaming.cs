using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class NewNaming : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Themes",
                table: "Themes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ideas",
                table: "Ideas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments",
                table: "Comments");

            migrationBuilder.RenameTable(
                name: "Themes",
                newName: "ApiThemes");

            migrationBuilder.RenameTable(
                name: "Ideas",
                newName: "ApiIdeas");

            migrationBuilder.RenameTable(
                name: "Comments",
                newName: "ApiComments");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "ApiThemes",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "ApiThemes",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "owner_id",
                table: "ApiThemes",
                newName: "OwnerId");

            migrationBuilder.RenameColumn(
                name: "owner_name",
                table: "ApiThemes",
                newName: "Owner");

            migrationBuilder.RenameColumn(
                name: "open_dt",
                table: "ApiThemes",
                newName: "OpenDt");

            migrationBuilder.RenameColumn(
                name: "close_dt",
                table: "ApiThemes",
                newName: "CloseDt");

            migrationBuilder.RenameColumn(
                name: "theme_id",
                table: "ApiThemes",
                newName: "ThemeId");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "ApiIdeas",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "submit_dt",
                table: "ApiIdeas",
                newName: "SubmitDt");

            migrationBuilder.RenameColumn(
                name: "post_score",
                table: "ApiIdeas",
                newName: "Score");

            migrationBuilder.RenameColumn(
                name: "parent_id",
                table: "ApiIdeas",
                newName: "ParentId");

            migrationBuilder.RenameColumn(
                name: "owner_id",
                table: "ApiIdeas",
                newName: "OwnerId");

            migrationBuilder.RenameColumn(
                name: "owner_name",
                table: "ApiIdeas",
                newName: "Owner");

            migrationBuilder.RenameColumn(
                name: "modified_dt",
                table: "ApiIdeas",
                newName: "ModifiedDt");

            migrationBuilder.RenameColumn(
                name: "message_text",
                table: "ApiIdeas",
                newName: "Message");

            migrationBuilder.RenameColumn(
                name: "post_id",
                table: "ApiIdeas",
                newName: "PostId");

            migrationBuilder.RenameColumn(
                name: "submit_dt",
                table: "ApiComments",
                newName: "SubmitDt");

            migrationBuilder.RenameColumn(
                name: "owner_id",
                table: "ApiComments",
                newName: "OwnerId");

            migrationBuilder.RenameColumn(
                name: "owner_name",
                table: "ApiComments",
                newName: "Owner");

            migrationBuilder.RenameColumn(
                name: "message_text",
                table: "ApiComments",
                newName: "Message");

            migrationBuilder.RenameColumn(
                name: "comment_score",
                table: "ApiComments",
                newName: "CommentScore");

            migrationBuilder.RenameColumn(
                name: "comment_id",
                table: "ApiComments",
                newName: "CommentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApiThemes",
                table: "ApiThemes",
                column: "ThemeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApiIdeas",
                table: "ApiIdeas",
                column: "PostId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApiComments",
                table: "ApiComments",
                column: "CommentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ApiThemes",
                table: "ApiThemes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApiIdeas",
                table: "ApiIdeas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApiComments",
                table: "ApiComments");

            migrationBuilder.RenameTable(
                name: "ApiThemes",
                newName: "Themes");

            migrationBuilder.RenameTable(
                name: "ApiIdeas",
                newName: "Ideas");

            migrationBuilder.RenameTable(
                name: "ApiComments",
                newName: "Comments");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Themes",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Themes",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Themes",
                newName: "owner_id");

            migrationBuilder.RenameColumn(
                name: "Owner",
                table: "Themes",
                newName: "owner_name");

            migrationBuilder.RenameColumn(
                name: "OpenDt",
                table: "Themes",
                newName: "open_dt");

            migrationBuilder.RenameColumn(
                name: "CloseDt",
                table: "Themes",
                newName: "close_dt");

            migrationBuilder.RenameColumn(
                name: "ThemeId",
                table: "Themes",
                newName: "theme_id");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Ideas",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "SubmitDt",
                table: "Ideas",
                newName: "submit_dt");

            migrationBuilder.RenameColumn(
                name: "Score",
                table: "Ideas",
                newName: "post_score");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "Ideas",
                newName: "parent_id");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Ideas",
                newName: "owner_id");

            migrationBuilder.RenameColumn(
                name: "Owner",
                table: "Ideas",
                newName: "owner_name");

            migrationBuilder.RenameColumn(
                name: "ModifiedDt",
                table: "Ideas",
                newName: "modified_dt");

            migrationBuilder.RenameColumn(
                name: "Message",
                table: "Ideas",
                newName: "message_text");

            migrationBuilder.RenameColumn(
                name: "PostId",
                table: "Ideas",
                newName: "post_id");

            migrationBuilder.RenameColumn(
                name: "SubmitDt",
                table: "Comments",
                newName: "submit_dt");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Comments",
                newName: "owner_id");

            migrationBuilder.RenameColumn(
                name: "Owner",
                table: "Comments",
                newName: "owner_name");

            migrationBuilder.RenameColumn(
                name: "Message",
                table: "Comments",
                newName: "message_text");

            migrationBuilder.RenameColumn(
                name: "CommentScore",
                table: "Comments",
                newName: "comment_score");

            migrationBuilder.RenameColumn(
                name: "CommentId",
                table: "Comments",
                newName: "comment_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Themes",
                table: "Themes",
                column: "theme_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ideas",
                table: "Ideas",
                column: "post_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments",
                table: "Comments",
                column: "comment_id");
        }
    }
}
