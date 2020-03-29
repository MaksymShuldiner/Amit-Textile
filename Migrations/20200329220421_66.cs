using Microsoft.EntityFrameworkCore.Migrations;

namespace AmitTextile.Migrations
{
    public partial class _66 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Slider_SliderId",
                table: "Images");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Slider",
                table: "Slider");

            migrationBuilder.RenameTable(
                name: "Slider",
                newName: "Sliders");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sliders",
                table: "Sliders",
                column: "SliderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Sliders_SliderId",
                table: "Images",
                column: "SliderId",
                principalTable: "Sliders",
                principalColumn: "SliderId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Sliders_SliderId",
                table: "Images");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sliders",
                table: "Sliders");

            migrationBuilder.RenameTable(
                name: "Sliders",
                newName: "Slider");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Slider",
                table: "Slider",
                column: "SliderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Slider_SliderId",
                table: "Images",
                column: "SliderId",
                principalTable: "Slider",
                principalColumn: "SliderId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
