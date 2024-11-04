﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebShop.Data;

#nullable disable

namespace WebShop.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241104200331_InitDataBase")]
    partial class InitDataBase
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WebShop.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImgPath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<int>("ProductCategoryId")
                        .HasColumnType("int");

                    b.Property<int>("SupplierId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductCategoryId");

                    b.HasIndex("SupplierId");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Currency = "USD",
                            Description = "Fantastic price. Large content ecosystem. Good parental controls. Helpful technical support.",
                            ImgPath = "Amazon Fire",
                            Name = "Amazon Fire",
                            Price = 49.899999999999999,
                            ProductCategoryId = 1,
                            SupplierId = 1
                        },
                        new
                        {
                            Id = 2,
                            Currency = "USD",
                            Description = "Keyboard cover is included. Fanless Core m5 processor. Full-size USB ports. Adjustable kickstand.",
                            ImgPath = "Lenovo IdeaPad Miix 700",
                            Name = "Lenovo IdeaPad Miix 700",
                            Price = 479.89999999999998,
                            ProductCategoryId = 3,
                            SupplierId = 2
                        },
                        new
                        {
                            Id = 5,
                            Currency = "USD",
                            Description = "Keyboard cover is included. Fanless Core m5 processor. Full-size USB ports. Adjustable kickstand.",
                            ImgPath = "Lenovo M10",
                            Name = "Lenovo M10",
                            Price = 179.90000000000001,
                            ProductCategoryId = 1,
                            SupplierId = 2
                        },
                        new
                        {
                            Id = 3,
                            Currency = "USD",
                            Description = "Amazon's latest Fire HD 8 tablet is a great value for media consumption.",
                            ImgPath = "Amazon Fire HD 8",
                            Name = "Amazon Fire HD 8",
                            Price = 89.900000000000006,
                            ProductCategoryId = 1,
                            SupplierId = 1
                        },
                        new
                        {
                            Id = 4,
                            Currency = "USD",
                            Description = "The latest iPhone with improved performance, better camera, and 5G capabilities.",
                            ImgPath = "iPhone 12",
                            Name = "iPhone 12",
                            Price = 799.89999999999998,
                            ProductCategoryId = 2,
                            SupplierId = 4
                        });
                });

            modelBuilder.Entity("WebShop.Models.ProductCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Tablets"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Smartphone"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Laptops"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Others"
                        });
                });

            modelBuilder.Entity("WebShop.Models.Supplier", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.ToTable("Suppliers");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Amazon"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Lenovo"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Asus"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Apple"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Samsung"
                        },
                        new
                        {
                            Id = 6,
                            Name = "Xiaomi"
                        });
                });

            modelBuilder.Entity("WebShop.Models.Product", b =>
                {
                    b.HasOne("WebShop.Models.ProductCategory", "Category")
                        .WithMany()
                        .HasForeignKey("ProductCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebShop.Models.Supplier", "Supplier")
                        .WithMany()
                        .HasForeignKey("SupplierId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Supplier");
                });
#pragma warning restore 612, 618
        }
    }
}
