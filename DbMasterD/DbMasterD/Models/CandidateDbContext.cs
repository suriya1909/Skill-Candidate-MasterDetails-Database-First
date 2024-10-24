using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DbMasterD.Models;

public partial class CandidateDbContext : DbContext
{
    public CandidateDbContext()
    {
    }

    public CandidateDbContext(DbContextOptions<CandidateDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Candidate> Candidates { get; set; }

    public virtual DbSet<CandidateSkill> CandidateSkills { get; set; }

    public virtual DbSet<Skill> Skills { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

        => optionsBuilder.UseSqlServer("Server=DESKTOP-5903S8A;Database=candidate_db;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Candidate>(entity =>
        {
            entity.HasKey(e => e.CandidateId).HasName("PK__Candidat__DF539B9C94B4F6ED");

            entity.Property(e => e.CandidateName).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(30);
        });

        modelBuilder.Entity<CandidateSkill>(entity =>
        {
            entity.HasKey(e => e.CandidateSkillId).HasName("PK__Candidat__BCA602345AD3B992");

            entity.HasOne(d => d.Candidate).WithMany(p => p.CandidateSkills)
                .HasForeignKey(d => d.CandidateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Candidate__Candi__3C69FB99");

            entity.HasOne(d => d.Skill).WithMany(p => p.CandidateSkills)
                .HasForeignKey(d => d.SkillId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Candidate__Skill__3B75D760");
        });

        modelBuilder.Entity<Skill>(entity =>
        {
            entity.HasKey(e => e.SkillId).HasName("PK__Skills__AE6A6BBF7A1B1815");

            entity.Property(e => e.SkillId).HasColumnName("skillId");
            entity.Property(e => e.SkillName)
                .HasMaxLength(50)
                .HasColumnName("skillName");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
