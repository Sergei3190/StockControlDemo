﻿// <auto-generated />
using System;
using IntegrationEventLogEF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Notification.API.DAL.Migrations.IntegrationEventLog
{
    [DbContext(typeof(IntegrationEventLogContext))]
    partial class IntegrationEventLogContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("IntegrationEventLogEF.Entities.ExportIntegrationEventLog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id")
                        .HasDefaultValueSql("NEWSEQUENTIALID()");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("content");

                    b.Property<DateTimeOffset>("CreationTime")
                        .HasColumnType("datetimeoffset")
                        .HasColumnName("creation_time");

                    b.Property<string>("Error")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("error");

                    b.Property<Guid>("EventId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("event_id");

                    b.Property<string>("EventTypeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("event_type_name");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("nvarchar(25)")
                        .HasColumnName("state");

                    b.Property<int>("TimesSent")
                        .HasColumnType("int")
                        .HasColumnName("times_sent");

                    b.Property<Guid>("TransactionId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("transaction_id");

                    b.HasKey("Id");

                    b.HasIndex("EventId")
                        .IsUnique();

                    b.HasIndex("EventTypeName");

                    b.HasIndex("State");

                    b.HasIndex("EventTypeName", "State");

                    b.ToTable("export_integration_event_log", "logger");
                });

            modelBuilder.Entity("IntegrationEventLogEF.Entities.ImportSuccessIntegrationEventLog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id")
                        .HasDefaultValueSql("NEWSEQUENTIALID()");

                    b.Property<DateTimeOffset>("CreationTime")
                        .HasColumnType("datetimeoffset")
                        .HasColumnName("creation_time");

                    b.Property<Guid>("EventId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("event_id");

                    b.Property<string>("EventTypeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("event_type_name");

                    b.Property<DateTimeOffset?>("ProcessingEndDate")
                        .HasColumnType("datetimeoffset")
                        .HasColumnName("processing_end_date");

                    b.Property<Guid>("TransactionId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("transaction_id");

                    b.HasKey("Id");

                    b.HasIndex("EventId")
                        .IsUnique();

                    b.HasIndex("EventTypeName");

                    b.ToTable("import_integration_event_log", "logger");
                });
#pragma warning restore 612, 618
        }
    }
}
