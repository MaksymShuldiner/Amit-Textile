using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AmitTextile.Migrations
{
    public partial class _1001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Slider_SliderId",
                table: "Images");

            migrationBuilder.AlterColumn<Guid>(
                name: "SliderId",
                table: "Images",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Slider_SliderId",
                table: "Images",
                column: "SliderId",
                principalTable: "Slider",
                principalColumn: "SliderId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Slider_SliderId",
                table: "Images");

            migrationBuilder.AlterColumn<Guid>(
                name: "SliderId",
                table: "Images",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Slider_SliderId",
                table: "Images",
                column: "SliderId",
                principalTable: "Slider",
                principalColumn: "SliderId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
