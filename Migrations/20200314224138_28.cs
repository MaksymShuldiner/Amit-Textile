using Microsoft.EntityFrameworkCore.Migrations;

namespace AmitTextile.Migrations
{
    public partial class _28 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Textiles",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Textiles");
        }
    }
}
