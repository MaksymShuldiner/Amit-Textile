﻿// <auto-generated />
using System;
using AmitTextile.Domain.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AmitTextile.Migrations
{
    [DbContext(typeof(AmitDbContext))]
    [Migration("20200314182203_27")]
    partial class _27
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AmitTextile.Domain.Cart", b =>
                {
                    b.Property<Guid>("CartId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("CartId");

                    b.ToTable("Carts");
                });

            modelBuilder.Entity("AmitTextile.Domain.Category", b =>
                {
                    b.Property<Guid>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CategoryId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("AmitTextile.Domain.Charachteristic", b =>
                {
                    b.Property<Guid>("CharachteristicId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CharachteristicId");

                    b.ToTable("Charachteristics");
                });

            modelBuilder.Entity("AmitTextile.Domain.CharachteristicValues", b =>
                {
                    b.Property<Guid>("CharachteristicValuesId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("TextileId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CharachteristicValuesId");

                    b.HasIndex("TextileId");

                    b.ToTable("CharachteristicValues");
                });

            modelBuilder.Entity("AmitTextile.Domain.CharachteristicVariants", b =>
                {
                    b.Property<Guid>("CharachteristicVariantsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CharachteristicId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CharachteristicVariantsId");

                    b.HasIndex("CharachteristicId");

                    b.ToTable("CharachteristicVariants");
                });

            modelBuilder.Entity("AmitTextile.Domain.ChildCategory", b =>
                {
                    b.Property<Guid>("ChildCategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ChildCategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("ChildCategories");
                });

            modelBuilder.Entity("AmitTextile.Domain.ChildCommentQuestion", b =>
                {
                    b.Property<Guid>("ChildCommentQuestionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DatePosted")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ParentCommentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("SenderId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("TextileId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ChildCommentQuestionId");

                    b.HasIndex("ParentCommentId");

                    b.HasIndex("SenderId");

                    b.HasIndex("TextileId");

                    b.ToTable("ChildCommentQuestions");
                });

            modelBuilder.Entity("AmitTextile.Domain.FilterCharachteristics", b =>
                {
                    b.Property<Guid>("FilterCharachteristicsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CharachteristicId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("FilterCharachteristicsId");

                    b.HasIndex("CharachteristicId")
                        .IsUnique();

                    b.ToTable("FilterCharachteristicses");
                });

            modelBuilder.Entity("AmitTextile.Domain.Image", b =>
                {
                    b.Property<Guid>("ImageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("ByteImg")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("SliderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TextileId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ImageId");

                    b.HasIndex("SliderId");

                    b.HasIndex("TextileId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("AmitTextile.Domain.Item", b =>
                {
                    b.Property<Guid>("ItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CartId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ItemsAmount")
                        .HasColumnType("int");

                    b.Property<Guid>("TextileId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ItemId");

                    b.HasIndex("CartId");

                    b.HasIndex("TextileId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("AmitTextile.Domain.ItemOrder", b =>
                {
                    b.Property<Guid>("ItemId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ItemId", "OrderId");

                    b.HasIndex("OrderId");

                    b.ToTable("ItemOrder");
                });

            modelBuilder.Entity("AmitTextile.Domain.Order", b =>
                {
                    b.Property<Guid>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CardNum")
                        .HasColumnType("int");

                    b.Property<string>("DepartmentName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DepartmentNum")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Fio")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("isPaidByCash")
                        .HasColumnType("bit");

                    b.HasKey("OrderId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("AmitTextile.Domain.ParentCommentQuestion", b =>
                {
                    b.Property<Guid>("ParentCommentQuestionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DatePosted")
                        .HasColumnType("datetime2");

                    b.Property<string>("SenderId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("TextileId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ParentCommentQuestionId");

                    b.HasIndex("SenderId");

                    b.HasIndex("TextileId");

                    b.ToTable("ParentCommentQuestions");
                });

            modelBuilder.Entity("AmitTextile.Domain.ParentCommentReview", b =>
                {
                    b.Property<Guid>("ParentCommentReviewId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DatePosted")
                        .HasColumnType("datetime2");

                    b.Property<string>("SenderId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Stars")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("TextileId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ParentCommentReviewId");

                    b.HasIndex("SenderId");

                    b.HasIndex("TextileId");

                    b.ToTable("ParentCommentReviews");
                });

            modelBuilder.Entity("AmitTextile.Domain.Slider", b =>
                {
                    b.Property<Guid>("SliderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("SliderName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SliderId");

                    b.ToTable("Slider");
                });

            modelBuilder.Entity("AmitTextile.Domain.Textile", b =>
                {
                    b.Property<Guid>("TextileId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ChildCategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateWhenAdded")
                        .HasColumnType("datetime2");

                    b.Property<double>("Discount")
                        .HasColumnType("float");

                    b.Property<bool>("IsOnDiscount")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPopular")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Sold")
                        .HasColumnType("int");

                    b.Property<double>("Stars")
                        .HasColumnType("float");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ViewsCounter")
                        .HasColumnType("int");

                    b.Property<int>("WarehouseAmount")
                        .HasColumnType("int");

                    b.HasKey("TextileId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("ChildCategoryId");

                    b.ToTable("Textiles");
                });

            modelBuilder.Entity("AmitTextile.Domain.UserChosenTextile", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("TextileId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "TextileId");

                    b.HasIndex("TextileId");

                    b.ToTable("UserChosenTextile");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityUser");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("AmitTextile.Domain.User", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityUser");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("CartId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Fio")
                        .HasColumnType("nvarchar(max)");

                    b.HasIndex("CartId")
                        .IsUnique()
                        .HasFilter("[CartId] IS NOT NULL");

                    b.HasDiscriminator().HasValue("User");
                });

            modelBuilder.Entity("AmitTextile.Domain.CharachteristicValues", b =>
                {
                    b.HasOne("AmitTextile.Domain.Textile", "Textile")
                        .WithMany("Charachteristics")
                        .HasForeignKey("TextileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AmitTextile.Domain.CharachteristicVariants", b =>
                {
                    b.HasOne("AmitTextile.Domain.Charachteristic", "Charachteristic")
                        .WithMany("Values")
                        .HasForeignKey("CharachteristicId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AmitTextile.Domain.ChildCategory", b =>
                {
                    b.HasOne("AmitTextile.Domain.Category", "Category")
                        .WithMany("ChildCategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AmitTextile.Domain.ChildCommentQuestion", b =>
                {
                    b.HasOne("AmitTextile.Domain.ParentCommentQuestion", "ParentComment")
                        .WithMany("ChildComments")
                        .HasForeignKey("ParentCommentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AmitTextile.Domain.User", "Sender")
                        .WithMany("ChildCommentQuestions")
                        .HasForeignKey("SenderId");

                    b.HasOne("AmitTextile.Domain.Textile", "Textile")
                        .WithMany("ChildCommentQuestions")
                        .HasForeignKey("TextileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AmitTextile.Domain.FilterCharachteristics", b =>
                {
                    b.HasOne("AmitTextile.Domain.Charachteristic", "Charachteristic")
                        .WithOne("FilterCharachteristics")
                        .HasForeignKey("AmitTextile.Domain.FilterCharachteristics", "CharachteristicId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AmitTextile.Domain.Image", b =>
                {
                    b.HasOne("AmitTextile.Domain.Slider", "Slider")
                        .WithMany("Images")
                        .HasForeignKey("SliderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AmitTextile.Domain.Textile", "Textile")
                        .WithMany("Images")
                        .HasForeignKey("TextileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AmitTextile.Domain.Item", b =>
                {
                    b.HasOne("AmitTextile.Domain.Cart", "Cart")
                        .WithMany("Items")
                        .HasForeignKey("CartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AmitTextile.Domain.Textile", "Textile")
                        .WithMany()
                        .HasForeignKey("TextileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AmitTextile.Domain.ItemOrder", b =>
                {
                    b.HasOne("AmitTextile.Domain.Item", "Item")
                        .WithMany("ItemOrders")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AmitTextile.Domain.Order", "Order")
                        .WithMany("ItemOrders")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AmitTextile.Domain.ParentCommentQuestion", b =>
                {
                    b.HasOne("AmitTextile.Domain.User", "Sender")
                        .WithMany("ParentCommentQuestions")
                        .HasForeignKey("SenderId");

                    b.HasOne("AmitTextile.Domain.Textile", "Textile")
                        .WithMany("ParentCommentQuestions")
                        .HasForeignKey("TextileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AmitTextile.Domain.ParentCommentReview", b =>
                {
                    b.HasOne("AmitTextile.Domain.User", "Sender")
                        .WithMany("ParentCommentReviews")
                        .HasForeignKey("SenderId");

                    b.HasOne("AmitTextile.Domain.Textile", "Textile")
                        .WithMany("ParentCommentReviews")
                        .HasForeignKey("TextileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AmitTextile.Domain.Textile", b =>
                {
                    b.HasOne("AmitTextile.Domain.Category", "Category")
                        .WithMany("TextilesOfThisCategory")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AmitTextile.Domain.ChildCategory", "ChildCategory")
                        .WithMany("TextilesOfThisChildCategory")
                        .HasForeignKey("ChildCategoryId");
                });

            modelBuilder.Entity("AmitTextile.Domain.UserChosenTextile", b =>
                {
                    b.HasOne("AmitTextile.Domain.Textile", "Textile")
                        .WithMany("UserChosenTextiles")
                        .HasForeignKey("TextileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AmitTextile.Domain.User", "User")
                        .WithMany("UserChosenTextiles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AmitTextile.Domain.User", b =>
                {
                    b.HasOne("AmitTextile.Domain.Cart", "Cart")
                        .WithOne("User")
                        .HasForeignKey("AmitTextile.Domain.User", "CartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
