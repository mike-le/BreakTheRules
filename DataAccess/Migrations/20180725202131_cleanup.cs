using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class cleanup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "ApiThemes");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "ApiIdeas");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "ApiComments");

            migrationBuilder.AddColumn<string>(
                name: "AccountableGroup",
                table: "ApiComments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResponseText",
                table: "ApiComments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountableGroup",
                table: "ApiComments");

            migrationBuilder.DropColumn(
                name: "ResponseText",
                table: "ApiComments");

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "ApiThemes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "ApiIdeas",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "ApiComments",
                nullable: false,
                defaultValue: 0);
        }
    }
}
