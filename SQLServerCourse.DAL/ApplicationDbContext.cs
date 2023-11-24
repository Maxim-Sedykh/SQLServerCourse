using Funq;
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

        public DbSet<Question> Questions { get; set; }

        public DbSet<TestVariant> TestVariants { get; set; }

        public DbSet<UserProfile> UserProfiles { get; set; }

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
                builder.ToTable("Users").HasKey(u => u.Id);

                builder.HasData(new User
                {
                    Id = 1,
                    Login = "Admin123",
                    Password = HashPasswordHelper.HashPassword("1234567"),
                    Role = Role.Admin
                },
                new User
                {
                    Id = 2,
                    Login = "CommonUser02",
                    Password = HashPasswordHelper.HashPassword("1234567"),
                    Role = Role.User
                },
                new User
                {
                    Id = 3,
                    Login = "qwerty1234",
                    Password = HashPasswordHelper.HashPassword("25252525"),
                    Role = Role.User
                });

                builder.Property(u => u.Id).ValueGeneratedOnAdd();
                builder.Property(u => u.Password).IsRequired();
                builder.Property(u => u.Login).HasMaxLength(12).IsRequired();

                builder.HasOne(u => u.UserProfile)
                    .WithOne(up => up.User)
                    .HasPrincipalKey<User>(u => u.Id)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<UserProfile>(builder =>
            {
                builder.ToTable("UserProfiles").HasKey(up => up.Id);

                builder.HasData(new UserProfile
                {
                    Id = 1,
                    UserId = 1,
                },
                new UserProfile
                {
                    Id = 2,
                    UserId = 2,
                },
                new UserProfile
                {
                    Id = 3, 
                    UserId = 3,
                });

                builder.Property(up => up.Id).ValueGeneratedOnAdd();
                builder.Property(up => up.Name).HasMaxLength(50).IsRequired(false);
                builder.Property(up => up.Surname).HasMaxLength(50).IsRequired(false);
                builder.Property(up => up.Analys).HasMaxLength(150).IsRequired(false);
            });

            modelBuilder.Entity<Review>(builder =>
            {
                builder.ToTable("Reviews").HasKey(r => r.Id);

                builder.Property(r => r.Id).ValueGeneratedOnAdd();
                builder.Property(r => r.ReviewText).HasMaxLength(200).IsRequired();

                builder.HasOne(r => r.User)
                    .WithMany(u => u.Reviews)
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<LessonRecord>(builder =>
            {
                builder.ToTable("LessonRecords").HasKey(lr => lr.Id);

                builder.Property(lr => lr.Id).ValueGeneratedOnAdd();

                builder.HasOne(lr => lr.User)
                    .WithMany(u => u.LessonRecords)
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                builder.HasOne(lr => lr.Lesson)
                    .WithMany(l => l.LessonRecords)
                    .HasForeignKey(r => r.LessonId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Question>(builder =>
            {
                builder.ToTable("Questions").HasKey(x => x.Id);

                builder.Property(x => x.Id).ValueGeneratedOnAdd();
                builder.Property(r => r.DisplayQuestion).HasMaxLength(100).IsRequired();
                builder.Property(r => r.Answer).HasMaxLength(30).IsRequired();

                builder.HasOne(q => q.Lesson)
                    .WithMany(l => l.Questions)
                    .HasForeignKey(q => q.LessonId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<TestVariant>(builder =>
            {
                builder.ToTable("TestVariants").HasKey(x => x.Id);

                builder.Property(tv => tv.Id).ValueGeneratedOnAdd();
                builder.Property(tv => tv.Content).HasMaxLength(100).IsRequired();

                builder.HasOne(tv => tv.Question)
                    .WithMany(q => q.TestVariants)
                    .HasForeignKey(tv => tv.QuestionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Lesson>(builder => 
            {
                builder.ToTable("Lessons").HasKey(x => x.Id);

                builder.Property(l => l.Id).ValueGeneratedOnAdd();
                builder.Property(l => l.Name).HasMaxLength(75).IsRequired();
                builder.Property(l => l.LectureMarkup).HasMaxLength(5000).IsRequired();
            });

            // Таблицы для практических заданий

            modelBuilder.Entity<Film>(builder =>
            {
                builder.ToTable("Films").HasKey(x => x.Id);

                builder.Property(x => x.Id).ValueGeneratedOnAdd();
                builder.Property(l => l.Name).HasMaxLength(50).IsRequired();
                builder.Property(l => l.Description).HasMaxLength(100).IsRequired(false);
            });

            modelBuilder.Entity<Hall>(builder =>
            {
                builder.ToTable("Halls").HasKey(x => x.Id);

                builder.Property(x => x.Id).ValueGeneratedOnAdd();
                builder.Property(l => l.Name).HasMaxLength(50).IsRequired();
            });

            modelBuilder.Entity<Screening>(builder =>
            {
                builder.ToTable("Screenings").HasKey(x => x.Id);

                builder.Property(x => x.Id).ValueGeneratedOnAdd();

                builder.HasOne(tv => tv.Hall)
                    .WithMany(q => q.Screenings)
                    .HasForeignKey(tv => tv.HallId)
                    .OnDelete(DeleteBehavior.Cascade);

                builder.HasOne(tv => tv.Film)
                    .WithMany(q => q.Screenings)
                    .HasForeignKey(tv => tv.FilmId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Ticket>(builder =>
            {
                builder.ToTable("Tickets").HasKey(x => x.Id);

                builder.Property(x => x.Id).ValueGeneratedOnAdd();

                builder.HasOne(tv => tv.Screening)
                    .WithMany(q => q.Tickets)
                    .HasForeignKey(tv => tv.ScreeningId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<HallRow>(builder =>
            {
                builder.ToTable("HallRows").HasKey(x => x.Id);

                builder.Property(x => x.Id).ValueGeneratedOnAdd();

                builder.HasOne(tv => tv.Hall)
                    .WithMany(q => q.HallRows)
                    .HasForeignKey(tv => tv.HallId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
