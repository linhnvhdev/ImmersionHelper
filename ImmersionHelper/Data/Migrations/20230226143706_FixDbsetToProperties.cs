using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImmersionHelper.Data.Migrations
{
    public partial class FixDbsetToProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserArticle_Article_ArticleId",
                table: "UserArticle");

            migrationBuilder.DropForeignKey(
                name: "FK_UserArticle_AspNetUsers_ApplicationUserId",
                table: "UserArticle");

            migrationBuilder.DropForeignKey(
                name: "FK_UserVocabulary_AspNetUsers_ApplicationUserId",
                table: "UserVocabulary");

            migrationBuilder.DropForeignKey(
                name: "FK_UserVocabulary_Vocabulary_VocabularyId",
                table: "UserVocabulary");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vocabulary",
                table: "Vocabulary");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserVocabulary",
                table: "UserVocabulary");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserArticle",
                table: "UserArticle");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Article",
                table: "Article");

            migrationBuilder.RenameTable(
                name: "Vocabulary",
                newName: "Vocabularies");

            migrationBuilder.RenameTable(
                name: "UserVocabulary",
                newName: "UserVocabularies");

            migrationBuilder.RenameTable(
                name: "UserArticle",
                newName: "UserArticles");

            migrationBuilder.RenameTable(
                name: "Article",
                newName: "Articles");

            migrationBuilder.RenameIndex(
                name: "IX_UserVocabulary_ApplicationUserId",
                table: "UserVocabularies",
                newName: "IX_UserVocabularies_ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserArticle_ApplicationUserId",
                table: "UserArticles",
                newName: "IX_UserArticles_ApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vocabularies",
                table: "Vocabularies",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserVocabularies",
                table: "UserVocabularies",
                columns: new[] { "VocabularyId", "ApplicationUserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserArticles",
                table: "UserArticles",
                columns: new[] { "ArticleId", "ApplicationUserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Articles",
                table: "Articles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserArticles_Articles_ArticleId",
                table: "UserArticles",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserArticles_AspNetUsers_ApplicationUserId",
                table: "UserArticles",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserVocabularies_AspNetUsers_ApplicationUserId",
                table: "UserVocabularies",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserVocabularies_Vocabularies_VocabularyId",
                table: "UserVocabularies",
                column: "VocabularyId",
                principalTable: "Vocabularies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserArticles_Articles_ArticleId",
                table: "UserArticles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserArticles_AspNetUsers_ApplicationUserId",
                table: "UserArticles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserVocabularies_AspNetUsers_ApplicationUserId",
                table: "UserVocabularies");

            migrationBuilder.DropForeignKey(
                name: "FK_UserVocabularies_Vocabularies_VocabularyId",
                table: "UserVocabularies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vocabularies",
                table: "Vocabularies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserVocabularies",
                table: "UserVocabularies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserArticles",
                table: "UserArticles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Articles",
                table: "Articles");

            migrationBuilder.RenameTable(
                name: "Vocabularies",
                newName: "Vocabulary");

            migrationBuilder.RenameTable(
                name: "UserVocabularies",
                newName: "UserVocabulary");

            migrationBuilder.RenameTable(
                name: "UserArticles",
                newName: "UserArticle");

            migrationBuilder.RenameTable(
                name: "Articles",
                newName: "Article");

            migrationBuilder.RenameIndex(
                name: "IX_UserVocabularies_ApplicationUserId",
                table: "UserVocabulary",
                newName: "IX_UserVocabulary_ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserArticles_ApplicationUserId",
                table: "UserArticle",
                newName: "IX_UserArticle_ApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vocabulary",
                table: "Vocabulary",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserVocabulary",
                table: "UserVocabulary",
                columns: new[] { "VocabularyId", "ApplicationUserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserArticle",
                table: "UserArticle",
                columns: new[] { "ArticleId", "ApplicationUserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Article",
                table: "Article",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserArticle_Article_ArticleId",
                table: "UserArticle",
                column: "ArticleId",
                principalTable: "Article",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserArticle_AspNetUsers_ApplicationUserId",
                table: "UserArticle",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserVocabulary_AspNetUsers_ApplicationUserId",
                table: "UserVocabulary",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserVocabulary_Vocabulary_VocabularyId",
                table: "UserVocabulary",
                column: "VocabularyId",
                principalTable: "Vocabulary",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
