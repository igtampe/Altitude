using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Igtampe.Altitude.Data.Migrations
{
    public partial class SharingAndCustomization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Public",
                table: "Trip",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Event",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Icon",
                table: "Event",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TripShareData",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    TripID = table.Column<Guid>(type: "uuid", nullable: true),
                    Username = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripShareData", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TripShareData_Trip_TripID",
                        column: x => x.TripID,
                        principalTable: "Trip",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_TripShareData_User_Username",
                        column: x => x.Username,
                        principalTable: "User",
                        principalColumn: "Username");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TripShareData_TripID",
                table: "TripShareData",
                column: "TripID");

            migrationBuilder.CreateIndex(
                name: "IX_TripShareData_Username",
                table: "TripShareData",
                column: "Username");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TripShareData");

            migrationBuilder.DropColumn(
                name: "Public",
                table: "Trip");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "Event");
        }
    }
}
