using Microsoft.EntityFrameworkCore.Migrations;

namespace AmitTextile.Migrations
{
    public partial class _10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cost",
                table: "Textiles");

            migrationBuilder.AlterColumn<decimal>(
                name: "CostWithDiscount",
                table: "Textiles",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "CostWithDiscount",
                table: "Textiles",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AddColumn<double>(
                name: "Cost",
                table: "Textiles",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
