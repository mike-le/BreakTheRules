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
    [Migration("20180706194812_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BTR.DataAccess.Entities.CommentEntity", b =>
                {
                    b.Property<int>("CommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("comment_id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CommentScore")
                        .HasColumnName("comment_score");

                    b.Property<string>("Message")
                        .HasColumnName("message_text");

                    b.Property<string>("Owner")
                        .HasColumnName("owner_name");

                    b.Property<int>("OwnerId")
                        .HasColumnName("owner_id");

                    b.Property<DateTime>("SubmitDt")
                        .HasColumnName("submit_dt");

                    b.HasKey("CommentId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("BTR.DataAccess.Entities.IdeaEntity", b =>
                {
                    b.Property<int>("PostId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("post_id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Message")
                        .HasColumnName("message_text");

                    b.Property<DateTime>("ModifiedDt")
                        .HasColumnName("modified_dt");

                    b.Property<string>("Owner")
                        .HasColumnName("owner_name");

                    b.Property<int>("OwnerId")
                        .HasColumnName("owner_id");

                    b.Property<int>("ParentId")
                        .HasColumnName("parent_id");

                    b.Property<string>("Score")
                        .HasColumnName("post_score");

                    b.Property<int>("Status")
                        .HasColumnName("status");

                    b.Property<DateTime>("SubmitDt")
                        .HasColumnName("submit_dt");

                    b.HasKey("PostId");

                    b.ToTable("Ideas");
                });

            modelBuilder.Entity("BTR.DataAccess.Entities.ThemeEntity", b =>
                {
                    b.Property<int>("ThemeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("theme_id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CloseDt")
                        .HasColumnName("close_dt");

                    b.Property<string>("Description")
                        .HasColumnName("description");

                    b.Property<DateTime>("OpenDt")
                        .HasColumnName("open_dt");

                    b.Property<string>("Owner")
                        .HasColumnName("owner_name");

                    b.Property<int>("OwnerId")
                        .HasColumnName("owner_id");

                    b.Property<string>("Title")
                        .HasColumnName("title");

                    b.HasKey("ThemeId");

                    b.ToTable("Themes");
                });
#pragma warning restore 612, 618
        }
    }
}
