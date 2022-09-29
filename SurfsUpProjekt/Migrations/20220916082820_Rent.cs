using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurfsUpProjekt.Migrations
{
    public partial class Rent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rent",
                columns: table => new
                {
                    BoardId = table.Column<int>(type: "int", nullable: false),
                    StartRent = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndRent = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rent", x => x.BoardId);
                    table.ForeignKey(
                        name: "FK_Rent_Board_BoardId",
                        column: x => x.BoardId,
                        principalTable: "Board",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rent");
        }
    }
}
