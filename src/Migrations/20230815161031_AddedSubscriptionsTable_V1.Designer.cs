﻿// <auto-generated />
using EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Src.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20230815161031_AddedSubscriptionsTable_V1")]
    partial class AddedSubscriptionsTable_V1
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("EF.Models.Auth", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("Id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(320)
                        .HasColumnType("character varying(320)")
                        .HasColumnName("Email");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("boolean")
                        .HasColumnName("IsAdmin");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(120)
                        .HasColumnType("character varying(120)")
                        .HasColumnName("Password");

                    b.HasKey("Id");

                    b.ToTable("Auth");
                });

            modelBuilder.Entity("EF.Models.Friends", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("Id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("FirstProfileId")
                        .HasColumnType("integer");

                    b.Property<int>("SecondProfileId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("FirstProfileId");

                    b.HasIndex("SecondProfileId");

                    b.ToTable("Friends");
                });

            modelBuilder.Entity("EF.Models.Profile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("Id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AuthId")
                        .HasColumnType("integer");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("FirstName");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("LastName");

                    b.Property<string>("Nickname")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("Nickname");

                    b.Property<string>("ProfileName")
                        .IsRequired()
                        .HasMaxLength(9)
                        .HasColumnType("character varying(9)")
                        .HasColumnName("ProfileName");

                    b.HasKey("Id");

                    b.HasIndex("AuthId");

                    b.ToTable("Profile");
                });

            modelBuilder.Entity("EF.Models.Subscription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("Id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("SubscribedId")
                        .HasColumnType("integer");

                    b.Property<int>("SubscribingId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("SubscribedId");

                    b.HasIndex("SubscribingId");

                    b.ToTable("Subscription");
                });

            modelBuilder.Entity("EF.Models.Friends", b =>
                {
                    b.HasOne("EF.Models.Profile", "FirstProfile")
                        .WithMany()
                        .HasForeignKey("FirstProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EF.Models.Profile", "SecondProfile")
                        .WithMany()
                        .HasForeignKey("SecondProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FirstProfile");

                    b.Navigation("SecondProfile");
                });

            modelBuilder.Entity("EF.Models.Profile", b =>
                {
                    b.HasOne("EF.Models.Auth", "Auth")
                        .WithMany()
                        .HasForeignKey("AuthId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Auth");
                });

            modelBuilder.Entity("EF.Models.Subscription", b =>
                {
                    b.HasOne("EF.Models.Profile", "SubscribedProfile")
                        .WithMany()
                        .HasForeignKey("SubscribedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EF.Models.Profile", "SubscribingProfile")
                        .WithMany()
                        .HasForeignKey("SubscribingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SubscribedProfile");

                    b.Navigation("SubscribingProfile");
                });
#pragma warning restore 612, 618
        }
    }
}
