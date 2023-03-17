using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImmersionHelper.Data.Migrations
{
    public partial class WordCountField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WordKnowCount",
                table: "UserArticles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CharacterCount",
                table: "Articles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "N1WordCount",
                table: "Articles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "N2WordCount",
                table: "Articles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "N3WordCount",
                table: "Articles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "N4WordCount",
                table: "Articles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "N5WordCount",
                table: "Articles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WordKnowCount",
                table: "UserArticles");

            migrationBuilder.DropColumn(
                name: "CharacterCount",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "N1WordCount",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "N2WordCount",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "N3WordCount",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "N4WordCount",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "N5WordCount",
                table: "Articles");
        }
    }
}
