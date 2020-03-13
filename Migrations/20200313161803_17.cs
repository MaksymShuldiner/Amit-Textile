using Microsoft.EntityFrameworkCore.Migrations;

namespace AmitTextile.Migrations
{
    public partial class _17 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FilterCharachteristicses_CharachteristicId",
                table: "FilterCharachteristicses");

            migrationBuilder.CreateIndex(
                name: "IX_FilterCharachteristicses_CharachteristicId",
                table: "FilterCharachteristicses",
                column: "CharachteristicId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FilterCharachteristicses_CharachteristicId",
                table: "FilterCharachteristicses");

            migrationBuilder.CreateIndex(
                name: "IX_FilterCharachteristicses_CharachteristicId",
                table: "FilterCharachteristicses",
                column: "CharachteristicId");
        }
    }
}
