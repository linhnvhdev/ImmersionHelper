using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImmersionHelper.Data.Migrations
{
    public partial class AddPageSource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Author",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "PublicationDate",
                table: "Articles");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Articles",
                newName: "Link");

            migrationBuilder.AddColumn<int>(
                name: "PageSourceId",
                table: "Articles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PageSources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PageLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MainPageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PageTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MainSectionPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ArticleContentPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsImageLinkAbsolute = table.Column<bool>(type: "bit", nullable: false),
                    IsLinkAbsolute = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageSources", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "PageSources",
                columns: new[] { "Id", "ArticleContentPath", "IsImageLinkAbsolute", "IsLinkAbsolute", "MainPageUrl", "MainSectionPath", "PageLink", "PageTitle" },
                values: new object[,]
                {
                    { 1, ".//div[@class='p-main-contents ']", false, true, "https://www.yomiuri.co.jp/", ".//div[@class='news-top-latest']", "https://www.yomiuri.co.jp/news/", "yomiuri" },
                    { 2, ".//article[@class='articleDetail-content']", false, false, "https://www.nishinippon.co.jp/", ".//div[@class='c-articleList']", "https://www.nishinippon.co.jp/theme/easy_japanese/", "nishinippon" },
                    { 3, ".//div[@id='novel_honbun']", true, true, "https://ncode.syosetu.com/", ".//div[@id='main_search']", "https://yomou.syosetu.com/search.php?word=&notword=&genre=&type=t&mintime=&maxtime=&minlen=&maxlen=&min_globalpoint=&max_globalpoint=&minlastup=&maxlastup=&minfirstup=&maxfirstup=&order=hyoka", "syosetu" },
                    { 4, ".//section[@class='module--detail-content']", false, false, "https://www3.nhk.or.jp", ".//main[@id='main']", "https://www3.nhk.or.jp/news/catnew.html", "nhk news" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Articles_PageSourceId",
                table: "Articles",
                column: "PageSourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_PageSources_PageSourceId",
                table: "Articles",
                column: "PageSourceId",
                principalTable: "PageSources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_PageSources_PageSourceId",
                table: "Articles");

            migrationBuilder.DropTable(
                name: "PageSources");

            migrationBuilder.DropIndex(
                name: "IX_Articles_PageSourceId",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "PageSourceId",
                table: "Articles");

            migrationBuilder.RenameColumn(
                name: "Link",
                table: "Articles",
                newName: "Description");

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "PublicationDate",
                table: "Articles",
                type: "datetime2",
                nullable: true);
        }
    }
}
