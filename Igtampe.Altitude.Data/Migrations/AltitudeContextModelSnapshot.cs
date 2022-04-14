﻿// <auto-generated />
using System;
using Igtampe.Altitude.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Igtampe.Altitude.Data.Migrations
{
    [DbContext(typeof(AltitudeContext))]
    partial class AltitudeContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Igtampe.Altitude.Common.Day", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ImageURL")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Index")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("TripID")
                        .HasColumnType("uuid");

                    b.HasKey("ID");

                    b.HasIndex("TripID");

                    b.ToTable("Day");
                });

            modelBuilder.Entity("Igtampe.Altitude.Common.Event", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("DayID")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Hour")
                        .HasColumnType("integer");

                    b.Property<string>("ImageURL")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Index")
                        .HasColumnType("integer");

                    b.Property<int>("Minute")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ReminderTime")
                        .HasColumnType("integer");

                    b.HasKey("ID");

                    b.HasIndex("DayID");

                    b.ToTable("Event");
                });

            modelBuilder.Entity("Igtampe.Altitude.Common.Trip", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DateUpdated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ImageURL")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("OwnerUsername")
                        .HasColumnType("text");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("ID");

                    b.HasIndex("OwnerUsername");

                    b.ToTable("Trip");
                });

            modelBuilder.Entity("Igtampe.ChopoAuth.User", b =>
                {
                    b.Property<string>("Username")
                        .HasColumnType("text");

                    b.Property<string>("ImageURL")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("boolean");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Username");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Igtampe.ChopoImageHandling.Image", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<byte[]>("Data")
                        .HasColumnType("bytea");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ID");

                    b.ToTable("Image");
                });

            modelBuilder.Entity("Igtampe.Altitude.Common.Day", b =>
                {
                    b.HasOne("Igtampe.Altitude.Common.Trip", null)
                        .WithMany("Days")
                        .HasForeignKey("TripID");
                });

            modelBuilder.Entity("Igtampe.Altitude.Common.Event", b =>
                {
                    b.HasOne("Igtampe.Altitude.Common.Day", null)
                        .WithMany("Events")
                        .HasForeignKey("DayID");
                });

            modelBuilder.Entity("Igtampe.Altitude.Common.Trip", b =>
                {
                    b.HasOne("Igtampe.ChopoAuth.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerUsername");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("Igtampe.Altitude.Common.Day", b =>
                {
                    b.Navigation("Events");
                });

            modelBuilder.Entity("Igtampe.Altitude.Common.Trip", b =>
                {
                    b.Navigation("Days");
                });
#pragma warning restore 612, 618
        }
    }
}