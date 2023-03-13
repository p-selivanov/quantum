﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Quantum.AccountSearch.Persistence;

#nullable disable

namespace Quantum.AccountSearch.Persistence.Migrations
{
    [DbContext(typeof(AccountSearchDbContext))]
    partial class AccountSearchDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "citext");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Quantum.AccountSearch.Persistence.Models.CustomerAccount", b =>
                {
                    b.Property<string>("CustomerId")
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.Property<string>("Currency")
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<decimal>("Balance")
                        .HasPrecision(24, 8)
                        .HasColumnType("numeric(24,8)");

                    b.Property<DateTime?>("BalanceUpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Country")
                        .HasMaxLength(20)
                        .HasColumnType("citext");

                    b.Property<DateTime>("CustomerCreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("EmailAddress")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTime?>("FirstDepositTimestamp")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FirstName")
                        .HasMaxLength(50)
                        .HasColumnType("citext");

                    b.Property<bool>("HadSuspension")
                        .HasColumnType("boolean");

                    b.Property<string>("LastName")
                        .HasMaxLength(50)
                        .HasColumnType("citext");

                    b.Property<string>("Status")
                        .HasMaxLength(20)
                        .HasColumnType("citext");

                    b.Property<long>("Version")
                        .HasColumnType("bigint");

                    b.HasKey("CustomerId", "Currency");

                    b.HasIndex("Balance");

                    b.HasIndex("BalanceUpdatedAt");

                    b.HasIndex("Country");

                    b.HasIndex("CustomerCreatedAt");

                    b.HasIndex("EmailAddress");

                    b.HasIndex("FirstName");

                    b.HasIndex("LastName");

                    b.ToTable("CustomerAccounts");
                });
#pragma warning restore 612, 618
        }
    }
}
