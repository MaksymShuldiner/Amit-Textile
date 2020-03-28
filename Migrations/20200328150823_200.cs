using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AmitTextile.Migrations
{
    public partial class _200 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MainTextileId",
                table: "Images",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Images_MainTextileId",
                table: "Images",
                column: "MainTextileId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Textiles_MainTextileId",
                table: "Images",
                column: "MainTextileId",
                principalTable: "Textiles",
                principalColumn: "TextileId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Textiles_MainTextileId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_MainTextileId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "MainTextileId",
                table: "Images");
        }
    }
}
