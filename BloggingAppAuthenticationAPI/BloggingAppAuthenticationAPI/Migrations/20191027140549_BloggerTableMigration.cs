using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BloggingAppAuthenticationAPI.Migrations
{
    public partial class BloggerTableMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bloggers",
                columns: table => new
                {
                    BloggerId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BloggerFullName = table.Column<string>(maxLength: 50, nullable: false),
                    BloggerEmail = table.Column<string>(nullable: false),
                    BloggerDOB = table.Column<DateTime>(nullable: false),
                    BloggerPasswordHash = table.Column<string>(nullable: false),
                    BloggerSalt = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bloggers", x => x.BloggerId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bloggers");
        }
    }
}
