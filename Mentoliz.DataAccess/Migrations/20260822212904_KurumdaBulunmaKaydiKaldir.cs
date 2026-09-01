using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mentoliz.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class KurumdaBulunmaKaydiKaldir : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KurumdaBulunmaKayitlari");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KurumdaBulunmaKayitlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OgrenciId = table.Column<int>(type: "INTEGER", nullable: false),
                    Aciklama = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    ElleGirilenCikisSaati = table.Column<TimeSpan>(type: "TEXT", nullable: true),
                    ElleGirilenGirisSaati = table.Column<TimeSpan>(type: "TEXT", nullable: true),
                    GuncellemeTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SilindiMi = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    Tarih = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KurumdaBulunmaKayitlari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KurumdaBulunmaKayitlari_Ogrenciler_OgrenciId",
                        column: x => x.OgrenciId,
                        principalTable: "Ogrenciler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KurumdaBulunmaKayitlari_OgrenciId",
                table: "KurumdaBulunmaKayitlari",
                column: "OgrenciId");
        }
    }
}
