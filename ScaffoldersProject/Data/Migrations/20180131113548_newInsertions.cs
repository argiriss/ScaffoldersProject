using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ScaffoldersProject.Data.Migrations
{
    public partial class newInsertions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ask",
                columns: table => new
                {
                    AskId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateofAsk = table.Column<DateTime>(nullable: false),
                    IsMatched = table.Column<bool>(nullable: false),
                    PriceAsk = table.Column<decimal>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    Quantity = table.Column<double>(nullable: false),
                    UserAskId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ask", x => x.AskId);
                    table.ForeignKey(
                        name: "FK_Ask_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Offer",
                columns: table => new
                {
                    OfferId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateofOffer = table.Column<DateTime>(nullable: false),
                    IsMatched = table.Column<bool>(nullable: false),
                    PriceOffer = table.Column<decimal>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    Quantity = table.Column<double>(nullable: false),
                    UserOfferId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offer", x => x.OfferId);
                    table.ForeignKey(
                        name: "FK_Offer_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ask_ProductId",
                table: "Ask",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Offer_ProductId",
                table: "Offer",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ask");

            migrationBuilder.DropTable(
                name: "Offer");
        }
    }
}
