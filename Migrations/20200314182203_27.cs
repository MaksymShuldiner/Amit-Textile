using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AmitTextile.Migrations
{
    public partial class _27 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TextileId",
                table: "ParentCommentReviews",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TextileId",
                table: "ParentCommentQuestions",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TextileId",
                table: "ChildCommentQuestions",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ParentCommentReviews_TextileId",
                table: "ParentCommentReviews",
                column: "TextileId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentCommentQuestions_TextileId",
                table: "ParentCommentQuestions",
                column: "TextileId");

            migrationBuilder.CreateIndex(
                name: "IX_ChildCommentQuestions_TextileId",
                table: "ChildCommentQuestions",
                column: "TextileId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChildCommentQuestions_Textiles_TextileId",
                table: "ChildCommentQuestions",
                column: "TextileId",
                principalTable: "Textiles",
                principalColumn: "TextileId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ParentCommentQuestions_Textiles_TextileId",
                table: "ParentCommentQuestions",
                column: "TextileId",
                principalTable: "Textiles",
                principalColumn: "TextileId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ParentCommentReviews_Textiles_TextileId",
                table: "ParentCommentReviews",
                column: "TextileId",
                principalTable: "Textiles",
                principalColumn: "TextileId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChildCommentQuestions_Textiles_TextileId",
                table: "ChildCommentQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_ParentCommentQuestions_Textiles_TextileId",
                table: "ParentCommentQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_ParentCommentReviews_Textiles_TextileId",
                table: "ParentCommentReviews");

            migrationBuilder.DropIndex(
                name: "IX_ParentCommentReviews_TextileId",
                table: "ParentCommentReviews");

            migrationBuilder.DropIndex(
                name: "IX_ParentCommentQuestions_TextileId",
                table: "ParentCommentQuestions");

            migrationBuilder.DropIndex(
                name: "IX_ChildCommentQuestions_TextileId",
                table: "ChildCommentQuestions");

            migrationBuilder.DropColumn(
                name: "TextileId",
                table: "ParentCommentReviews");

            migrationBuilder.DropColumn(
                name: "TextileId",
                table: "ParentCommentQuestions");

            migrationBuilder.DropColumn(
                name: "TextileId",
                table: "ChildCommentQuestions");
        }
    }
}
