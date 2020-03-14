using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AmitTextile.Migrations
{
    public partial class _18 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChildCommentReviews");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChildCommentReviews",
                columns: table => new
                {
                    ChildCommentReviewId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DatePosted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ParentCommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SenderId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChildCommentReviews", x => x.ChildCommentReviewId);
                    table.ForeignKey(
                        name: "FK_ChildCommentReviews_ParentCommentReviews_ParentCommentId",
                        column: x => x.ParentCommentId,
                        principalTable: "ParentCommentReviews",
                        principalColumn: "ParentCommentReviewId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChildCommentReviews_AspNetUsers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChildCommentReviews_ParentCommentId",
                table: "ChildCommentReviews",
                column: "ParentCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_ChildCommentReviews_SenderId",
                table: "ChildCommentReviews",
                column: "SenderId");
        }
    }
}
