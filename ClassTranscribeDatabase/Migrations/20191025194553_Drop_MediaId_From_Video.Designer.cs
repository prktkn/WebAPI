﻿// <auto-generated />
using System;
using ClassTranscribeDatabase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ClassTranscribeDatabase.Migrations
{
    [DbContext(typeof(CTDbContext))]
    [Migration("20191025194553_Drop_MediaId_From_Video")]
    partial class Drop_MediaId_From_Video
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("ClassTranscribeDatabase.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("Metadata");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<int>("Status");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UniversityId");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.HasIndex("UniversityId");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("ClassTranscribeDatabase.Models.Caption", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<TimeSpan>("Begin");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy");

                    b.Property<int>("DownVote");

                    b.Property<TimeSpan>("End");

                    b.Property<int>("Index");

                    b.Property<int>("IsDeletedStatus");

                    b.Property<DateTime>("LastUpdatedAt");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Text");

                    b.Property<string>("TranscriptionId");

                    b.Property<int>("UpVote");

                    b.HasKey("Id");

                    b.HasIndex("TranscriptionId");

                    b.ToTable("Captions");
                });

            modelBuilder.Entity("ClassTranscribeDatabase.Models.Course", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CourseNumber");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy");

                    b.Property<string>("DepartmentId");

                    b.Property<int>("IsDeletedStatus");

                    b.Property<DateTime>("LastUpdatedAt");

                    b.Property<string>("LastUpdatedBy");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("ClassTranscribeDatabase.Models.CourseOffering", b =>
                {
                    b.Property<string>("CourseId");

                    b.Property<string>("OfferingId");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy");

                    b.Property<string>("Id");

                    b.Property<int>("IsDeletedStatus");

                    b.Property<DateTime>("LastUpdatedAt");

                    b.Property<string>("LastUpdatedBy");

                    b.HasKey("CourseId", "OfferingId");

                    b.HasIndex("OfferingId");

                    b.ToTable("CourseOfferings");
                });

            modelBuilder.Entity("ClassTranscribeDatabase.Models.Department", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Acronym");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy");

                    b.Property<int>("IsDeletedStatus");

                    b.Property<DateTime>("LastUpdatedAt");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Name");

                    b.Property<string>("UniversityId");

                    b.HasKey("Id");

                    b.HasIndex("UniversityId");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("ClassTranscribeDatabase.Models.FileRecord", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy");

                    b.Property<string>("FileName");

                    b.Property<string>("Hash");

                    b.Property<int>("IsDeletedStatus");

                    b.Property<DateTime>("LastUpdatedAt");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("PrivatePath");

                    b.HasKey("Id");

                    b.ToTable("FileRecords");
                });

            modelBuilder.Entity("ClassTranscribeDatabase.Models.Log", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy");

                    b.Property<string>("EventType");

                    b.Property<int>("IsDeletedStatus");

                    b.Property<string>("Json");

                    b.Property<DateTime>("LastUpdatedAt");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("MediaId");

                    b.Property<string>("OfferingId");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.ToTable("Logs");
                });

            modelBuilder.Entity("ClassTranscribeDatabase.Models.Media", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy");

                    b.Property<int>("IsDeletedStatus");

                    b.Property<string>("JsonMetadata");

                    b.Property<DateTime>("LastUpdatedAt");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("PlaylistId");

                    b.Property<int>("SourceType");

                    b.Property<string>("UniqueMediaIdentifier");

                    b.Property<string>("VideoId");

                    b.HasKey("Id");

                    b.HasIndex("PlaylistId");

                    b.ToTable("Medias");
                });

            modelBuilder.Entity("ClassTranscribeDatabase.Models.Offering", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessType");

                    b.Property<string>("CourseName");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy");

                    b.Property<string>("Description");

                    b.Property<int>("IsDeletedStatus");

                    b.Property<DateTime>("LastUpdatedAt");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<bool>("LogEventsFlag");

                    b.Property<string>("SectionName");

                    b.Property<string>("TermId");

                    b.HasKey("Id");

                    b.HasIndex("TermId");

                    b.ToTable("Offerings");
                });

            modelBuilder.Entity("ClassTranscribeDatabase.Models.Playlist", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy");

                    b.Property<int>("IsDeletedStatus");

                    b.Property<DateTime>("LastUpdatedAt");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Name");

                    b.Property<string>("OfferingId");

                    b.Property<string>("PlaylistIdentifier");

                    b.Property<int>("SourceType");

                    b.HasKey("Id");

                    b.HasIndex("OfferingId");

                    b.ToTable("Playlists");
                });

            modelBuilder.Entity("ClassTranscribeDatabase.Models.Term", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("EndDate");

                    b.Property<int>("IsDeletedStatus");

                    b.Property<DateTime>("LastUpdatedAt");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Name");

                    b.Property<DateTime>("StartDate");

                    b.Property<string>("UniversityId");

                    b.HasKey("Id");

                    b.HasIndex("UniversityId");

                    b.ToTable("Terms");
                });

            modelBuilder.Entity("ClassTranscribeDatabase.Models.Transcription", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy");

                    b.Property<string>("Description");

                    b.Property<string>("FileId");

                    b.Property<int>("IsDeletedStatus");

                    b.Property<string>("Language");

                    b.Property<DateTime>("LastUpdatedAt");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("MediaId");

                    b.HasKey("Id");

                    b.HasIndex("FileId");

                    b.HasIndex("MediaId");

                    b.ToTable("Transcriptions");
                });

            modelBuilder.Entity("ClassTranscribeDatabase.Models.University", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy");

                    b.Property<string>("Domain");

                    b.Property<int>("IsDeletedStatus");

                    b.Property<DateTime>("LastUpdatedAt");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Universities");
                });

            modelBuilder.Entity("ClassTranscribeDatabase.Models.UserOffering", b =>
                {
                    b.Property<string>("ApplicationUserId");

                    b.Property<string>("OfferingId");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy");

                    b.Property<string>("Id");

                    b.Property<string>("IdentityRoleId");

                    b.Property<int>("IsDeletedStatus");

                    b.Property<DateTime>("LastUpdatedAt");

                    b.Property<string>("LastUpdatedBy");

                    b.HasKey("ApplicationUserId", "OfferingId");

                    b.HasIndex("IdentityRoleId");

                    b.HasIndex("OfferingId");

                    b.ToTable("UserOfferings");
                });

            modelBuilder.Entity("ClassTranscribeDatabase.Models.Video", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AudioId");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy");

                    b.Property<string>("Description");

                    b.Property<int>("IsDeletedStatus");

                    b.Property<DateTime>("LastUpdatedAt");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("MediaId");

                    b.Property<string>("ProcessedVideo1Id");

                    b.Property<string>("ProcessedVideo2Id");

                    b.Property<string>("TranscriptionStatus");

                    b.Property<string>("Video1Id");

                    b.Property<string>("Video2Id");

                    b.HasKey("Id");

                    b.HasIndex("AudioId");

                    b.HasIndex("MediaId");

                    b.HasIndex("ProcessedVideo1Id");

                    b.HasIndex("ProcessedVideo2Id");

                    b.HasIndex("Video1Id");

                    b.HasIndex("Video2Id");

                    b.ToTable("Videos");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("ClassTranscribeDatabase.Models.ApplicationUser", b =>
                {
                    b.HasOne("ClassTranscribeDatabase.Models.University", "University")
                        .WithMany()
                        .HasForeignKey("UniversityId");
                });

            modelBuilder.Entity("ClassTranscribeDatabase.Models.Caption", b =>
                {
                    b.HasOne("ClassTranscribeDatabase.Models.Transcription", "Transcription")
                        .WithMany("Captions")
                        .HasForeignKey("TranscriptionId");
                });

            modelBuilder.Entity("ClassTranscribeDatabase.Models.Course", b =>
                {
                    b.HasOne("ClassTranscribeDatabase.Models.Department", "Department")
                        .WithMany("Courses")
                        .HasForeignKey("DepartmentId");
                });

            modelBuilder.Entity("ClassTranscribeDatabase.Models.CourseOffering", b =>
                {
                    b.HasOne("ClassTranscribeDatabase.Models.Course", "Course")
                        .WithMany("CourseOfferings")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ClassTranscribeDatabase.Models.Offering", "Offering")
                        .WithMany("CourseOfferings")
                        .HasForeignKey("OfferingId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ClassTranscribeDatabase.Models.Department", b =>
                {
                    b.HasOne("ClassTranscribeDatabase.Models.University", "University")
                        .WithMany("Departments")
                        .HasForeignKey("UniversityId");
                });

            modelBuilder.Entity("ClassTranscribeDatabase.Models.Media", b =>
                {
                    b.HasOne("ClassTranscribeDatabase.Models.Playlist", "Playlist")
                        .WithMany("Medias")
                        .HasForeignKey("PlaylistId");
                });

            modelBuilder.Entity("ClassTranscribeDatabase.Models.Offering", b =>
                {
                    b.HasOne("ClassTranscribeDatabase.Models.Term", "Term")
                        .WithMany("Offerings")
                        .HasForeignKey("TermId");
                });

            modelBuilder.Entity("ClassTranscribeDatabase.Models.Playlist", b =>
                {
                    b.HasOne("ClassTranscribeDatabase.Models.Offering", "Offering")
                        .WithMany("Playlists")
                        .HasForeignKey("OfferingId");
                });

            modelBuilder.Entity("ClassTranscribeDatabase.Models.Term", b =>
                {
                    b.HasOne("ClassTranscribeDatabase.Models.University", "University")
                        .WithMany("Terms")
                        .HasForeignKey("UniversityId");
                });

            modelBuilder.Entity("ClassTranscribeDatabase.Models.Transcription", b =>
                {
                    b.HasOne("ClassTranscribeDatabase.Models.FileRecord", "File")
                        .WithMany()
                        .HasForeignKey("FileId");

                    b.HasOne("ClassTranscribeDatabase.Models.Media", "Media")
                        .WithMany("Transcriptions")
                        .HasForeignKey("MediaId");
                });

            modelBuilder.Entity("ClassTranscribeDatabase.Models.UserOffering", b =>
                {
                    b.HasOne("ClassTranscribeDatabase.Models.ApplicationUser", "ApplicationUser")
                        .WithMany("UserOfferings")
                        .HasForeignKey("ApplicationUserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", "IdentityRole")
                        .WithMany()
                        .HasForeignKey("IdentityRoleId");

                    b.HasOne("ClassTranscribeDatabase.Models.Offering", "Offering")
                        .WithMany("OfferingUsers")
                        .HasForeignKey("OfferingId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ClassTranscribeDatabase.Models.Video", b =>
                {
                    b.HasOne("ClassTranscribeDatabase.Models.FileRecord", "Audio")
                        .WithMany()
                        .HasForeignKey("AudioId");

                    b.HasOne("ClassTranscribeDatabase.Models.Media")
                        .WithMany("Videos")
                        .HasForeignKey("MediaId");

                    b.HasOne("ClassTranscribeDatabase.Models.FileRecord", "ProcessedVideo1")
                        .WithMany()
                        .HasForeignKey("ProcessedVideo1Id");

                    b.HasOne("ClassTranscribeDatabase.Models.FileRecord", "ProcessedVideo2")
                        .WithMany()
                        .HasForeignKey("ProcessedVideo2Id");

                    b.HasOne("ClassTranscribeDatabase.Models.FileRecord", "Video1")
                        .WithMany()
                        .HasForeignKey("Video1Id");

                    b.HasOne("ClassTranscribeDatabase.Models.FileRecord", "Video2")
                        .WithMany()
                        .HasForeignKey("Video2Id");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("ClassTranscribeDatabase.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("ClassTranscribeDatabase.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ClassTranscribeDatabase.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("ClassTranscribeDatabase.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
