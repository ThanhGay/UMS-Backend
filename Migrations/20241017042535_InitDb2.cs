using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class InitDb2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LopHP_Student",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LopHpId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiemCC = table.Column<double>(type: "float", nullable: false),
                    DiemGK = table.Column<double>(type: "float", nullable: false),
                    DiemCK = table.Column<double>(type: "float", nullable: false),
                    DiemKT = table.Column<double>(type: "float", nullable: true),
                    IsSaved = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LopHP_Student", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LopHP_Student_LopHP_LopHpId",
                        column: x => x.LopHpId,
                        principalTable: "LopHP",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LopHP_Student_LopHpId",
                table: "LopHP_Student",
                column: "LopHpId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LopHP_Student");
        }
    }
}
