using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AmitTextile.Migrations
{
    public partial class migration15 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Textiles_Charachteristics_CharachteristicId",
                table: "Textiles");

            migrationBuilder.DropIndex(
                name: "IX_Textiles_CharachteristicId",
                table: "Textiles");

            migrationBuilder.DropColumn(
                name: "CharachteristicId",
                table: "Textiles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CharachteristicId",
                table: "Textiles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Textiles_CharachteristicId",
                table: "Textiles",
                column: "CharachteristicId");

            migrationBuilder.AddForeignKey(
                name: "FK_Textiles_Charachteristics_CharachteristicId",
                table: "Textiles",
                column: "CharachteristicId",
                principalTable: "Charachteristics",
                principalColumn: "CharachteristicId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
