using Microsoft.EntityFrameworkCore.Migrations;

namespace SMGApp.EntityFramework.Migrations
{
    public partial class AddCostToServiceItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "ServiceItems",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "ServiceItems");
        }
    }
}
