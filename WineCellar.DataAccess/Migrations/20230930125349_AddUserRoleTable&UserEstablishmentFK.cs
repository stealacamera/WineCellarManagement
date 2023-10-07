using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WineCellar.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddUserRoleTableUserEstablishmentFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EstablishmentId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_EstablishmentId",
                table: "AspNetUsers",
                column: "EstablishmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Establishment_EstablishmentId",
                table: "AspNetUsers",
                column: "EstablishmentId",
                principalTable: "Establishment",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Establishment_EstablishmentId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_EstablishmentId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EstablishmentId",
                table: "AspNetUsers");
        }
    }
}
