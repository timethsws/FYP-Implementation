﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SinSenseInfastructure;

namespace SinSense.SQLite.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                    b.HasIndex("Language");

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

                    b.HasIndex("Type");

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
