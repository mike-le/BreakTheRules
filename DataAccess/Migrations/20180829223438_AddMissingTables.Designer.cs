﻿// <auto-generated />
using System;
using BTR.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataAccess.Migrations
{
    [DbContext(typeof(BTRContext))]
    [Migration("20180829223438_AddMissingTables")]
    partial class AddMissingTables
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BTR.DataAccess.Entities.Audit", b =>
                {
                    b.Property<int>("OwnerId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("ChangeDt");

                    b.Property<string>("ChangeOwner");

                    b.Property<string>("KeyValues");

                    b.Property<string>("NewValues");

                    b.Property<string>("OldValues");

                    b.Property<string>("TableName");

                    b.HasKey("OwnerId");

                    b.ToTable("Audits");
                });

            modelBuilder.Entity("BTR.DataAccess.Entities.CommentEntity", b =>
                {
                    b.Property<int>("CommentId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Message")
                        .IsRequired();

                    b.Property<DateTime>("ModifiedDt");

                    b.Property<string>("Owner")
                        .IsRequired();

                    b.Property<int?>("ParentCommentId");

                    b.Property<int?>("ParentIdeaId");

                    b.Property<DateTime>("SubmitDt");

                    b.HasKey("CommentId");

                    b.HasIndex("ParentCommentId");

                    b.HasIndex("ParentIdeaId");

                    b.ToTable("ApiComments");
                });

            modelBuilder.Entity("BTR.DataAccess.Entities.IdeaEntity", b =>
                {
                    b.Property<int>("PostId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Message")
                        .IsRequired();

                    b.Property<DateTime>("ModifiedDt");

                    b.Property<string>("Owner")
                        .IsRequired();

                    b.Property<DateTime>("SubmitDt");

                    b.Property<int>("ThemeId");

                    b.HasKey("PostId");

                    b.HasIndex("ThemeId");

                    b.ToTable("ApiIdeas");
                });

            modelBuilder.Entity("BTR.DataAccess.Entities.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsExec");

                    b.Property<int>("StatusId");

                    b.Property<DateTime>("createDt");

                    b.HasKey("Id");

                    b.HasIndex("StatusId")
                        .IsUnique();

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("BTR.DataAccess.Entities.Status", b =>
                {
                    b.Property<int>("StatusId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("IdeaId");

                    b.Property<string>("Response");

                    b.Property<int>("StatusCode");

                    b.Property<DateTime>("SubmitDt");

                    b.HasKey("StatusId");

                    b.HasIndex("IdeaId");

                    b.ToTable("Statuses");
                });

            modelBuilder.Entity("BTR.DataAccess.Entities.ThemeEntity", b =>
                {
                    b.Property<int>("ThemeId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CloseDt");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<DateTime>("OpenDt");

                    b.Property<string>("Owner");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("ThemeId");

                    b.ToTable("ApiThemes");
                });

            modelBuilder.Entity("BTR.DataAccess.Entities.Vote", b =>
                {
                    b.Property<int>("VoteId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CommentId");

                    b.Property<int>("Direction");

                    b.Property<int?>("IdeaId");

                    b.Property<string>("Owner");

                    b.Property<DateTime>("SubmitDt");

                    b.HasKey("VoteId");

                    b.HasIndex("CommentId");

                    b.HasIndex("IdeaId");

                    b.ToTable("Votes");
                });

            modelBuilder.Entity("BTR.DataAccess.Entities.CommentEntity", b =>
                {
                    b.HasOne("BTR.DataAccess.Entities.CommentEntity", "ParentComment")
                        .WithMany("Comments")
                        .HasForeignKey("ParentCommentId");

                    b.HasOne("BTR.DataAccess.Entities.IdeaEntity", "ParentIdea")
                        .WithMany("Comments")
                        .HasForeignKey("ParentIdeaId");
                });

            modelBuilder.Entity("BTR.DataAccess.Entities.IdeaEntity", b =>
                {
                    b.HasOne("BTR.DataAccess.Entities.ThemeEntity", "Theme")
                        .WithMany("Ideas")
                        .HasForeignKey("ThemeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BTR.DataAccess.Entities.Notification", b =>
                {
                    b.HasOne("BTR.DataAccess.Entities.Status", "Status")
                        .WithOne("Notification")
                        .HasForeignKey("BTR.DataAccess.Entities.Notification", "StatusId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BTR.DataAccess.Entities.Status", b =>
                {
                    b.HasOne("BTR.DataAccess.Entities.IdeaEntity", "Idea")
                        .WithMany("IdeaStatus")
                        .HasForeignKey("IdeaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BTR.DataAccess.Entities.Vote", b =>
                {
                    b.HasOne("BTR.DataAccess.Entities.CommentEntity", "Comment")
                        .WithMany("Votes")
                        .HasForeignKey("CommentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BTR.DataAccess.Entities.IdeaEntity", "Idea")
                        .WithMany("Votes")
                        .HasForeignKey("IdeaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
