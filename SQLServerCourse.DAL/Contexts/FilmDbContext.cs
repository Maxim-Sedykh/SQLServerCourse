using Microsoft.EntityFrameworkCore;
using SQLServerCourse.Domain.Entity;
using SQLServerCourse.Domain.Entitys_for_lesson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerCourse.DAL.Contexts
{
    public class FilmDbContext : DbContext
    {
        public FilmDbContext(DbContextOptions<FilmDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Film> Films { get; set; }

        public DbSet<Hall> Halls { get; set; }

        public DbSet<HallRow> HallRows { get; set; }

        public DbSet<Screening> Screenings { get; set; }

        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Film>(builder =>
            {
                builder.ToTable("Films").HasKey(x => x.Id);

                builder.Property(x => x.Id).ValueGeneratedOnAdd();
                builder.Property(l => l.Name).HasMaxLength(50).IsRequired();
                builder.Property(l => l.Description).HasMaxLength(200).IsRequired(false);
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
