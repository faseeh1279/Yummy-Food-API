using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yummy_Food_API.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedDatabaseUserProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Complaints_AdminProfiles_AdminProfileId",
                table: "Complaints");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerProfiles_AdminProfiles_AdminProfileId",
                table: "CustomerProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemCategories_AdminProfiles_AdminProfileId",
                table: "ItemCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_AdminProfiles_AdminProfileId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AdminProfiles_AdminProfileId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_AdminProfiles_AdminProfileId",
                table: "RefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_RiderProfiles_AdminProfiles_AdminProfileId",
                table: "RiderProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_RiderProfiles_Users_UserId",
                table: "RiderProfiles");

            migrationBuilder.DropIndex(
                name: "IX_RiderProfiles_AdminProfileId",
                table: "RiderProfiles");

            migrationBuilder.DropIndex(
                name: "IX_RefreshTokens_AdminProfileId",
                table: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_Orders_AdminProfileId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Items_AdminProfileId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_ItemCategories_AdminProfileId",
                table: "ItemCategories");

            migrationBuilder.DropIndex(
                name: "IX_CustomerProfiles_AdminProfileId",
                table: "CustomerProfiles");

            migrationBuilder.DropIndex(
                name: "IX_CustomerProfiles_UserId",
                table: "CustomerProfiles");

            migrationBuilder.DropIndex(
                name: "IX_Complaints_AdminProfileId",
                table: "Complaints");

            migrationBuilder.DropIndex(
                name: "IX_AdminProfiles_UserId",
                table: "AdminProfiles");

            migrationBuilder.DropColumn(
                name: "AdminProfileId",
                table: "RiderProfiles");

            migrationBuilder.DropColumn(
                name: "AdminProfileId",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "AdminProfileId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "AdminProfileId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "AdminProfileId",
                table: "ItemCategories");

            migrationBuilder.DropColumn(
                name: "AdminProfileId",
                table: "CustomerProfiles");

            migrationBuilder.DropColumn(
                name: "AdminProfileId",
                table: "Complaints");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId1",
                table: "RiderProfiles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_RiderProfiles_UserId1",
                table: "RiderProfiles",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProfiles_UserId",
                table: "CustomerProfiles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AdminProfiles_UserId",
                table: "AdminProfiles",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RiderProfiles_Users_UserId1",
                table: "RiderProfiles",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RiderProfiles_Users_UserId1",
                table: "RiderProfiles");

            migrationBuilder.DropIndex(
                name: "IX_RiderProfiles_UserId1",
                table: "RiderProfiles");

            migrationBuilder.DropIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_CustomerProfiles_UserId",
                table: "CustomerProfiles");

            migrationBuilder.DropIndex(
                name: "IX_AdminProfiles_UserId",
                table: "AdminProfiles");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "RiderProfiles");

            migrationBuilder.AddColumn<Guid>(
                name: "AdminProfileId",
                table: "RiderProfiles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AdminProfileId",
                table: "RefreshTokens",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AdminProfileId",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AdminProfileId",
                table: "Items",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AdminProfileId",
                table: "ItemCategories",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AdminProfileId",
                table: "CustomerProfiles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AdminProfileId",
                table: "Complaints",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RiderProfiles_AdminProfileId",
                table: "RiderProfiles",
                column: "AdminProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_AdminProfileId",
                table: "RefreshTokens",
                column: "AdminProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AdminProfileId",
                table: "Orders",
                column: "AdminProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_AdminProfileId",
                table: "Items",
                column: "AdminProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemCategories_AdminProfileId",
                table: "ItemCategories",
                column: "AdminProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProfiles_AdminProfileId",
                table: "CustomerProfiles",
                column: "AdminProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProfiles_UserId",
                table: "CustomerProfiles",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_AdminProfileId",
                table: "Complaints",
                column: "AdminProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_AdminProfiles_UserId",
                table: "AdminProfiles",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_AdminProfiles_AdminProfileId",
                table: "Complaints",
                column: "AdminProfileId",
                principalTable: "AdminProfiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerProfiles_AdminProfiles_AdminProfileId",
                table: "CustomerProfiles",
                column: "AdminProfileId",
                principalTable: "AdminProfiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemCategories_AdminProfiles_AdminProfileId",
                table: "ItemCategories",
                column: "AdminProfileId",
                principalTable: "AdminProfiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_AdminProfiles_AdminProfileId",
                table: "Items",
                column: "AdminProfileId",
                principalTable: "AdminProfiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AdminProfiles_AdminProfileId",
                table: "Orders",
                column: "AdminProfileId",
                principalTable: "AdminProfiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_AdminProfiles_AdminProfileId",
                table: "RefreshTokens",
                column: "AdminProfileId",
                principalTable: "AdminProfiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RiderProfiles_AdminProfiles_AdminProfileId",
                table: "RiderProfiles",
                column: "AdminProfileId",
                principalTable: "AdminProfiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RiderProfiles_Users_UserId",
                table: "RiderProfiles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
