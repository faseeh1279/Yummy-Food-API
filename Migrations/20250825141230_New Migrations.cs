using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yummy_Food_API.Migrations
{
    /// <inheritdoc />
    public partial class NewMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemImages_Items_ItemId",
                table: "ItemImages");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "ItemImages",
                newName: "FilePath");

            migrationBuilder.RenameColumn(
                name: "ContentType",
                table: "ItemImages",
                newName: "FileExtension");

            migrationBuilder.AlterColumn<Guid>(
                name: "ItemId",
                table: "ItemImages",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<string>(
                name: "FileDescription",
                table: "ItemImages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "FileSizeInBytes",
                table: "ItemImages",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemImages_Items_ItemId",
                table: "ItemImages",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemImages_Items_ItemId",
                table: "ItemImages");

            migrationBuilder.DropColumn(
                name: "FileDescription",
                table: "ItemImages");

            migrationBuilder.DropColumn(
                name: "FileSizeInBytes",
                table: "ItemImages");

            migrationBuilder.RenameColumn(
                name: "FilePath",
                table: "ItemImages",
                newName: "Url");

            migrationBuilder.RenameColumn(
                name: "FileExtension",
                table: "ItemImages",
                newName: "ContentType");

            migrationBuilder.AlterColumn<Guid>(
                name: "ItemId",
                table: "ItemImages",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemImages_Items_ItemId",
                table: "ItemImages",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
