using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });
            
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Name" }, // Specify columns, including the Id for seed data
                values: new object[,]
                {
                    { 1, "Laptop Pro 15 inch" },
                    { 2, "Wireless Mouse ergonomic" },
                    { 3, "Mechanical Keyboard RGB" },
                    { 4, "4K Monitor 27 inch" },
                    { 5, "USB-C Hub 7-in-1" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
