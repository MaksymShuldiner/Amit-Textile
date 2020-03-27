using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AmitTextile.Migrations
{
    public partial class _60 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastTimeEmailSent",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastTimePassChanged",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastTimeEmailSent",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastTimePassChanged",
                table: "AspNetUsers");
        }
    }
}
