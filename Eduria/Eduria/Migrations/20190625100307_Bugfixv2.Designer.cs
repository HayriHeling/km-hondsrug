﻿// <auto-generated />
using System;
using Eduria;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Eduria.Migrations
{
    [DbContext(typeof(EduriaContext))]
    [Migration("20190625100307_Bugfixv2")]
    partial class Bugfixv2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EduriaData.Models.AnalyticLayer.AnalyticData", b =>
                {
                    b.Property<int>("AnalyticDataId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ExamCode")
                        .IsRequired();

                    b.Property<int>("PeriodId");

                    b.Property<int>("UserId");

                    b.HasKey("AnalyticDataId");

                    b.ToTable("AnalyticDatas");
                });

            modelBuilder.Entity("EduriaData.Models.AnalyticLayer.AnalyticDefault", b =>
                {
                    b.Property<int>("AnalyticDefaultId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AnalyticCategory");

                    b.Property<string>("AnalyticDefaultName")
                        .IsRequired();

                    b.Property<int>("AnalyticDefaultOption");

                    b.HasKey("AnalyticDefaultId");

                    b.ToTable("AnalyticDefaults");
                });

            modelBuilder.Entity("EduriaData.Models.AnalyticLayer.DataHasDefault", b =>
                {
                    b.Property<int>("DataHasDefaultId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AnalyticDataId");

                    b.Property<int>("AnalyticDefaultId");

                    b.HasKey("DataHasDefaultId");

                    b.ToTable("DataHasDefaults");
                });

            modelBuilder.Entity("EduriaData.Models.AnalyticLayer.DefaultDataInput", b =>
                {
                    b.Property<int>("DefaultDataInputId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DataHasDefaultId");

                    b.Property<string>("Text")
                        .IsRequired();

                    b.HasKey("DefaultDataInputId");

                    b.ToTable("DefaultDataInputs");
                });

            modelBuilder.Entity("EduriaData.Models.AnalyticLayer.DefaultDataScore", b =>
                {
                    b.Property<int>("DefaultDateScoreId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DataHasDefaultId");

                    b.Property<int>("Score");

                    b.HasKey("DefaultDateScoreId");

                    b.ToTable("DefaultDataScores");
                });

            modelBuilder.Entity("EduriaData.Models.AnalyticLayer.Period", b =>
                {
                    b.Property<int>("PeriodId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("PeriodEnd");

                    b.Property<int>("PeriodNum");

                    b.Property<DateTime>("PeriodStart");

                    b.Property<int>("SchoolYearEnd");

                    b.Property<int>("SchoolYearStart");

                    b.HasKey("PeriodId");

                    b.ToTable("Periods");
                });

            modelBuilder.Entity("EduriaData.Models.Config", b =>
                {
                    b.Property<int>("ConfigId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<DateTime>("EntryChangedAt");

                    b.Property<string>("FromMail")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<string>("Host")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<int>("SMTPPort");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.HasKey("ConfigId");

                    b.ToTable("Configs");
                });

            modelBuilder.Entity("EduriaData.Models.ExamLayer.Answer", b =>
                {
                    b.Property<int>("AnswerId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Correct");

                    b.Property<int>("QuestionId");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.HasKey("AnswerId");

                    b.ToTable("Answers");
                });

            modelBuilder.Entity("EduriaData.Models.ExamLayer.Exam", b =>
                {
                    b.Property<int>("ExamId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<int>("IsActive");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(45);

                    b.Property<int>("TimeTableId");

                    b.HasKey("ExamId");

                    b.ToTable("Exams");
                });

            modelBuilder.Entity("EduriaData.Models.ExamLayer.ExamQuestion", b =>
                {
                    b.Property<int>("ExamHasQuestionId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ExamId");

                    b.Property<int>("QuestionId");

                    b.HasKey("ExamHasQuestionId");

                    b.ToTable("ExamQuestions");
                });

            modelBuilder.Entity("EduriaData.Models.ExamLayer.ExamResult", b =>
                {
                    b.Property<int>("ExamResultId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ExamId");

                    b.Property<DateTime>("FinishedAt");

                    b.Property<int>("Score");

                    b.Property<DateTime>("StartedAt");

                    b.Property<int>("UserId");

                    b.HasKey("ExamResultId");

                    b.ToTable("ExamResults");
                });

            modelBuilder.Entity("EduriaData.Models.ExamLayer.Question", b =>
                {
                    b.Property<int>("QuestionId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("MediaSourceId");

                    b.Property<int>("QuestionType");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<int>("TimeTableId");

                    b.HasKey("QuestionId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("EduriaData.Models.ExamLayer.UserEQLog", b =>
                {
                    b.Property<int>("UserEQLogId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AnsweredOn");

                    b.Property<int>("CorrectAnswered");

                    b.Property<int>("ExamHasQuestionId");

                    b.Property<int>("ExamResultId");

                    b.Property<int>("TimesWrong");

                    b.Property<int>("UserId");

                    b.HasKey("UserEQLogId");

                    b.ToTable("UserEQLogs");
                });

            modelBuilder.Entity("EduriaData.Models.MediaSource", b =>
                {
                    b.Property<int>("MediaSourceId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("MediaType");

                    b.Property<string>("Source")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.HasKey("MediaSourceId");

                    b.ToTable("MediaSources");
                });

            modelBuilder.Entity("EduriaData.Models.TimeLineLayer.TimeTableInfoHasMediaSrc", b =>
                {
                    b.Property<int>("TimeTableInfoHasMediaSrcId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("MediaSourceId");

                    b.Property<int>("TimeTableInformationId");

                    b.HasKey("TimeTableInfoHasMediaSrcId");

                    b.ToTable("TimeTableInfoHasMediaSrcs");
                });

            modelBuilder.Entity("EduriaData.Models.TimeLineLayer.TimeTableInformation", b =>
                {
                    b.Property<int>("TimeTableInformationId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BeforeChrist");

                    b.Property<string>("Description")
                        .HasMaxLength(2147483647);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int>("TimeTableId");

                    b.Property<int>("UserId");

                    b.Property<int>("Year");

                    b.HasKey("TimeTableInformationId");

                    b.ToTable("TimeTableInformations");
                });

            modelBuilder.Entity("EduriaData.Models.TimeTable", b =>
                {
                    b.Property<int>("TimeTableId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<int>("MediaSourceId");

                    b.Property<string>("Text")
                        .IsRequired();

                    b.Property<string>("TimeTableDesignId")
                        .IsRequired();

                    b.HasKey("TimeTableId");

                    b.ToTable("TimeTables");
                });

            modelBuilder.Entity("EduriaData.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ClassId");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasMaxLength(45);

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasMaxLength(45);

                    b.Property<int>("MediaSourceId");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<string>("Token")
                        .HasMaxLength(200);

                    b.Property<int>("UserNum");

                    b.Property<int>("UserType");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
