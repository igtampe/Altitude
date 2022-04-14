using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Igtampe.Altitude.Data.Migrations
{
    public partial class DayColorIcon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Day",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Icon",
                table: "Day",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "Day");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "Day");
        }
    }
}
