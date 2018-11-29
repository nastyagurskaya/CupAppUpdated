﻿// <auto-generated />
using CupApp.Models;
using CupApp.Presentation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace CupApp.Migrations
{
    [DbContext(typeof(CupContext))]
    [Migration("20181127104449_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CupApp.Models.Country", b =>
                {
                    b.Property<int>("CountryId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CountryName");

                    b.HasKey("CountryId");

                    b.ToTable("Country");
                });

            modelBuilder.Entity("CupApp.Models.Cup", b =>
                {
                    b.Property<int>("CupId")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Capacity");

                    b.Property<int>("CountryID");

                    b.Property<int?>("CupType");

                    b.HasKey("CupId");

                    b.HasIndex("CountryID");

                    b.ToTable("Cup");
                });

            modelBuilder.Entity("CupApp.Models.CupImage", b =>
                {
                    b.Property<int>("CupImageID");

                    b.Property<byte[]>("Image");

                    b.HasKey("CupImageID");

                    b.ToTable("CupImage");
                });

            modelBuilder.Entity("CupApp.Models.Cup", b =>
                {
                    b.HasOne("CupApp.Models.Country", "Country")
                        .WithMany("Cups")
                        .HasForeignKey("CountryID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CupApp.Models.CupImage", b =>
                {
                    b.HasOne("CupApp.Models.Cup", "Cup")
                        .WithOne("CupImage")
                        .HasForeignKey("CupApp.Models.CupImage", "CupImageID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
