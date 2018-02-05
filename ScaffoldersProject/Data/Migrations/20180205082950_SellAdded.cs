using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ScaffoldersProject.Data.Migrations
{
    public partial class SellAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sell",
                columns: table => new
                {
                    SellId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SellDay = table.Column<DateTime>(nullable: false),
                    UserSellId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sell", x => x.SellId);
                });

            migrationBuilder.CreateTable(
                name: "CartSell",
                columns: table => new
                {
                    CartSellId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProductId = table.Column<int>(nullable: false),
                    Quantity = table.Column<decimal>(nullable: false),
                    SellId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartSell", x => x.CartSellId);
                    table.ForeignKey(
                        name: "FK_CartSell_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartSell_Sell_SellId",
                        column: x => x.SellId,
                        principalTable: "Sell",
                        principalColumn: "SellId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartSell_ProductId",
                table: "CartSell",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CartSell_SellId",
                table: "CartSell",
                column: "SellId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartSell");

            migrationBuilder.DropTable(
                name: "Sell");
        }
    }
}
