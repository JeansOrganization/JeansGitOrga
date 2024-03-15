﻿// <auto-generated />
using System;
using EFCore实体配置DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EFCore实体配置DataAnnotations.Migrations
{
    [DbContext(typeof(MyDbContext))]
    [Migration("20240314030848_upd1")]
    partial class upd1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.28")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("EFCore实体配置DataAnnotations.Teacher", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("indate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("name")
                        .HasColumnType("longtext");

                    b.Property<double>("salary")
                        .HasColumnType("double");

                    b.HasKey("id");

                    b.ToTable("T_Teacher");
                });
#pragma warning restore 612, 618
        }
    }
}
