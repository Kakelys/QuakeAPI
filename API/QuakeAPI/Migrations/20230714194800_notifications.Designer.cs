﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using QuakeAPI.Data;

#nullable disable

namespace QuakeAPI.Migrations
{
    [DbContext(typeof(QuakeDbContext))]
    [Migration("20230714194800_notifications")]
    partial class notifications
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("QuakeAPI.Data.Models.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("LastLoggedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(50)")
                        .HasDefaultValue("User");

                    b.Property<int>("TelegramId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<string>("Username")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(255)")
                        .HasDefaultValue("noname");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("QuakeAPI.Data.Models.ActiveAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ConnectedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DisconnectedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("SessionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("SessionId");

                    b.ToTable("ActiveAccounts");
                });

            modelBuilder.Entity("QuakeAPI.Data.Models.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(3000)");

                    b.Property<string>("LocationPath")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<int>("MaxPlayers")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("PosterPath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("QuakeAPI.Data.Models.Notification", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasColumnType("nvarchar(3000)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime?>("ReadAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("QuakeAPI.Data.Models.Session", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("LocationId")
                        .HasColumnType("int");

                    b.Property<int>("MaxPlayers")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("LocationId");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("QuakeAPI.Data.Models.Token", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ExpiresAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(1000)");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("RefreshToken")
                        .IsUnique();

                    b.ToTable("Tokens");
                });

            modelBuilder.Entity("QuakeAPI.Data.Models.ActiveAccount", b =>
                {
                    b.HasOne("QuakeAPI.Data.Models.Account", "Account")
                        .WithMany("ActiveAccounts")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_ActiveAccount_AccountId");

                    b.HasOne("QuakeAPI.Data.Models.Session", "Session")
                        .WithMany("ActiveAccounts")
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_ActiveAccount_SessionId");

                    b.Navigation("Account");

                    b.Navigation("Session");
                });

            modelBuilder.Entity("QuakeAPI.Data.Models.Notification", b =>
                {
                    b.HasOne("QuakeAPI.Data.Models.Account", "Account")
                        .WithMany("Notifications")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("QuakeAPI.Data.Models.Session", b =>
                {
                    b.HasOne("QuakeAPI.Data.Models.Location", "Location")
                        .WithMany("Sessions")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Session_LocationId");

                    b.Navigation("Location");
                });

            modelBuilder.Entity("QuakeAPI.Data.Models.Token", b =>
                {
                    b.HasOne("QuakeAPI.Data.Models.Account", "Account")
                        .WithMany("Tokens")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Token_AccountId");

                    b.Navigation("Account");
                });

            modelBuilder.Entity("QuakeAPI.Data.Models.Account", b =>
                {
                    b.Navigation("ActiveAccounts");

                    b.Navigation("Notifications");

                    b.Navigation("Tokens");
                });

            modelBuilder.Entity("QuakeAPI.Data.Models.Location", b =>
                {
                    b.Navigation("Sessions");
                });

            modelBuilder.Entity("QuakeAPI.Data.Models.Session", b =>
                {
                    b.Navigation("ActiveAccounts");
                });
#pragma warning restore 612, 618
        }
    }
}
