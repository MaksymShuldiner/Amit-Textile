using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AmitTextile.Migrations
{
    public partial class migration6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Sold",
                table: "Textiles",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UserChosenTextile",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    TextileId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserChosenTextile", x => new { x.UserId, x.TextileId });
                    table.ForeignKey(
                        name: "FK_UserChosenTextile_Textiles_TextileId",
                        column: x => x.TextileId,
                        principalTable: "Textiles",
                        principalColumn: "TextileId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserChosenTextile_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserChosenTextile_TextileId",
                table: "UserChosenTextile",
                column: "TextileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserChosenTextile");

            migrationBuilder.DropColumn(
                name: "Sold",
                table: "Textiles");
        }
    }
}
