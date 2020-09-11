using Microsoft.EntityFrameworkCore.Migrations;

namespace SMGApp.EntityFramework.Migrations
{
    public partial class AddedIMEI : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductIMEI",
                table: "Guarantees",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductIMEI",
                table: "Guarantees");
        }
    }
}
