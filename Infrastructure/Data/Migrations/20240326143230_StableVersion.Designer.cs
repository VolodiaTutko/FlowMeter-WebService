﻿// <auto-generated />
using System;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240326143230_StableVersion")]
    partial class StableVersion
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Application.Models.Account", b =>
                {
                    b.Property<int>("AccountID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("AccountID"));

                    b.Property<string>("ColdWater")
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<string>("Electricity")
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<string>("Gas")
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<string>("Heating")
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<string>("HotWater")
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<string>("PersonalAccount")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<string>("PublicService")
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.HasKey("AccountID");

                    b.HasIndex("PersonalAccount");

                    b.ToTable("accounts");
                });

            modelBuilder.Entity("Application.Models.Accrual", b =>
                {
                    b.Property<int>("AccrualID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("AccrualID"));

                    b.Property<double>("Accrued")
                        .HasColumnType("double precision");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("PersonalAccount")
                        .IsRequired()
                        .HasColumnType("character varying(10)");

                    b.Property<double?>("PreviuosDebit")
                        .HasColumnType("double precision");

                    b.HasKey("AccrualID");

                    b.HasIndex("PersonalAccount");

                    b.ToTable("accruals");
                });

            modelBuilder.Entity("Application.Models.Admin", b =>
                {
                    b.Property<string>("AdminEmail")
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("AdminEmail");

                    b.ToTable("admins");
                });

            modelBuilder.Entity("Application.Models.Consumer", b =>
                {
                    b.Property<string>("PersonalAccount")
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<string>("ConsumerEmail")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ConsumerOwner")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Flat")
                        .HasColumnType("integer");

                    b.Property<int>("HeatingArea")
                        .HasColumnType("integer");

                    b.Property<int>("HouseId")
                        .HasColumnType("integer");

                    b.Property<int>("NumberOfPersons")
                        .HasColumnType("integer");

                    b.HasKey("PersonalAccount");

                    b.HasIndex("HouseId");

                    b.ToTable("consumers");
                });

            modelBuilder.Entity("Application.Models.House", b =>
                {
                    b.Property<int>("HouseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("HouseId"));

                    b.Property<int?>("HeatingAreaOfHouse")
                        .HasColumnType("integer");

                    b.Property<string>("HouseAddress")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("character varying(80)");

                    b.Property<int?>("NumberOfFlat")
                        .HasColumnType("integer");

                    b.Property<int?>("NumberOfResidents")
                        .HasColumnType("integer");

                    b.HasKey("HouseId");

                    b.ToTable("houses");
                });

            modelBuilder.Entity("Application.Models.Meter", b =>
                {
                    b.Property<int>("CountersId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("CountersId"));

                    b.Property<string>("CounterAccount")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal?>("CurrentIndicator")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TypeOfAccount")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("CountersId");

                    b.ToTable("meters");
                });

            modelBuilder.Entity("Application.Models.Payment", b =>
                {
                    b.Property<int>("PaymentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("PaymentID"));

                    b.Property<double>("Amount")
                        .HasColumnType("double precision");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("PersonalAccount")
                        .IsRequired()
                        .HasColumnType("character varying(10)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("PaymentID");

                    b.HasIndex("PersonalAccount");

                    b.ToTable("payments");
                });

            modelBuilder.Entity("Application.Models.Receipt", b =>
                {
                    b.Property<int>("ReceiptId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ReceiptId"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<byte[]>("PDF")
                        .HasColumnType("bytea");

                    b.Property<string>("PersonalAccount")
                        .IsRequired()
                        .HasColumnType("character varying(10)");

                    b.HasKey("ReceiptId");

                    b.HasIndex("PersonalAccount");

                    b.ToTable("receipts");
                });

            modelBuilder.Entity("Application.Models.Service", b =>
                {
                    b.Property<int>("ServiceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ServiceId"));

                    b.Property<int?>("HouseId")
                        .HasColumnType("integer");

                    b.Property<int?>("Price")
                        .HasColumnType("integer");

                    b.Property<string>("TypeOfAccount")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ServiceId");

                    b.HasIndex("HouseId");

                    b.ToTable("services");
                });

            modelBuilder.Entity("Application.Models.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("UserID"));

                    b.Property<string>("ConsumerEmail")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("UserID");

                    b.ToTable("users");
                });

            modelBuilder.Entity("Application.Models.Account", b =>
                {
                    b.HasOne("Application.Models.Consumer", "Consumer")
                        .WithMany()
                        .HasForeignKey("PersonalAccount")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Consumer");
                });

            modelBuilder.Entity("Application.Models.Accrual", b =>
                {
                    b.HasOne("Application.Models.Consumer", "Consumer")
                        .WithMany()
                        .HasForeignKey("PersonalAccount")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Consumer");
                });

            modelBuilder.Entity("Application.Models.Consumer", b =>
                {
                    b.HasOne("Application.Models.House", "House")
                        .WithMany()
                        .HasForeignKey("HouseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("House");
                });

            modelBuilder.Entity("Application.Models.Payment", b =>
                {
                    b.HasOne("Application.Models.Consumer", "Consumer")
                        .WithMany()
                        .HasForeignKey("PersonalAccount")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Consumer");
                });

            modelBuilder.Entity("Application.Models.Receipt", b =>
                {
                    b.HasOne("Application.Models.Consumer", "Consumer")
                        .WithMany()
                        .HasForeignKey("PersonalAccount")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Consumer");
                });

            modelBuilder.Entity("Application.Models.Service", b =>
                {
                    b.HasOne("Application.Models.House", "House")
                        .WithMany()
                        .HasForeignKey("HouseId");

                    b.Navigation("House");
                });
#pragma warning restore 612, 618
        }
    }
}
