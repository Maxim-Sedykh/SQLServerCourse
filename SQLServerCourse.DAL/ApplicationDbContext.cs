using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SQLServerCourse.Domain.Entity;
using SQLServerCourse.Domain.Entitys_for_lesson;
using SQLServerCourse.Domain.Enum;
using SQLServerCourse.Domain.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerCourse.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        //Системные таблицы
      
        public DbSet<User> Users { get; set; }

        public DbSet<Lesson> Lessons { get; set; }

        public DbSet<LessonRecord> LessonRecords { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<PracticalTask> PracticalTasks { get; set; }

        public DbSet<FrequentRemark> FrequentRemarks { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<TestVariant> TestVariants { get; set; }

        //Ниже таблицы для практических занятий курса, чтобы делать по ним запросы:

        public DbSet<Film> Films { get; set; }

        public DbSet<Hall> Halls { get; set; }

        public DbSet<HallRow> HallRows { get; set; }

        public DbSet<Screening> Screenings { get; set; }

        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Системные таблицы

            modelBuilder.Entity<User>(builder => 
            {
                builder.ToTable("Users").HasKey(x => x.Id);

                builder.HasData(new User
                {
                    Id = 1,
                    Login = "Admin123",
                    Name = "Maxim",
                    Surname = "Sedykh",
                    Password = HashPasswordHelper.HashPassword("1234567"),
                    Role = Role.Admin,
                    FinalGrade = 0,
                    LessonsCompleted = 0,
                    IsExamCompleted = false,
                },
                new User
                {
                    Id = 2,
                    Login = "CommonUser02",
                    Name = "Максим",
                    Surname = "Максимов",
                    Password = HashPasswordHelper.HashPassword("1234567"),
                    Role = Role.User,
                    FinalGrade = 0,
                    LessonsCompleted = 0,
                    IsExamCompleted = false,
                },
                new User
                {
                    Id = 3,
                    Login = "qwerty1234",
                    Name = "Петр",
                    Surname = "Петров",
                    Password = HashPasswordHelper.HashPassword("25252525"),
                    Role = Role.User,
                    FinalGrade = 0,
                    LessonsCompleted = 0,
                    IsExamCompleted = false,
                });

                builder.Property(x => x.Id).ValueGeneratedOnAdd();

                builder.Property(x => x.Password).IsRequired();
                builder.Property(x => x.Name).HasMaxLength(25).IsRequired();
                builder.Property(x => x.Login).HasMaxLength(12).IsRequired();
                builder.Property(x => x.Surname).HasMaxLength(25).IsRequired();
            });

            modelBuilder.Entity<Review>(builder =>
            {
                builder.ToTable("Reviews").HasKey(x => x.Id);

                builder.Property(x => x.Id).ValueGeneratedOnAdd();
                builder.Property(x => x.ReviewText).HasMaxLength(200).IsRequired();
            });

            modelBuilder.Entity<LessonRecord>(builder =>
            {
                builder.ToTable("LessonRecords").HasKey(x => x.Id);

                builder.Property(x => x.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<PracticalTask>(builder =>
            {
                builder.ToTable("PracticalTasks").HasKey(x => x.Id);

                builder.Property(x => x.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<FrequentRemark>(builder =>
            {
                builder.ToTable("FrequentRemarks").HasKey(x => x.Id);

                builder.Property(x => x.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Question>(builder =>
            {
                builder.ToTable("Questions").HasKey(x => x.Id);

                builder.Property(x => x.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<TestVariant>(builder =>
            {
                builder.ToTable("TestVariants").HasKey(x => x.Id);

                builder.Property(x => x.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Lesson>(builder => 
            {
                builder.ToTable("Lessons").HasKey(x => x.Id);

                builder.Property(x => x.Id).ValueGeneratedOnAdd();
            });

            // Таблицы для практических заданий

            modelBuilder.Entity<Film>(builder =>
            {
                builder.ToTable("Films").HasKey(x => x.Id);

                builder.Property(x => x.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Hall>(builder =>
            {
                builder.ToTable("Halls").HasKey(x => x.Id);

                builder.Property(x => x.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Screening>(builder =>
            {
                builder.ToTable("Screenings").HasKey(x => x.Id);

                builder.Property(x => x.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Ticket>(builder =>
            {
                builder.ToTable("Tickets").HasKey(x => x.Id);

                builder.Property(x => x.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<HallRow>(builder =>
            {
                builder.ToTable("HallRows").HasKey(x => x.Id);

                builder.Property(x => x.Id).ValueGeneratedOnAdd();
            });
        }
    }
}
