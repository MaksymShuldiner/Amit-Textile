using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AmitTextile.Migrations
{
    public partial class migration16 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FilterCharachteristicses",
                columns: table => new
                {
                    FilterCharachteristicsId = table.Column<Guid>(nullable: false),
                    CharachteristicId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilterCharachteristicses", x => x.FilterCharachteristicsId);
                    table.ForeignKey(
                        name: "FK_FilterCharachteristicses_Charachteristics_CharachteristicId",
                        column: x => x.CharachteristicId,
                        principalTable: "Charachteristics",
                        principalColumn: "CharachteristicId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FilterCharachteristicses_CharachteristicId",
                table: "FilterCharachteristicses",
                column: "CharachteristicId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FilterCharachteristicses");
        }
    }
}
