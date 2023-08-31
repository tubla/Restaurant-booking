using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantBookingApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatedforeignkeyfieldname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RestaurantId",
                table: "DiningTables");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RestaurantId",
                table: "DiningTables",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
