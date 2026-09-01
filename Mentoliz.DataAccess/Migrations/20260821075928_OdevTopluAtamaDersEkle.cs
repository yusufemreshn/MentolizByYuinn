using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mentoliz.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class OdevTopluAtamaDersEkle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DersId",
                table: "OdevTopluAtamalari",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OdevTopluAtamalari_DersId",
                table: "OdevTopluAtamalari",
                column: "DersId");

            migrationBuilder.AddForeignKey(
                name: "FK_OdevTopluAtamalari_Dersler_DersId",
                table: "OdevTopluAtamalari",
                column: "DersId",
                principalTable: "Dersler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OdevTopluAtamalari_Dersler_DersId",
                table: "OdevTopluAtamalari");

            migrationBuilder.DropIndex(
                name: "IX_OdevTopluAtamalari_DersId",
                table: "OdevTopluAtamalari");

            migrationBuilder.DropColumn(
                name: "DersId",
                table: "OdevTopluAtamalari");
        }
    }
}
