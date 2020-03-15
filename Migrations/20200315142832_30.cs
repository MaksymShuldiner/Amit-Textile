using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AmitTextile.Migrations
{
    public partial class _30 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "NonAuthorizedId",
                table: "Carts",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NonAuthorizedId",
                table: "Carts");
        }
    }
}
