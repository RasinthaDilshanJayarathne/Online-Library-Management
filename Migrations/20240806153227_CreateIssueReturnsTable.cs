using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagement.Migrations
{
    /// <inheritdoc />
    public partial class CreateIssueReturnsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IssueReturn_Items_Item_Id",
                table: "IssueReturn");

            migrationBuilder.DropForeignKey(
                name: "FK_IssueReturn_Users_UserId",
                table: "IssueReturn");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IssueReturn",
                table: "IssueReturn");

            migrationBuilder.RenameTable(
                name: "IssueReturn",
                newName: "IssueReturns");

            migrationBuilder.RenameIndex(
                name: "IX_IssueReturn_UserId",
                table: "IssueReturns",
                newName: "IX_IssueReturns_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_IssueReturn_Item_Id",
                table: "IssueReturns",
                newName: "IX_IssueReturns_Item_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IssueReturns",
                table: "IssueReturns",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_IssueReturns_Items_Item_Id",
                table: "IssueReturns",
                column: "Item_Id",
                principalTable: "Items",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IssueReturns_Users_UserId",
                table: "IssueReturns",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IssueReturns_Items_Item_Id",
                table: "IssueReturns");

            migrationBuilder.DropForeignKey(
                name: "FK_IssueReturns_Users_UserId",
                table: "IssueReturns");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IssueReturns",
                table: "IssueReturns");

            migrationBuilder.RenameTable(
                name: "IssueReturns",
                newName: "IssueReturn");

            migrationBuilder.RenameIndex(
                name: "IX_IssueReturns_UserId",
                table: "IssueReturn",
                newName: "IX_IssueReturn_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_IssueReturns_Item_Id",
                table: "IssueReturn",
                newName: "IX_IssueReturn_Item_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IssueReturn",
                table: "IssueReturn",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_IssueReturn_Items_Item_Id",
                table: "IssueReturn",
                column: "Item_Id",
                principalTable: "Items",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IssueReturn_Users_UserId",
                table: "IssueReturn",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
