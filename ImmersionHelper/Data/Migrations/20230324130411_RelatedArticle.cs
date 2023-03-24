using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImmersionHelper.Data.Migrations
{
    public partial class RelatedArticle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RelatedToArticleId",
                table: "Posts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_RelatedToArticleId",
                table: "Posts",
                column: "RelatedToArticleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Articles_RelatedToArticleId",
                table: "Posts",
                column: "RelatedToArticleId",
                principalTable: "Articles",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Articles_RelatedToArticleId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_RelatedToArticleId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "RelatedToArticleId",
                table: "Posts");
        }
    }
}
