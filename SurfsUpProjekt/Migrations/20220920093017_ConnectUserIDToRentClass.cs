using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurfsUpProjekt.Migrations
{
    public partial class ConnectUserIDToRentClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserID",
                table: "Rent",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Rent");
        }
    }
}
