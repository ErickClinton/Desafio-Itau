using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesafioInvestimentosItau.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAssetEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_position_asset_id",
                table: "position");

            migrationBuilder.DropForeignKey(
                name: "fk_quote_asset_id",
                table: "quote");

            migrationBuilder.DropForeignKey(
                name: "fk_trade_asset_id",
                table: "trade");

            migrationBuilder.DropTable(
                name: "asset");

            migrationBuilder.DropIndex(
                name: "IX_trade_asset_id",
                table: "trade");

            migrationBuilder.DropIndex(
                name: "IX_quote_asset_id",
                table: "quote");

            migrationBuilder.DropIndex(
                name: "IX_position_asset_id",
                table: "position");

            migrationBuilder.DropColumn(
                name: "asset_id",
                table: "trade");

            migrationBuilder.DropColumn(
                name: "asset_id",
                table: "quote");

            migrationBuilder.DropColumn(
                name: "asset_id",
                table: "position");

            migrationBuilder.AddColumn<string>(
                name: "asset_code",
                table: "trade",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "asset_code",
                table: "quote",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "asset_code",
                table: "position",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "asset_code",
                table: "trade");

            migrationBuilder.DropColumn(
                name: "asset_code",
                table: "quote");

            migrationBuilder.DropColumn(
                name: "asset_code",
                table: "position");

            migrationBuilder.AddColumn<long>(
                name: "asset_id",
                table: "trade",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "asset_id",
                table: "quote",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "asset_id",
                table: "position",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "asset",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    code = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_asset", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_trade_asset_id",
                table: "trade",
                column: "asset_id");

            migrationBuilder.CreateIndex(
                name: "IX_quote_asset_id",
                table: "quote",
                column: "asset_id");

            migrationBuilder.CreateIndex(
                name: "IX_position_asset_id",
                table: "position",
                column: "asset_id");

            migrationBuilder.AddForeignKey(
                name: "fk_position_asset_id",
                table: "position",
                column: "asset_id",
                principalTable: "asset",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_quote_asset_id",
                table: "quote",
                column: "asset_id",
                principalTable: "asset",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_trade_asset_id",
                table: "trade",
                column: "asset_id",
                principalTable: "asset",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
