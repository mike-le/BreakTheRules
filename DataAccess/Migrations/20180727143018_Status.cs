using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class Status : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "ApiIdeas");

            migrationBuilder.DropColumn(
                name: "AccountableGroup",
                table: "ApiComments");

            migrationBuilder.DropColumn(
                name: "ResponseText",
                table: "ApiComments");

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    StatusId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StatusCode = table.Column<int>(nullable: false),
                    Response = table.Column<string>(nullable: true),
                    SubmitDt = table.Column<DateTime>(nullable: false),
                    IdeaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.StatusId);
                    table.ForeignKey(
                        name: "FK_Status_ApiIdeas_IdeaId",
                        column: x => x.IdeaId,
                        principalTable: "ApiIdeas",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Status_IdeaId",
                table: "Status",
                column: "IdeaId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ApiIdeas",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AccountableGroup",
                table: "ApiComments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResponseText",
                table: "ApiComments",
                nullable: true);
        }
    }
}
