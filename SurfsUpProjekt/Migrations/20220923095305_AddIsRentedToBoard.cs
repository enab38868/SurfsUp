using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurfsUpProjekt.Migrations
{
    public partial class AddIsRentedToBoard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRented",
                table: "Board",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRented",
                table: "Board");
        }
    }
}
