using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Mentoliz.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class IlkOlusturma : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ayarlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Anahtar = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Deger = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SilindiMi = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ayarlar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Dersler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Ad = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    KisaAd = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Kategori = table.Column<int>(type: "INTEGER", nullable: false),
                    Sira = table.Column<int>(type: "INTEGER", nullable: false),
                    AktifMi = table.Column<bool>(type: "INTEGER", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SilindiMi = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dersler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SinavTurleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Ad = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Kod = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Sira = table.Column<int>(type: "INTEGER", nullable: false),
                    AktifMi = table.Column<bool>(type: "INTEGER", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SilindiMi = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SinavTurleri", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Siniflar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Ad = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Seviye = table.Column<int>(type: "INTEGER", nullable: false),
                    Alan = table.Column<int>(type: "INTEGER", nullable: false),
                    Kontenjan = table.Column<int>(type: "INTEGER", nullable: false),
                    AktifMi = table.Column<bool>(type: "INTEGER", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SilindiMi = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Siniflar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Konular",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DersId = table.Column<int>(type: "INTEGER", nullable: false),
                    Ad = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Seviye = table.Column<int>(type: "INTEGER", nullable: false),
                    Sira = table.Column<int>(type: "INTEGER", nullable: false),
                    AktifMi = table.Column<bool>(type: "INTEGER", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SilindiMi = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Konular", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Konular_Dersler_DersId",
                        column: x => x.DersId,
                        principalTable: "Dersler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Denemeler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Ad = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Tarih = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SinavTuruId = table.Column<int>(type: "INTEGER", nullable: false),
                    YayinAdi = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Aciklama = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    OlusturmaTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SilindiMi = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Denemeler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Denemeler_SinavTurleri_SinavTuruId",
                        column: x => x.SinavTuruId,
                        principalTable: "SinavTurleri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SinavTuruTestleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SinavTuruId = table.Column<int>(type: "INTEGER", nullable: false),
                    DersId = table.Column<int>(type: "INTEGER", nullable: false),
                    SoruSayisi = table.Column<int>(type: "INTEGER", nullable: false),
                    Sira = table.Column<int>(type: "INTEGER", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SilindiMi = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SinavTuruTestleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SinavTuruTestleri_Dersler_DersId",
                        column: x => x.DersId,
                        principalTable: "Dersler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SinavTuruTestleri_SinavTurleri_SinavTuruId",
                        column: x => x.SinavTuruId,
                        principalTable: "SinavTurleri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DersProgramiSatirlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SinifId = table.Column<int>(type: "INTEGER", nullable: false),
                    Gun = table.Column<int>(type: "INTEGER", nullable: false),
                    BaslangicSaati = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    BitisSaati = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    DersId = table.Column<int>(type: "INTEGER", nullable: false),
                    DerslikAdi = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    OgretmenAdi = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    OlusturmaTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SilindiMi = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DersProgramiSatirlari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DersProgramiSatirlari_Dersler_DersId",
                        column: x => x.DersId,
                        principalTable: "Dersler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DersProgramiSatirlari_Siniflar_SinifId",
                        column: x => x.SinifId,
                        principalTable: "Siniflar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OdevTopluAtamalari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SinifId = table.Column<int>(type: "INTEGER", nullable: false),
                    Baslik = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    VerilisTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SonTeslimTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SilindiMi = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OdevTopluAtamalari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OdevTopluAtamalari_Siniflar_SinifId",
                        column: x => x.SinifId,
                        principalTable: "Siniflar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ogrenciler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Ad = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Soyad = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Telefon = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    OgrenciNo = table.Column<string>(type: "TEXT", maxLength: 30, nullable: true),
                    SinifId = table.Column<int>(type: "INTEGER", nullable: true),
                    DogumTarihi = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Cinsiyet = table.Column<int>(type: "INTEGER", nullable: false),
                    OkulAdi = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Alan = table.Column<int>(type: "INTEGER", nullable: false),
                    KayitTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AktifMi = table.Column<bool>(type: "INTEGER", nullable: false),
                    FotografYolu = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    GenelNotlar = table.Column<string>(type: "TEXT", maxLength: 4000, nullable: true),
                    OlusturmaTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SilindiMi = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ogrenciler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ogrenciler_Siniflar_SinifId",
                        column: x => x.SinifId,
                        principalTable: "Siniflar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "CalismaProgramlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OgrenciId = table.Column<int>(type: "INTEGER", nullable: false),
                    HaftaBaslangicTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Aciklama = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    OlusturmaTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SilindiMi = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalismaProgramlari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalismaProgramlari_Ogrenciler_OgrenciId",
                        column: x => x.OgrenciId,
                        principalTable: "Ogrenciler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DenemeSonuclari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DenemeId = table.Column<int>(type: "INTEGER", nullable: false),
                    OgrenciId = table.Column<int>(type: "INTEGER", nullable: false),
                    Notlar = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    KatilmadiMi = table.Column<bool>(type: "INTEGER", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SilindiMi = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DenemeSonuclari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DenemeSonuclari_Denemeler_DenemeId",
                        column: x => x.DenemeId,
                        principalTable: "Denemeler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DenemeSonuclari_Ogrenciler_OgrenciId",
                        column: x => x.OgrenciId,
                        principalTable: "Ogrenciler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Gorevler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Baslik = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Aciklama = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    Tarih = table.Column<DateTime>(type: "TEXT", nullable: false),
                    OgrenciId = table.Column<int>(type: "INTEGER", nullable: true),
                    Oncelik = table.Column<int>(type: "INTEGER", nullable: false),
                    TamamlandiMi = table.Column<bool>(type: "INTEGER", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SilindiMi = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gorevler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Gorevler_Ogrenciler_OgrenciId",
                        column: x => x.OgrenciId,
                        principalTable: "Ogrenciler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Gorusmeler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OgrenciId = table.Column<int>(type: "INTEGER", nullable: false),
                    Tarih = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Tur = table.Column<int>(type: "INTEGER", nullable: false),
                    Konu = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Notlar = table.Column<string>(type: "TEXT", maxLength: 4000, nullable: false),
                    AlinanKarar = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    SonrakiGorusmeTarihi = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Sure = table.Column<int>(type: "INTEGER", nullable: true),
                    OlusturmaTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SilindiMi = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gorusmeler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Gorusmeler_Ogrenciler_OgrenciId",
                        column: x => x.OgrenciId,
                        principalTable: "Ogrenciler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Hedefler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OgrenciId = table.Column<int>(type: "INTEGER", nullable: false),
                    HedefUniversite = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    HedefBolum = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    PuanTuru = table.Column<int>(type: "INTEGER", nullable: false),
                    HedefSiralama = table.Column<int>(type: "INTEGER", nullable: true),
                    HedefToplamNet = table.Column<decimal>(type: "TEXT", nullable: true),
                    AktifMi = table.Column<bool>(type: "INTEGER", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SilindiMi = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hedefler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hedefler_Ogrenciler_OgrenciId",
                        column: x => x.OgrenciId,
                        principalTable: "Ogrenciler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KurumdaBulunmaKayitlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OgrenciId = table.Column<int>(type: "INTEGER", nullable: false),
                    Tarih = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ElleGirilenGirisSaati = table.Column<TimeSpan>(type: "TEXT", nullable: true),
                    ElleGirilenCikisSaati = table.Column<TimeSpan>(type: "TEXT", nullable: true),
                    Aciklama = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    OlusturmaTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SilindiMi = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false)
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

            migrationBuilder.CreateTable(
                name: "Odevler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OgrenciId = table.Column<int>(type: "INTEGER", nullable: false),
                    DersId = table.Column<int>(type: "INTEGER", nullable: false),
                    KonuBasligi = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    KaynakAdi = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    ToplamSoruSayisi = table.Column<int>(type: "INTEGER", nullable: true),
                    TamamlananSoruSayisi = table.Column<int>(type: "INTEGER", nullable: true),
                    VerilisTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SonTeslimTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    YapilmaTarihi = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Durum = table.Column<int>(type: "INTEGER", nullable: false),
                    Aciklama = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    TopluAtamaId = table.Column<int>(type: "INTEGER", nullable: true),
                    OlusturmaTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SilindiMi = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Odevler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Odevler_Dersler_DersId",
                        column: x => x.DersId,
                        principalTable: "Dersler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Odevler_OdevTopluAtamalari_TopluAtamaId",
                        column: x => x.TopluAtamaId,
                        principalTable: "OdevTopluAtamalari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Odevler_Ogrenciler_OgrenciId",
                        column: x => x.OgrenciId,
                        principalTable: "Ogrenciler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OgrenciKonuTakipleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OgrenciId = table.Column<int>(type: "INTEGER", nullable: false),
                    KonuId = table.Column<int>(type: "INTEGER", nullable: false),
                    Durum = table.Column<int>(type: "INTEGER", nullable: false),
                    Notlar = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    OlusturmaTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SilindiMi = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OgrenciKonuTakipleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OgrenciKonuTakipleri_Konular_KonuId",
                        column: x => x.KonuId,
                        principalTable: "Konular",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OgrenciKonuTakipleri_Ogrenciler_OgrenciId",
                        column: x => x.OgrenciId,
                        principalTable: "Ogrenciler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OgrenciProgramIstisnalari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OgrenciId = table.Column<int>(type: "INTEGER", nullable: false),
                    Gun = table.Column<int>(type: "INTEGER", nullable: false),
                    BaslangicSaati = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    BitisSaati = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    DersId = table.Column<int>(type: "INTEGER", nullable: true),
                    Tur = table.Column<int>(type: "INTEGER", nullable: false),
                    OgretmenAdi = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Aciklama = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    BaslangicTarihi = table.Column<DateTime>(type: "TEXT", nullable: true),
                    BitisTarihi = table.Column<DateTime>(type: "TEXT", nullable: true),
                    OlusturmaTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SilindiMi = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OgrenciProgramIstisnalari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OgrenciProgramIstisnalari_Dersler_DersId",
                        column: x => x.DersId,
                        principalTable: "Dersler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_OgrenciProgramIstisnalari_Ogrenciler_OgrenciId",
                        column: x => x.OgrenciId,
                        principalTable: "Ogrenciler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Veliler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OgrenciId = table.Column<int>(type: "INTEGER", nullable: false),
                    Yakinlik = table.Column<int>(type: "INTEGER", nullable: false),
                    Ad = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Soyad = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Telefon = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Meslek = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Eposta = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    BirincilIletisimMi = table.Column<bool>(type: "INTEGER", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SilindiMi = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Veliler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Veliler_Ogrenciler_OgrenciId",
                        column: x => x.OgrenciId,
                        principalTable: "Ogrenciler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CalismaProgramiSatirlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CalismaProgramiId = table.Column<int>(type: "INTEGER", nullable: false),
                    Gun = table.Column<int>(type: "INTEGER", nullable: false),
                    BaslangicSaati = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    BitisSaati = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    DersId = table.Column<int>(type: "INTEGER", nullable: true),
                    Aciklama = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    TamamlandiMi = table.Column<bool>(type: "INTEGER", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SilindiMi = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalismaProgramiSatirlari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalismaProgramiSatirlari_CalismaProgramlari_CalismaProgramiId",
                        column: x => x.CalismaProgramiId,
                        principalTable: "CalismaProgramlari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CalismaProgramiSatirlari_Dersler_DersId",
                        column: x => x.DersId,
                        principalTable: "Dersler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "DenemeSonucDetaylari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DenemeSonucId = table.Column<int>(type: "INTEGER", nullable: false),
                    SinavTuruTestId = table.Column<int>(type: "INTEGER", nullable: false),
                    Dogru = table.Column<int>(type: "INTEGER", nullable: false),
                    Yanlis = table.Column<int>(type: "INTEGER", nullable: false),
                    Bos = table.Column<int>(type: "INTEGER", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SilindiMi = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DenemeSonucDetaylari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DenemeSonucDetaylari_DenemeSonuclari_DenemeSonucId",
                        column: x => x.DenemeSonucId,
                        principalTable: "DenemeSonuclari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DenemeSonucDetaylari_SinavTuruTestleri_SinavTuruTestId",
                        column: x => x.SinavTuruTestId,
                        principalTable: "SinavTuruTestleri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HedefDersNetleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HedefId = table.Column<int>(type: "INTEGER", nullable: false),
                    DersId = table.Column<int>(type: "INTEGER", nullable: false),
                    HedefNet = table.Column<decimal>(type: "TEXT", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SilindiMi = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HedefDersNetleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HedefDersNetleri_Dersler_DersId",
                        column: x => x.DersId,
                        principalTable: "Dersler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HedefDersNetleri_Hedefler_HedefId",
                        column: x => x.HedefId,
                        principalTable: "Hedefler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Dersler",
                columns: new[] { "Id", "Ad", "AktifMi", "GuncellemeTarihi", "Kategori", "KisaAd", "OlusturmaTarihi", "Sira" },
                values: new object[,]
                {
                    { 1, "Türkçe", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "TR", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 2, "Matematik", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "MAT", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 3, "Sosyal Bilimler", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "SOS", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3 },
                    { 4, "Fen Bilimleri", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "FEN", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4 },
                    { 5, "Fizik", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "FIZ", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5 },
                    { 6, "Kimya", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "KIM", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6 },
                    { 7, "Biyoloji", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "BIY", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 7 },
                    { 8, "Türk Dili ve Edebiyatı", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "TDE", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8 },
                    { 9, "Tarih", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "TAR", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9 },
                    { 10, "Coğrafya", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "COG", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 10 },
                    { 11, "Tarih-2", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "TAR2", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 12, "Coğrafya-2", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "COG2", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 13, "Felsefe Grubu", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "FEL", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 14, "Din Kültürü ve Ahlak Bilgisi", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "DIN", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 14 },
                    { 15, "Yabancı Dil", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "YD", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 15 }
                });

            migrationBuilder.InsertData(
                table: "SinavTurleri",
                columns: new[] { "Id", "Ad", "AktifMi", "GuncellemeTarihi", "Kod", "OlusturmaTarihi", "Sira" },
                values: new object[,]
                {
                    { 1, "TYT", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "TYT", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 2, "AYT Sayısal", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "AYT_SAY", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 3, "AYT Eşit Ağırlık", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "AYT_EA", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3 },
                    { 4, "AYT Sözel", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "AYT_SOZ", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4 },
                    { 5, "YDT", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "YDT", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5 }
                });

            migrationBuilder.InsertData(
                table: "SinavTuruTestleri",
                columns: new[] { "Id", "DersId", "GuncellemeTarihi", "OlusturmaTarihi", "SinavTuruId", "Sira", "SoruSayisi" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, 40 },
                    { 2, 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, 40 },
                    { 3, 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 3, 20 },
                    { 4, 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 4, 20 },
                    { 5, 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 1, 40 },
                    { 6, 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 2, 14 },
                    { 7, 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 3, 13 },
                    { 8, 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 4, 13 },
                    { 9, 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 1, 40 },
                    { 10, 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 2, 24 },
                    { 11, 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 3, 10 },
                    { 12, 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 4, 6 },
                    { 13, 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 1, 24 },
                    { 14, 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 2, 10 },
                    { 15, 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 3, 6 },
                    { 16, 11, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 4, 11 },
                    { 17, 12, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 5, 11 },
                    { 18, 13, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 6, 12 },
                    { 19, 14, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 7, 6 },
                    { 20, 15, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 1, 80 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ayarlar_Anahtar",
                table: "Ayarlar",
                column: "Anahtar",
                unique: true,
                filter: "SilindiMi = 0");

            migrationBuilder.CreateIndex(
                name: "IX_CalismaProgramiSatirlari_CalismaProgramiId",
                table: "CalismaProgramiSatirlari",
                column: "CalismaProgramiId");

            migrationBuilder.CreateIndex(
                name: "IX_CalismaProgramiSatirlari_DersId",
                table: "CalismaProgramiSatirlari",
                column: "DersId");

            migrationBuilder.CreateIndex(
                name: "IX_CalismaProgramlari_OgrenciId",
                table: "CalismaProgramlari",
                column: "OgrenciId");

            migrationBuilder.CreateIndex(
                name: "IX_Denemeler_SinavTuruId",
                table: "Denemeler",
                column: "SinavTuruId");

            migrationBuilder.CreateIndex(
                name: "IX_DenemeSonucDetaylari_DenemeSonucId_SinavTuruTestId",
                table: "DenemeSonucDetaylari",
                columns: new[] { "DenemeSonucId", "SinavTuruTestId" },
                unique: true,
                filter: "SilindiMi = 0");

            migrationBuilder.CreateIndex(
                name: "IX_DenemeSonucDetaylari_SinavTuruTestId",
                table: "DenemeSonucDetaylari",
                column: "SinavTuruTestId");

            migrationBuilder.CreateIndex(
                name: "IX_DenemeSonuclari_DenemeId_OgrenciId",
                table: "DenemeSonuclari",
                columns: new[] { "DenemeId", "OgrenciId" },
                unique: true,
                filter: "SilindiMi = 0");

            migrationBuilder.CreateIndex(
                name: "IX_DenemeSonuclari_OgrenciId",
                table: "DenemeSonuclari",
                column: "OgrenciId");

            migrationBuilder.CreateIndex(
                name: "IX_DersProgramiSatirlari_DersId",
                table: "DersProgramiSatirlari",
                column: "DersId");

            migrationBuilder.CreateIndex(
                name: "IX_DersProgramiSatirlari_SinifId",
                table: "DersProgramiSatirlari",
                column: "SinifId");

            migrationBuilder.CreateIndex(
                name: "IX_Gorevler_OgrenciId",
                table: "Gorevler",
                column: "OgrenciId");

            migrationBuilder.CreateIndex(
                name: "IX_Gorusmeler_OgrenciId",
                table: "Gorusmeler",
                column: "OgrenciId");

            migrationBuilder.CreateIndex(
                name: "IX_HedefDersNetleri_DersId",
                table: "HedefDersNetleri",
                column: "DersId");

            migrationBuilder.CreateIndex(
                name: "IX_HedefDersNetleri_HedefId_DersId",
                table: "HedefDersNetleri",
                columns: new[] { "HedefId", "DersId" },
                unique: true,
                filter: "SilindiMi = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Hedefler_OgrenciId",
                table: "Hedefler",
                column: "OgrenciId");

            migrationBuilder.CreateIndex(
                name: "IX_Konular_DersId",
                table: "Konular",
                column: "DersId");

            migrationBuilder.CreateIndex(
                name: "IX_KurumdaBulunmaKayitlari_OgrenciId",
                table: "KurumdaBulunmaKayitlari",
                column: "OgrenciId");

            migrationBuilder.CreateIndex(
                name: "IX_Odevler_DersId",
                table: "Odevler",
                column: "DersId");

            migrationBuilder.CreateIndex(
                name: "IX_Odevler_OgrenciId",
                table: "Odevler",
                column: "OgrenciId");

            migrationBuilder.CreateIndex(
                name: "IX_Odevler_TopluAtamaId",
                table: "Odevler",
                column: "TopluAtamaId");

            migrationBuilder.CreateIndex(
                name: "IX_OdevTopluAtamalari_SinifId",
                table: "OdevTopluAtamalari",
                column: "SinifId");

            migrationBuilder.CreateIndex(
                name: "IX_OgrenciKonuTakipleri_KonuId",
                table: "OgrenciKonuTakipleri",
                column: "KonuId");

            migrationBuilder.CreateIndex(
                name: "IX_OgrenciKonuTakipleri_OgrenciId_KonuId",
                table: "OgrenciKonuTakipleri",
                columns: new[] { "OgrenciId", "KonuId" },
                unique: true,
                filter: "SilindiMi = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Ogrenciler_OgrenciNo",
                table: "Ogrenciler",
                column: "OgrenciNo",
                unique: true,
                filter: "OgrenciNo IS NOT NULL AND SilindiMi = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Ogrenciler_SinifId",
                table: "Ogrenciler",
                column: "SinifId");

            migrationBuilder.CreateIndex(
                name: "IX_OgrenciProgramIstisnalari_DersId",
                table: "OgrenciProgramIstisnalari",
                column: "DersId");

            migrationBuilder.CreateIndex(
                name: "IX_OgrenciProgramIstisnalari_OgrenciId",
                table: "OgrenciProgramIstisnalari",
                column: "OgrenciId");

            migrationBuilder.CreateIndex(
                name: "IX_SinavTurleri_Kod",
                table: "SinavTurleri",
                column: "Kod",
                unique: true,
                filter: "SilindiMi = 0");

            migrationBuilder.CreateIndex(
                name: "IX_SinavTuruTestleri_DersId",
                table: "SinavTuruTestleri",
                column: "DersId");

            migrationBuilder.CreateIndex(
                name: "IX_SinavTuruTestleri_SinavTuruId",
                table: "SinavTuruTestleri",
                column: "SinavTuruId");

            migrationBuilder.CreateIndex(
                name: "IX_Veliler_OgrenciId",
                table: "Veliler",
                column: "OgrenciId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ayarlar");

            migrationBuilder.DropTable(
                name: "CalismaProgramiSatirlari");

            migrationBuilder.DropTable(
                name: "DenemeSonucDetaylari");

            migrationBuilder.DropTable(
                name: "DersProgramiSatirlari");

            migrationBuilder.DropTable(
                name: "Gorevler");

            migrationBuilder.DropTable(
                name: "Gorusmeler");

            migrationBuilder.DropTable(
                name: "HedefDersNetleri");

            migrationBuilder.DropTable(
                name: "KurumdaBulunmaKayitlari");

            migrationBuilder.DropTable(
                name: "Odevler");

            migrationBuilder.DropTable(
                name: "OgrenciKonuTakipleri");

            migrationBuilder.DropTable(
                name: "OgrenciProgramIstisnalari");

            migrationBuilder.DropTable(
                name: "Veliler");

            migrationBuilder.DropTable(
                name: "CalismaProgramlari");

            migrationBuilder.DropTable(
                name: "DenemeSonuclari");

            migrationBuilder.DropTable(
                name: "SinavTuruTestleri");

            migrationBuilder.DropTable(
                name: "Hedefler");

            migrationBuilder.DropTable(
                name: "OdevTopluAtamalari");

            migrationBuilder.DropTable(
                name: "Konular");

            migrationBuilder.DropTable(
                name: "Denemeler");

            migrationBuilder.DropTable(
                name: "Ogrenciler");

            migrationBuilder.DropTable(
                name: "Dersler");

            migrationBuilder.DropTable(
                name: "SinavTurleri");

            migrationBuilder.DropTable(
                name: "Siniflar");
        }
    }
}
