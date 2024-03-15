﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TODOBack.Data;

#nullable disable

namespace TODOBack.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240314173112_Initialize")]
    partial class Initialize
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TODOBack.Models.BillingModel", b =>
                {
                    b.Property<string>("TxID")
                        .HasColumnType("text");

                    b.Property<string>("CPF")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("Deadline")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UserID")
                        .HasColumnType("integer");

                    b.Property<float?>("Value")
                        .HasColumnType("real");

                    b.HasKey("TxID");

                    b.HasIndex("UserID")
                        .IsUnique();

                    b.ToTable("Billings", (string)null);
                });

            modelBuilder.Entity("TODOBack.Models.TaskModel", b =>
                {
                    b.Property<int>("TaskId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("TaskId"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("OrganizerId")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("TaskId");

                    b.HasIndex("OrganizerId");

                    b.ToTable("Tasks", (string)null);
                });

            modelBuilder.Entity("TODOBack.Models.UserModel", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("UserId"));

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<bool>("IsPremium")
                        .HasColumnType("boolean");

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("UserId");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("TODOBack.Models.BillingModel", b =>
                {
                    b.HasOne("TODOBack.Models.UserModel", "User")
                        .WithOne("Billing")
                        .HasForeignKey("TODOBack.Models.BillingModel", "UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("TODOBack.Models.TaskModel", b =>
                {
                    b.HasOne("TODOBack.Models.UserModel", "Organizer")
                        .WithMany("Tasks")
                        .HasForeignKey("OrganizerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Organizer");
                });

            modelBuilder.Entity("TODOBack.Models.UserModel", b =>
                {
                    b.Navigation("Billing");

                    b.Navigation("Tasks");
                });
#pragma warning restore 612, 618
        }
    }
}
