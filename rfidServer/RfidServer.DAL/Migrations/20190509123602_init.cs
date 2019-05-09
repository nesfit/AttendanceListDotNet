using Microsoft.EntityFrameworkCore.Migrations;

namespace RfidServer.DAL.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Variants",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WisId = table.Column<int>(nullable: false),
                    WisItemId = table.Column<int>(nullable: false),
                    WisCourseId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Points = table.Column<int>(nullable: false),
                    Limit = table.Column<int>(nullable: false),
                    CourseAbbrv = table.Column<string>(nullable: true),
                    Year = table.Column<int>(nullable: false),
                    Sem = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Variants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WisId = table.Column<int>(nullable: false),
                    WisPersonId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Login = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Points = table.Column<int>(nullable: false),
                    Who = table.Column<string>(nullable: true),
                    RegType = table.Column<string>(nullable: true),
                    Date = table.Column<string>(nullable: true),
                    RegTime = table.Column<string>(nullable: true),
                    Update = table.Column<string>(nullable: true),
                    Registered = table.Column<bool>(nullable: false),
                    VariantId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Students_Variants_VariantId",
                        column: x => x.VariantId,
                        principalTable: "Variants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Students_VariantId",
                table: "Students",
                column: "VariantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Variants");
        }
    }
}
