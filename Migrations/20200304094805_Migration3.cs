using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AmitTextile.Migrations
{
    public partial class Migration3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SliderId",
                table: "Images",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Slider",
                columns: table => new
                {
                    SliderId = table.Column<Guid>(nullable: false),
                    SliderName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Slider", x => x.SliderId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Images_SliderId",
                table: "Images",
                column: "SliderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Slider_SliderId",
                table: "Images",
                column: "SliderId",
                principalTable: "Slider",
                principalColumn: "SliderId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Slider_SliderId",
                table: "Images");

            migrationBuilder.DropTable(
                name: "Slider");

            migrationBuilder.DropIndex(
                name: "IX_Images_SliderId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "SliderId",
                table: "Images");
        }
    }
}
