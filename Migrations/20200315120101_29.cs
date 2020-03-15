using Microsoft.EntityFrameworkCore.Migrations;

namespace AmitTextile.Migrations
{
    public partial class _29 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Advantages",
                table: "ParentCommentReviews",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DrawBacks",
                table: "ParentCommentReviews",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Advantages",
                table: "ParentCommentReviews");

            migrationBuilder.DropColumn(
                name: "DrawBacks",
                table: "ParentCommentReviews");
        }
    }
}
