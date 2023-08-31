﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RestaurantBookingApp.Data;

#nullable disable

namespace RestaurantBookingApp.Data.Migrations
{
    [DbContext(typeof(RestaurantBookingDBContext))]
    [Migration("20230831053158_updated foreign key field name")]
    partial class Updatedforeignkeyfieldname
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("RestaurantBookingApp.Core.DiningTable", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.Property<int>("RestaurantBranchId")
                        .HasColumnType("int");

                    b.Property<string>("TableName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("RestaurantBranchId");

                    b.ToTable("DiningTables");
                });

            modelBuilder.Entity("RestaurantBookingApp.Core.Reservation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("ReservationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ReservationStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TimeSlotId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TimeSlotId");

                    b.HasIndex("UserId");

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("RestaurantBookingApp.Core.Restaurant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Email")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ImageUrl")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Phone")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("Id");

                    b.ToTable("Restaurants");
                });

            modelBuilder.Entity("RestaurantBookingApp.Core.RestaurantBranch", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Email")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ImageUrl")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Phone")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<int>("RestaurantId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RestaurantId");

                    b.ToTable("RestaurantBranches");
                });

            modelBuilder.Entity("RestaurantBookingApp.Core.TimeSlot", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DiningTableId")
                        .HasColumnType("int");

                    b.Property<string>("MealType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ReservationDay")
                        .HasColumnType("datetime2");

                    b.Property<string>("TableStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DiningTableId");

                    b.ToTable("TimeSlots");
                });

            modelBuilder.Entity("RestaurantBookingApp.Core.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AdObjId")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ProfileImageUrl")
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("RestaurantBookingApp.Core.DiningTable", b =>
                {
                    b.HasOne("RestaurantBookingApp.Core.RestaurantBranch", "RestaurantBranch")
                        .WithMany("DiningTables")
                        .HasForeignKey("RestaurantBranchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RestaurantBranch");
                });

            modelBuilder.Entity("RestaurantBookingApp.Core.Reservation", b =>
                {
                    b.HasOne("RestaurantBookingApp.Core.TimeSlot", "TimeSlot")
                        .WithMany("Reservations")
                        .HasForeignKey("TimeSlotId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RestaurantBookingApp.Core.User", "User")
                        .WithMany("Reservations")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TimeSlot");

                    b.Navigation("User");
                });

            modelBuilder.Entity("RestaurantBookingApp.Core.RestaurantBranch", b =>
                {
                    b.HasOne("RestaurantBookingApp.Core.Restaurant", "Restaurant")
                        .WithMany("RestaurantBranches")
                        .HasForeignKey("RestaurantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Restaurant");
                });

            modelBuilder.Entity("RestaurantBookingApp.Core.TimeSlot", b =>
                {
                    b.HasOne("RestaurantBookingApp.Core.DiningTable", null)
                        .WithMany("TimeSlots")
                        .HasForeignKey("DiningTableId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RestaurantBookingApp.Core.DiningTable", b =>
                {
                    b.Navigation("TimeSlots");
                });

            modelBuilder.Entity("RestaurantBookingApp.Core.Restaurant", b =>
                {
                    b.Navigation("RestaurantBranches");
                });

            modelBuilder.Entity("RestaurantBookingApp.Core.RestaurantBranch", b =>
                {
                    b.Navigation("DiningTables");
                });

            modelBuilder.Entity("RestaurantBookingApp.Core.TimeSlot", b =>
                {
                    b.Navigation("Reservations");
                });

            modelBuilder.Entity("RestaurantBookingApp.Core.User", b =>
                {
                    b.Navigation("Reservations");
                });
#pragma warning restore 612, 618
        }
    }
}
