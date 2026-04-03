using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yummy_Food_API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate1122 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "OrderItems",
                newName: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "OrderItems",
                newName: "CustomerId");
        }
    }
}
