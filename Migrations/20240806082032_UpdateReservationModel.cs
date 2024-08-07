using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagement.Migrations
{
    /// <inheritdoc />
    public partial class UpdateReservationModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MessageId",
                table: "Reservations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_MessageId",
                table: "Reservations",
                column: "MessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Messages_MessageId",
                table: "Reservations",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "MessageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Messages_MessageId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_MessageId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "MessageId",
                table: "Reservations");
        }
    }
}
