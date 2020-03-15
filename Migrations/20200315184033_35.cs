using Microsoft.EntityFrameworkCore.Migrations;

namespace AmitTextile.Migrations
{
    public partial class _35 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Fio",
                table: "ParentCommentReviews",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Fio",
                table: "ParentCommentQuestions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Fio",
                table: "ChildCommentQuestions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fio",
                table: "ParentCommentReviews");

            migrationBuilder.DropColumn(
                name: "Fio",
                table: "ParentCommentQuestions");

            migrationBuilder.DropColumn(
                name: "Fio",
                table: "ChildCommentQuestions");
        }
    }
}
