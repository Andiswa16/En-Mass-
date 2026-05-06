using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnMasse.Migrations
{
    /// <inheritdoc />
    public partial class AddDriverRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DriverID",
                table: "Deliveries",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Drivers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DUsername = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DPassword = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drivers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_DriverID",
                table: "Deliveries",
                column: "DriverID");

            migrationBuilder.AddForeignKey(
                name: "FK_Deliveries_Drivers_DriverID",
                table: "Deliveries",
                column: "DriverID",
                principalTable: "Drivers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deliveries_Drivers_DriverID",
                table: "Deliveries");

            migrationBuilder.DropTable(
                name: "Drivers");

            migrationBuilder.DropIndex(
                name: "IX_Deliveries_DriverID",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "DriverID",
                table: "Deliveries");
        }
    }
}
