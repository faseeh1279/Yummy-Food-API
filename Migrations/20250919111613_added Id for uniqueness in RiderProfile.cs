using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yummy_Food_API.Migrations
{
    /// <inheritdoc />
    public partial class addedIdforuniquenessinRiderProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Complaints_RiderProfiles_RiderProfileId",
                table: "Complaints");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_RiderProfiles_RiderProfileId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_RiderProfiles_Users_UserId1",
                table: "RiderProfiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RiderProfiles",
                table: "RiderProfiles");

            migrationBuilder.DropIndex(
                name: "IX_RiderProfiles_UserId1",
                table: "RiderProfiles");

            migrationBuilder.RenameColumn(
                name: "UserId1",
                table: "RiderProfiles",
                newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RiderProfiles",
                table: "RiderProfiles",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_RiderProfiles_UserId",
                table: "RiderProfiles",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_RiderProfiles_RiderProfileId",
                table: "Complaints",
                column: "RiderProfileId",
                principalTable: "RiderProfiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_RiderProfiles_RiderProfileId",
                table: "Orders",
                column: "RiderProfileId",
                principalTable: "RiderProfiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RiderProfiles_Users_UserId",
                table: "RiderProfiles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Complaints_RiderProfiles_RiderProfileId",
                table: "Complaints");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_RiderProfiles_RiderProfileId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_RiderProfiles_Users_UserId",
                table: "RiderProfiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RiderProfiles",
                table: "RiderProfiles");

            migrationBuilder.DropIndex(
                name: "IX_RiderProfiles_UserId",
                table: "RiderProfiles");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "RiderProfiles",
                newName: "UserId1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RiderProfiles",
                table: "RiderProfiles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RiderProfiles_UserId1",
                table: "RiderProfiles",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_RiderProfiles_RiderProfileId",
                table: "Complaints",
                column: "RiderProfileId",
                principalTable: "RiderProfiles",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_RiderProfiles_RiderProfileId",
                table: "Orders",
                column: "RiderProfileId",
                principalTable: "RiderProfiles",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RiderProfiles_Users_UserId1",
                table: "RiderProfiles",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
