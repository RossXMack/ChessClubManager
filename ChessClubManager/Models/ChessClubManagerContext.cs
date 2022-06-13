using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ChessClubManager.Models
{
    public partial class ChessClubManagerContext : DbContext
    {
        public ChessClubManagerContext()
        {
        }

        public ChessClubManagerContext(DbContextOptions<ChessClubManagerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Match> Matches { get; set; }
        public virtual DbSet<MatchParticipant> MatchParticipants { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Match>(entity =>
            {
                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasMaxLength(36);

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.MatchDate).HasColumnType("datetime");

                entity.Property(e => e.Updated).HasColumnType("datetime");
            });

            modelBuilder.Entity<MatchParticipant>(entity =>
            {
                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasMaxLength(36);

                entity.Property(e => e.MatchId)
                    .IsRequired()
                    .HasMaxLength(36);

                entity.Property(e => e.MemberId)
                    .IsRequired()
                    .HasMaxLength(36);

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.Updated).HasColumnType("datetime");
            });

            modelBuilder.Entity<Member>(entity =>
            {
                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasMaxLength(36);

                entity.Property(e => e.Birthday).HasColumnType("date");

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(320);

                entity.Property(e => e.JoinDate).HasColumnType("date");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.Updated).HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
