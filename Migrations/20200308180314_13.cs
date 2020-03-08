using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AmitTextile.Migrations
{
    public partial class _13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Textiles_ChildCategories_ChildCategoryId",
                table: "Textiles");

            migrationBuilder.AlterColumn<Guid>(
                name: "ChildCategoryId",
                table: "Textiles",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Textiles_ChildCategories_ChildCategoryId",
                table: "Textiles",
                column: "ChildCategoryId",
                principalTable: "ChildCategories",
                principalColumn: "ChildCategoryId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Textiles_ChildCategories_ChildCategoryId",
                table: "Textiles");

            migrationBuilder.AlterColumn<Guid>(
                name: "ChildCategoryId",
                table: "Textiles",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Textiles_ChildCategories_ChildCategoryId",
                table: "Textiles",
                column: "ChildCategoryId",
                principalTable: "ChildCategories",
                principalColumn: "ChildCategoryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
