using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImmersionHelper.Data.Migrations
{
    public partial class IndexForVocabulary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Vocabularies_Word_Pronunciation",
                table: "Vocabularies",
                columns: new[] { "Word", "Pronunciation" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Vocabularies_Word_Pronunciation",
                table: "Vocabularies");
        }
    }
}
