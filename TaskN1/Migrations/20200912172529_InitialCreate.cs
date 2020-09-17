using Microsoft.EntityFrameworkCore.Migrations;
namespace TaskN1.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Surname = table.Column<string>(nullable: true),
                    Sex = table.Column<string>(nullable: true),
                    PersonalID = table.Column<string>(nullable: true),
                    BirthDate = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Mobile = table.Column<string>(nullable: true),
                    Picture = table.Column<string>(nullable: true),
                    ConnectedPeople = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Person");
        }
    }
}
