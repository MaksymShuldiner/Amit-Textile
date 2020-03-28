using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AmitTextile.Migrations
{
    public partial class _500 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Textiles_MainTextileId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_Textiles_TextileId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_MainTextileId",
                table: "Images");

            migrationBuilder.AlterColumn<Guid>(
                name: "TextileId",
                table: "Images",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "MainTextileId",
                table: "Images",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Images_MainTextileId",
                table: "Images",
                column: "MainTextileId",
                unique: true,
                filter: "[MainTextileId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Textiles_MainTextileId",
                table: "Images",
                column: "MainTextileId",
                principalTable: "Textiles",
                principalColumn: "TextileId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Textiles_TextileId",
                table: "Images",
                column: "TextileId",
                principalTable: "Textiles",
                principalColumn: "TextileId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Textiles_MainTextileId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_Textiles_TextileId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_MainTextileId",
                table: "Images");

            migrationBuilder.AlterColumn<Guid>(
                name: "TextileId",
                table: "Images",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "MainTextileId",
                table: "Images",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

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
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Textiles_TextileId",
                table: "Images",
                column: "TextileId",
                principalTable: "Textiles",
                principalColumn: "TextileId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
