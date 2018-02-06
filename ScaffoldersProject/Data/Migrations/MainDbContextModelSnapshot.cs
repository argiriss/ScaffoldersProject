﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using ScaffoldersProject.Data;
using System;

namespace ScaffoldersProject.Data.Migrations
{
    [DbContext(typeof(MainDbContext))]
    partial class MainDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ScaffoldersProject.Models.Ask", b =>
                {
                    b.Property<int>("AskId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateofAsk");

                    b.Property<bool>("IsMatched");

                    b.Property<decimal>("PriceAsk");

                    b.Property<int>("ProductId");

                    b.Property<decimal>("Quantity");

                    b.Property<string>("UserAskId");

                    b.HasKey("AskId");

                    b.HasIndex("ProductId");

                    b.ToTable("Ask");
                });

            modelBuilder.Entity("ScaffoldersProject.Models.Cart", b =>
                {
                    b.Property<int>("CartId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ProductId");

                    b.Property<decimal>("Quantity");

                    b.Property<string>("UserCartId");

                    b.HasKey("CartId");

                    b.HasIndex("ProductId");

                    b.ToTable("Cart");
                });

            modelBuilder.Entity("ScaffoldersProject.Models.CartOrder", b =>
                {
                    b.Property<int>("CartOrderId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("OrderId");

                    b.Property<int>("ProductId");

                    b.Property<decimal>("Quantity");

                    b.HasKey("CartOrderId");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("CartOrder");
                });

            modelBuilder.Entity("ScaffoldersProject.Models.CartSell", b =>
                {
                    b.Property<int>("CartSellId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ProductId");

                    b.Property<decimal>("Quantity");

                    b.Property<int>("SellId");

                    b.HasKey("CartSellId");

                    b.HasIndex("ProductId");

                    b.HasIndex("SellId");

                    b.ToTable("CartSell");
                });

            modelBuilder.Entity("ScaffoldersProject.Models.Deposit", b =>
                {
                    b.Property<int>("DepositId")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("DepositAmount");

                    b.Property<DateTime>("DepositDate");

                    b.Property<string>("UserDepositId");

                    b.HasKey("DepositId");

                    b.ToTable("Deposit");
                });

            modelBuilder.Entity("ScaffoldersProject.Models.Offer", b =>
                {
                    b.Property<int>("OfferId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateofOffer");

                    b.Property<bool>("IsMatched");

                    b.Property<decimal>("PriceOffer");

                    b.Property<int>("ProductId");

                    b.Property<decimal>("Quantity");

                    b.Property<string>("UserOfferId");

                    b.HasKey("OfferId");

                    b.HasIndex("ProductId");

                    b.ToTable("Offer");
                });

            modelBuilder.Entity("ScaffoldersProject.Models.Order", b =>
                {
                    b.Property<int>("OrderID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("OrderDay");

                    b.Property<string>("UserOrderId");

                    b.HasKey("OrderID");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("ScaffoldersProject.Models.Portfolio", b =>
                {
                    b.Property<int>("PortfolioId")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("CoinsQuantity");

                    b.Property<int>("ProductId");

                    b.Property<string>("UserPortofolioId");

                    b.HasKey("PortfolioId");

                    b.HasIndex("ProductId");

                    b.ToTable("PortFolio");
                });

            modelBuilder.Entity("ScaffoldersProject.Models.Products", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("AdminApproved");

                    b.Property<string>("Category")
                        .IsRequired();

                    b.Property<string>("ContentType");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<byte[]>("Image");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<decimal>("Price");

                    b.Property<string>("ShortName")
                        .IsRequired()
                        .HasMaxLength(3);

                    b.Property<decimal>("Stock");

                    b.HasKey("ProductId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("ScaffoldersProject.Models.Sell", b =>
                {
                    b.Property<int>("SellId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("SellDay");

                    b.Property<string>("UserSellId");

                    b.HasKey("SellId");

                    b.ToTable("Sell");
                });

            modelBuilder.Entity("ScaffoldersProject.Models.Settings", b =>
                {
                    b.Property<int>("SettingsId")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("AdminFee");

                    b.Property<decimal>("MemberFee");

                    b.HasKey("SettingsId");

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("ScaffoldersProject.Models.TradeHistory", b =>
                {
                    b.Property<int>("TradeHistoryId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateofTransaction");

                    b.Property<decimal>("Price");

                    b.Property<int>("ProductId");

                    b.Property<decimal>("Quantity");

                    b.Property<string>("Status");

                    b.HasKey("TradeHistoryId");

                    b.HasIndex("ProductId");

                    b.ToTable("TradeHistory");
                });

            modelBuilder.Entity("ScaffoldersProject.Models.Ask", b =>
                {
                    b.HasOne("ScaffoldersProject.Models.Products", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ScaffoldersProject.Models.Cart", b =>
                {
                    b.HasOne("ScaffoldersProject.Models.Products", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ScaffoldersProject.Models.CartOrder", b =>
                {
                    b.HasOne("ScaffoldersProject.Models.Order", "Order")
                        .WithMany()
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ScaffoldersProject.Models.Products", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ScaffoldersProject.Models.CartSell", b =>
                {
                    b.HasOne("ScaffoldersProject.Models.Products", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ScaffoldersProject.Models.Sell", "Sell")
                        .WithMany()
                        .HasForeignKey("SellId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ScaffoldersProject.Models.Offer", b =>
                {
                    b.HasOne("ScaffoldersProject.Models.Products", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ScaffoldersProject.Models.Portfolio", b =>
                {
                    b.HasOne("ScaffoldersProject.Models.Products", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ScaffoldersProject.Models.TradeHistory", b =>
                {
                    b.HasOne("ScaffoldersProject.Models.Products", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
