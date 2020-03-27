using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AmitTextile.Migrations
{
    public partial class _100 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastTimeEmailSent",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastTimeEmailChanged",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastTimeEmailForEmailSent",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastTimeEmailForPassSent",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastTimeEmailChanged",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastTimeEmailForEmailSent",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastTimeEmailForPassSent",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastTimeEmailSent",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);
        }
    }
}
