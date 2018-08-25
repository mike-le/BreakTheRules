using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    comment_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    comment_score = table.Column<int>(nullable: false),
                    message_text = table.Column<string>(nullable: true),
                    submit_dt = table.Column<DateTime>(nullable: false),
                    owner_name = table.Column<string>(nullable: true),
                    owner_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.comment_id);
                });

            migrationBuilder.CreateTable(
                name: "Ideas",
                columns: table => new
                {
                    post_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    post_score = table.Column<string>(nullable: true),
                    message_text = table.Column<string>(nullable: true),
                    submit_dt = table.Column<DateTime>(nullable: false),
                    modified_dt = table.Column<DateTime>(nullable: false),
                    parent_id = table.Column<int>(nullable: false),
                    status = table.Column<int>(nullable: false),
                    owner_name = table.Column<string>(nullable: true),
                    owner_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ideas", x => x.post_id);
                });

            migrationBuilder.CreateTable(
                name: "Themes",
                columns: table => new
                {
                    theme_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    title = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    open_dt = table.Column<DateTime>(nullable: false),
                    close_dt = table.Column<DateTime>(nullable: false),
                    owner_name = table.Column<string>(nullable: true),
                    owner_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Themes", x => x.theme_id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Ideas");

            migrationBuilder.DropTable(
                name: "Themes");
        }
    }
}
