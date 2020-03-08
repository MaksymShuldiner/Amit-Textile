using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AmitTextile.Migrations
{
    public partial class _12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Textiles_Categories_CategoryId",
                table: "Textiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Textiles_ChildCategories_ChildCategoryId",
                table: "Textiles");

            migrationBuilder.AlterColumn<Guid>(
                name: "ChildCategoryId",
                table: "Textiles",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryId",
                table: "Textiles",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Textiles_Categories_CategoryId",
                table: "Textiles",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Textiles_ChildCategories_ChildCategoryId",
                table: "Textiles",
                column: "ChildCategoryId",
                principalTable: "ChildCategories",
                principalColumn: "ChildCategoryId",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Textiles_Categories_CategoryId",
                table: "Textiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Textiles_ChildCategories_ChildCategoryId",
                table: "Textiles");

            migrationBuilder.AlterColumn<Guid>(
                name: "ChildCategoryId",
                table: "Textiles",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryId",
                table: "Textiles",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_Textiles_Categories_CategoryId",
                table: "Textiles",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Textiles_ChildCategories_ChildCategoryId",
                table: "Textiles",
                column: "ChildCategoryId",
                principalTable: "ChildCategories",
                principalColumn: "ChildCategoryId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
