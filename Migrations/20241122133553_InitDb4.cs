using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class InitDb4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LopHP_Student_LopHP_LopHpId",
                table: "LopHP_Student");

            migrationBuilder.DropForeignKey(
                name: "FK_LopHP_Teacher_LopHP_LopHpId",
                table: "LopHP_Teacher");

            migrationBuilder.AddForeignKey(
                name: "FK_LopHP_Student_LopHP_LopHpId",
                table: "LopHP_Student",
                column: "LopHpId",
                principalTable: "LopHP",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LopHP_Teacher_LopHP_LopHpId",
                table: "LopHP_Teacher",
                column: "LopHpId",
                principalTable: "LopHP",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LopHP_Student_LopHP_LopHpId",
                table: "LopHP_Student");

            migrationBuilder.DropForeignKey(
                name: "FK_LopHP_Teacher_LopHP_LopHpId",
                table: "LopHP_Teacher");

            migrationBuilder.AddForeignKey(
                name: "FK_LopHP_Student_LopHP_LopHpId",
                table: "LopHP_Student",
                column: "LopHpId",
                principalTable: "LopHP",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LopHP_Teacher_LopHP_LopHpId",
                table: "LopHP_Teacher",
                column: "LopHpId",
                principalTable: "LopHP",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
