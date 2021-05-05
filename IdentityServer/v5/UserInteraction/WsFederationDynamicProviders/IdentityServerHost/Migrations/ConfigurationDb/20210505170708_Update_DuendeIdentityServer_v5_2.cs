using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityServerHost.Migrations.ConfigurationDb
{
    public partial class Update_DuendeIdentityServer_v5_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OidcIdentityProviders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Scheme = table.Column<string>(maxLength: 200, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 200, nullable: true),
                    Enabled = table.Column<bool>(nullable: false),
                    Type = table.Column<string>(maxLength: 20, nullable: false),
                    Authority = table.Column<string>(maxLength: 400, nullable: true),
                    ResponseType = table.Column<string>(maxLength: 20, nullable: true),
                    ClientId = table.Column<string>(maxLength: 100, nullable: true),
                    ClientSecret = table.Column<string>(maxLength: 200, nullable: true),
                    Scope = table.Column<string>(maxLength: 400, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OidcIdentityProviders", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OidcIdentityProviders");
        }
    }
}
