using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class NewVotes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Statuses_IdeaId",
                table: "Statuses");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "ApiIdeas");

            migrationBuilder.CreateTable(
                name: "Votes",
                columns: table => new
                {
                    VoteId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Direction = table.Column<int>(nullable: false),
                    SubmitDt = table.Column<DateTime>(nullable: false),
                    Owner = table.Column<string>(nullable: true),
                    IdeaId = table.Column<int>(nullable: true),
                    CommentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Votes", x => x.VoteId);
                    table.ForeignKey(
                        name: "FK_Votes_ApiComments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "ApiComments",
                        principalColumn: "CommentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Votes_ApiIdeas_IdeaId",
                        column: x => x.IdeaId,
                        principalTable: "ApiIdeas",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Statuses_IdeaId",
                table: "Statuses",
                column: "IdeaId");

            migrationBuilder.CreateIndex(
                name: "IX_Votes_CommentId",
                table: "Votes",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Votes_IdeaId",
                table: "Votes",
                column: "IdeaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Votes");

            migrationBuilder.DropIndex(
                name: "IX_Statuses_IdeaId",
                table: "Statuses");

            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "ApiIdeas",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "ApiComments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Statuses_IdeaId",
                table: "Statuses",
                column: "IdeaId",
                unique: true);
        }
    }
}
