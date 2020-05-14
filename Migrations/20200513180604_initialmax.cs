using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AmitTextile.Migrations
{
    public partial class initialmax : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsWithWholesale",
                table: "Items");

            migrationBuilder.AddColumn<Guid>(
                name: "ImageId",
                table: "ChildCategories",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChildCategories_ImageId",
                table: "ChildCategories",
                column: "ImageId",
                unique: true,
                filter: "[ImageId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ChildCategories_Images_ImageId",
                table: "ChildCategories",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "ImageId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChildCategories_Images_ImageId",
                table: "ChildCategories");

            migrationBuilder.DropIndex(
                name: "IX_ChildCategories_ImageId",
                table: "ChildCategories");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "ChildCategories");

            migrationBuilder.AddColumn<bool>(
                name: "IsWithWholesale",
                table: "Items",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
