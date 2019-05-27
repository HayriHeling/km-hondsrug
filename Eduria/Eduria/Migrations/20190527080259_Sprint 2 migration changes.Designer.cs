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
    [Migration("20190527080259_Sprint 2 migration changes")]
    partial class Sprint2migrationchanges
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

                    b.Property<int>("Period");

                    b.Property<string>("Reflection");

                    b.Property<string>("UniqueMethodName");

                    b.Property<int>("UniqueMethodScore");

                    b.Property<int>("UserId");

                    b.Property<int>("Year");

                    b.HasKey("AnalyticDataId");

                    b.ToTable("AnalyticDatas");
                });

            modelBuilder.Entity("EduriaData.Models.AnalyticLayer.AnalyticGoal", b =>
                {
                    b.Property<int>("AnalyticGoalId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AnalyticDataId");

                    b.Property<string>("AnalyticGoalName")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<int>("AnalyticGoalScore");

                    b.HasKey("AnalyticGoalId");

                    b.ToTable("AnalyticGoals");
                });

            modelBuilder.Entity("EduriaData.Models.AnalyticLayer.AnalyticMethod", b =>
                {
                    b.Property<int>("AnalyticMethodId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AnalyticDataId");

                    b.Property<string>("AnalyticMethodName")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<int>("AnalyticMethodScore");

                    b.HasKey("AnalyticMethodId");

                    b.ToTable("AnalyticMethods");
                });

            modelBuilder.Entity("EduriaData.Models.Answer", b =>
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

            modelBuilder.Entity("EduriaData.Models.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasMaxLength(45);

                    b.HasKey("CategoryId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("EduriaData.Models.Exam", b =>
                {
                    b.Property<int>("ExamId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CategoryId");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(45);

                    b.HasKey("ExamId");

                    b.ToTable("Exams");
                });

            modelBuilder.Entity("EduriaData.Models.ExamLayer.AnswerT", b =>
                {
                    b.Property<int>("AnswerTId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Source")
                        .IsRequired();

                    b.Property<string>("Text")
                        .IsRequired();

                    b.HasKey("AnswerTId");

                    b.ToTable("AnswerTs");
                });

            modelBuilder.Entity("EduriaData.Models.ExamLayer.QuestionHasAnswerT", b =>
                {
                    b.Property<int>("QuestionHasAnswerTId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AnswerTId");

                    b.Property<int>("QuestionId");

                    b.HasKey("QuestionHasAnswerTId");

                    b.ToTable("QuestionHasAnswerTs");
                });

            modelBuilder.Entity("EduriaData.Models.ExamQuestion", b =>
                {
                    b.Property<int>("ExamHasQuestionId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ExamId");

                    b.Property<int>("QuestionId");

                    b.HasKey("ExamHasQuestionId");

                    b.ToTable("ExamQuestions");
                });

            modelBuilder.Entity("EduriaData.Models.ExamResult", b =>
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

            modelBuilder.Entity("EduriaData.Models.Question", b =>
                {
                    b.Property<int>("QuestionId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CategoryId");

                    b.Property<string>("MediaLink")
                        .IsRequired()
                        .HasMaxLength(500);

                    b.Property<int>("MediaType");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.HasKey("QuestionId");

                    b.ToTable("Questions");
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

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<int>("StudNum");

                    b.Property<string>("Token")
                        .HasMaxLength(200);

                    b.Property<int>("UserType");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("EduriaData.Models.UserEQLog", b =>
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
#pragma warning restore 612, 618
        }
    }
}
