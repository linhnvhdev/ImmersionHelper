using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImmersionHelper.Data.Migrations
{
    public partial class PostTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PostTime",
                table: "Posts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostTime",
                table: "Posts");
        }
    }
}
