﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SinSense.Infastructure;

namespace SinSense.SQLite.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20200419083822_Initial Create")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3");

            modelBuilder.Entity("SinSenseCore.Entities.Word", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("Language")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Text")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Text");

                    b.HasIndex("Language", "Text")
                        .IsUnique();

                    b.ToTable("Words");
                });

            modelBuilder.Entity("SinSenseCore.Entities.WordRelation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("FromWordId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ToWordId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("FromWordId");

                    b.HasIndex("ToWordId");

                    b.HasIndex("Type", "FromWordId", "ToWordId")
                        .IsUnique();

                    b.ToTable("WordRelations");
                });

            modelBuilder.Entity("SinSenseCore.Entities.WordRelation", b =>
                {
                    b.HasOne("SinSenseCore.Entities.Word", "FromWord")
                        .WithMany("Relations")
                        .HasForeignKey("FromWordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SinSenseCore.Entities.Word", "ToWord")
                        .WithMany()
                        .HasForeignKey("ToWordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
