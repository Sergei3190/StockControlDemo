﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Note.API.DAL.Context;

#nullable disable

namespace Note.API.DAL.Migrations
{
    [DbContext(typeof(NoteDB))]
    [Migration("20230720130448_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Note.API.Domain.Note.UserNote", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id")
                        .HasDefaultValueSql("NEWSEQUENTIALID()");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("content");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("_created_by");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValue(new DateTimeOffset(new DateTime(2023, 7, 20, 17, 4, 48, 412, DateTimeKind.Unspecified).AddTicks(8821), new TimeSpan(0, 4, 0, 0, 0)))
                        .HasColumnName("_created");

                    b.Property<Guid?>("DeletedBy")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("_deleted_by");

                    b.Property<DateTimeOffset?>("DeletedDate")
                        .HasColumnType("datetimeoffset")
                        .HasColumnName("_deleted");

                    b.Property<DateTime?>("ExecutionDate")
                        .HasColumnType("date")
                        .HasColumnName("execution_date");

                    b.Property<bool>("IsFix")
                        .HasColumnType("bit")
                        .HasColumnName("is_fix");

                    b.Property<int>("Sort")
                        .HasColumnType("int")
                        .HasColumnName("sort");

                    b.Property<Guid?>("UpdatedBy")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("_updated_by");

                    b.Property<DateTimeOffset?>("UpdatedDate")
                        .HasColumnType("datetimeoffset")
                        .HasColumnName("_updated");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("Content");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("DeletedBy");

                    b.HasIndex("UpdatedBy");

                    b.HasIndex("UserId");

                    b.ToTable("notes", "note");
                });

            modelBuilder.Entity("Service.Common.Entities.App.Source", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id")
                        .HasDefaultValueSql("NEWSEQUENTIALID()");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit")
                        .HasColumnName("is_active");

                    b.Property<string>("Mnemo")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("mnemo");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("sources", "app");
                });

            modelBuilder.Entity("Service.Common.Entities.App.UserInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id")
                        .HasDefaultValueSql("NEWSEQUENTIALID()");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValue(new DateTimeOffset(new DateTime(2023, 7, 20, 17, 4, 48, 412, DateTimeKind.Unspecified).AddTicks(2399), new TimeSpan(0, 4, 0, 0, 0)))
                        .HasColumnName("_created");

                    b.Property<DateTimeOffset?>("DeletedDate")
                        .HasColumnType("datetimeoffset")
                        .HasColumnName("_deleted_date");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("email");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("name");

                    b.Property<Guid>("SourceId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("source_id");

                    b.Property<DateTimeOffset?>("UpdatedDate")
                        .HasColumnType("datetimeoffset")
                        .HasColumnName("_updated");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("SourceId");

                    b.ToTable("users_info", "app");
                });

            modelBuilder.Entity("Note.API.Domain.Note.UserNote", b =>
                {
                    b.HasOne("Service.Common.Entities.App.UserInfo", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedBy")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("Service.Common.Entities.App.UserInfo", "DeletedByUser")
                        .WithMany()
                        .HasForeignKey("DeletedBy")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("Service.Common.Entities.App.UserInfo", "UpdatedByUser")
                        .WithMany()
                        .HasForeignKey("UpdatedBy")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("Service.Common.Entities.App.UserInfo", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("CreatedByUser");

                    b.Navigation("DeletedByUser");

                    b.Navigation("UpdatedByUser");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Service.Common.Entities.App.UserInfo", b =>
                {
                    b.HasOne("Service.Common.Entities.App.Source", "Source")
                        .WithMany("UsersInfo")
                        .HasForeignKey("SourceId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Source");
                });

            modelBuilder.Entity("Service.Common.Entities.App.Source", b =>
                {
                    b.Navigation("UsersInfo");
                });
#pragma warning restore 612, 618
        }
    }
}
