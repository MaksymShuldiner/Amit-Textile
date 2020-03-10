using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AmitTextile.Migrations
{
    public partial class Migration1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isOnDiscount",
                table: "Textiles",
                newName: "IsOnDiscount");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateWhenAdded",
                table: "Textiles",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsPopular",
                table: "Textiles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ViewsCounter",
                table: "Textiles",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateWhenAdded",
                table: "Textiles");

            migrationBuilder.DropColumn(
                name: "IsPopular",
                table: "Textiles");

            migrationBuilder.DropColumn(
                name: "ViewsCounter",
                table: "Textiles");

            migrationBuilder.RenameColumn(
                name: "IsOnDiscount",
                table: "Textiles",
                newName: "isOnDiscount");
        }
    }
}
