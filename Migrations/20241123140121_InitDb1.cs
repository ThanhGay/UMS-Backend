using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class InitDb1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LopHP_Room_LopHP_LopHpId",
                table: "LopHP_Room");

            migrationBuilder.AddForeignKey(
                name: "FK_LopHP_Room_LopHP_LopHpId",
                table: "LopHP_Room",
                column: "LopHpId",
                principalTable: "LopHP",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LopHP_Room_LopHP_LopHpId",
                table: "LopHP_Room");

            migrationBuilder.AddForeignKey(
                name: "FK_LopHP_Room_LopHP_LopHpId",
                table: "LopHP_Room",
                column: "LopHpId",
                principalTable: "LopHP",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
