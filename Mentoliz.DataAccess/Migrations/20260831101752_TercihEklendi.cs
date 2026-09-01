using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mentoliz.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class TercihEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tercihler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OgrenciId = table.Column<int>(type: "INTEGER", nullable: false),
                    Sira = table.Column<int>(type: "INTEGER", nullable: false),
                    UniversiteAdi = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    BolumAdi = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    TabanPuan = table.Column<decimal>(type: "TEXT", nullable: true),
                    Notlar = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    OlusturmaTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SilindiMi = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tercihler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tercihler_Ogrenciler_OgrenciId",
                        column: x => x.OgrenciId,
                        principalTable: "Ogrenciler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tercihler_OgrenciId",
                table: "Tercihler",
                column: "OgrenciId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tercihler");
        }
    }
}
