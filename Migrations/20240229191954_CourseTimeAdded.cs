using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Escape.Migrations
{
    public partial class CourseTimeAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "CourseTime",
                table: "Courses",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourseTime",
                table: "Courses");
        }
    }
}
