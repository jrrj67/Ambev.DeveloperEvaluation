﻿// <auto-generated />
using System;
using Ambev.DeveloperEvaluation.ORM;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ambev.DeveloperEvaluation.ORM.Migrations
{
    [DbContext(typeof(StoreDbContext))]
    [Migration("20250310155856_FixSaleDateTimestamp")]
    partial class FixSaleDateTimestamp
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "postgis");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Ambev.DeveloperEvaluation.Domain.Entities.Cart", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("PriceTotal")
                        .HasColumnType("numeric");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Carts", (string)null);
                });

            modelBuilder.Entity("Ambev.DeveloperEvaluation.Domain.Entities.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric(10,2)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("Title");

                    b.ToTable("Products", (string)null);
                });

            modelBuilder.Entity("Ambev.DeveloperEvaluation.Domain.Entities.Sale", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<string>("Branch")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("CartId")
                        .HasColumnType("uuid");

                    b.Property<string>("Customer")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsCancelled")
                        .HasColumnType("boolean");

                    b.Property<decimal>("PriceTotal")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("SaleDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("SaleNumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Sales", (string)null);
                });

            modelBuilder.Entity("Ambev.DeveloperEvaluation.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("varchar(15)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("varchar(15)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Phone");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("Ambev.DeveloperEvaluation.Domain.Entities.Cart", b =>
                {
                    b.OwnsMany("Ambev.DeveloperEvaluation.Domain.Entities.CartItem", "Products", b1 =>
                        {
                            b1.Property<Guid>("CartId")
                                .HasColumnType("uuid");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<int>("Id"));

                            b1.Property<decimal>("Discount")
                                .HasColumnType("numeric");

                            b1.Property<decimal>("PriceTotal")
                                .HasColumnType("numeric");

                            b1.Property<decimal>("PriceTotalWithDiscount")
                                .HasColumnType("numeric");

                            b1.Property<Guid>("ProductId")
                                .HasColumnType("uuid")
                                .HasColumnName("ProductId");

                            b1.Property<int>("Quantity")
                                .HasColumnType("int")
                                .HasColumnName("Quantity");

                            b1.Property<decimal>("UnitPrice")
                                .HasColumnType("numeric");

                            b1.HasKey("CartId", "Id");

                            b1.ToTable("CartItem");

                            b1.WithOwner()
                                .HasForeignKey("CartId");
                        });

                    b.Navigation("Products");
                });

            modelBuilder.Entity("Ambev.DeveloperEvaluation.Domain.Entities.Product", b =>
                {
                    b.OwnsOne("Ambev.DeveloperEvaluation.Domain.Entities.CategoryInfo", "Category", b1 =>
                        {
                            b1.Property<Guid>("ProductId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("uuid");

                            b1.Property<string>("ExternalId")
                                .IsRequired()
                                .HasColumnType("varchar(50)")
                                .HasColumnName("Category_ExternalId");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasColumnType("varchar(100)")
                                .HasColumnName("Category_Name");

                            b1.HasKey("ProductId");

                            b1.ToTable("Products");

                            b1.WithOwner()
                                .HasForeignKey("ProductId");
                        });

                    b.OwnsOne("Ambev.DeveloperEvaluation.Domain.Entities.RatingInfo", "Rating", b1 =>
                        {
                            b1.Property<Guid>("ProductId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("uuid");

                            b1.Property<double>("AverageRate")
                                .HasColumnType("numeric(3,1)")
                                .HasColumnName("Rating_AverageRate");

                            b1.Property<string>("ExternalId")
                                .IsRequired()
                                .HasColumnType("varchar(50)")
                                .HasColumnName("Rating_ExternalId");

                            b1.Property<int>("TotalReviews")
                                .HasColumnType("int")
                                .HasColumnName("Rating_TotalReviews");

                            b1.HasKey("ProductId");

                            b1.ToTable("Products");

                            b1.WithOwner()
                                .HasForeignKey("ProductId");
                        });

                    b.Navigation("Category")
                        .IsRequired();

                    b.Navigation("Rating")
                        .IsRequired();
                });

            modelBuilder.Entity("Ambev.DeveloperEvaluation.Domain.Entities.User", b =>
                {
                    b.OwnsOne("Ambev.DeveloperEvaluation.Domain.Entities.AddressInfo", "Address", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("uuid");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasColumnType("varchar(100)")
                                .HasColumnName("Address_City");

                            b1.Property<int>("Number")
                                .HasColumnType("int")
                                .HasColumnName("Address_Number");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasColumnType("varchar(100)")
                                .HasColumnName("Address_Street");

                            b1.Property<string>("Zipcode")
                                .IsRequired()
                                .HasColumnType("varchar(20)")
                                .HasColumnName("Address_Zipcode");

                            b1.HasKey("UserId");

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("UserId");

                            b1.OwnsOne("Ambev.DeveloperEvaluation.Domain.Entities.GeolocationInfo", "Geolocation", b2 =>
                                {
                                    b2.Property<Guid>("AddressInfoUserId")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("uuid");

                                    b2.Property<double>("Latitude")
                                        .HasColumnType("double precision")
                                        .HasColumnName("Geolocation_Latitude");

                                    b2.Property<double>("Longitude")
                                        .HasColumnType("double precision")
                                        .HasColumnName("Geolocation_Longitude");

                                    b2.HasKey("AddressInfoUserId");

                                    b2.ToTable("Users");

                                    b2.WithOwner()
                                        .HasForeignKey("AddressInfoUserId");
                                });

                            b1.Navigation("Geolocation")
                                .IsRequired();
                        });

                    b.OwnsOne("Ambev.DeveloperEvaluation.Domain.Entities.NameInfo", "Name", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("uuid");

                            b1.Property<string>("Firstname")
                                .IsRequired()
                                .HasColumnType("varchar(100)")
                                .HasColumnName("Firstname");

                            b1.Property<string>("Lastname")
                                .IsRequired()
                                .HasColumnType("varchar(100)")
                                .HasColumnName("Lastname");

                            b1.HasKey("UserId");

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.Navigation("Address")
                        .IsRequired();

                    b.Navigation("Name")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
