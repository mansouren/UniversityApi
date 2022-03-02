using Microsoft.EntityFrameworkCore.Migrations;

namespace UniversityApi.Data.Migrations
{
    public partial class Edit_TblUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsStudent",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsTeacher",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsStudent",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsTeacher",
                table: "Users");
        }
    }
}
