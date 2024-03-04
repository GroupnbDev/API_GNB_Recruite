using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace nbRecruitment.Models;

public partial class NbRecruitmentContext : DbContext
{
    public NbRecruitmentContext()
    {
    }

    public NbRecruitmentContext(DbContextOptions<NbRecruitmentContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admissibility> Admissibilities { get; set; }

    public virtual DbSet<AsignUser> AsignUsers { get; set; }

    public virtual DbSet<Candidate> Candidates { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Jobtype> Jobtypes { get; set; }

    public virtual DbSet<Language> Languages { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<Posting> Postings { get; set; }

    public virtual DbSet<Sbu> Sbus { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Userlanguage> Userlanguages { get; set; }

    public virtual DbSet<VLanguageRecruiter> VLanguageRecruiters { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("host=97.74.83.143;database=nb_recruitment;username=gnb;password=gnb2023", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.34-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Admissibility>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("admissibility");

            entity.Property(e => e._1a).HasColumnName("1a");
            entity.Property(e => e._1b).HasColumnName("1b");
            entity.Property(e => e._1c).HasColumnName("1c");
            entity.Property(e => e._2a).HasColumnName("2a");
            entity.Property(e => e._2b).HasColumnName("2b");
            entity.Property(e => e._2c).HasColumnName("2c");
            entity.Property(e => e._2d).HasColumnName("2d");
            entity.Property(e => e._3a).HasColumnName("3a");
            entity.Property(e => e._3b).HasColumnName("3b");
            entity.Property(e => e._4a).HasColumnName("4a");
            entity.Property(e => e._4b).HasColumnName("4b");
            entity.Property(e => e._5a).HasColumnName("5a");
            entity.Property(e => e._6a).HasColumnName("6a");
            entity.Property(e => e._6b).HasColumnName("6b");
        });

        modelBuilder.Entity<AsignUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasIndex(e => e.PostingId, "PostingId_idx");

            entity.HasIndex(e => e.UserId, "userId_idx");

            entity.Property(e => e.LanguageCode).HasMaxLength(45);
            entity.Property(e => e.Status).HasDefaultValueSql("'1'");
        });

        modelBuilder.Entity<Candidate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("candidates");

            entity.Property(e => e.Country).HasMaxLength(45);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.Currency).HasMaxLength(500);
            entity.Property(e => e.CurrentCountry).HasMaxLength(45);
            entity.Property(e => e.Email).HasMaxLength(500);
            entity.Property(e => e.Firstname).HasMaxLength(45);
            entity.Property(e => e.IsDelete).HasDefaultValueSql("'0'");
            entity.Property(e => e.IsViewed).HasDefaultValueSql("'0'");
            entity.Property(e => e.JobCode).HasMaxLength(45);
            entity.Property(e => e.LastStatusDescription).HasMaxLength(45);
            entity.Property(e => e.Lastname).HasMaxLength(45);
            entity.Property(e => e.Middlename).HasMaxLength(45);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasDefaultValueSql("'1'");
            entity.Property(e => e.StatusDescription).HasMaxLength(45);
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("country");

            entity.Property(e => e.Code).HasMaxLength(45);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.IsDelete).HasDefaultValueSql("'0'");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Status).HasDefaultValueSql("'1'");
        });

        modelBuilder.Entity<Jobtype>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("jobtype");

            entity.HasIndex(e => e.Code, "Code_UNIQUE").IsUnique();

            entity.Property(e => e.Code).HasMaxLength(45);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.IsDelete).HasDefaultValueSql("'0'");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(45);
            entity.Property(e => e.Status).HasDefaultValueSql("'1'");
        });

        modelBuilder.Entity<Language>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("languages");

            entity.HasIndex(e => e.Code, "Code_UNIQUE").IsUnique();

            entity.Property(e => e.Code).HasMaxLength(45);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.IsDelete).HasDefaultValueSql("'0'");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(45);
            entity.Property(e => e.Status).HasDefaultValueSql("'1'");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("menu");

            entity.HasIndex(e => e.ParentId, "ParentId_UNIQUE").IsUnique();

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.IsDelete).HasDefaultValueSql("'0'");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Status).HasDefaultValueSql("'1'");
        });

        modelBuilder.Entity<Posting>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("posting");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.Currency).HasMaxLength(45);
            entity.Property(e => e.IsDelete).HasDefaultValueSql("'0'");
            entity.Property(e => e.JobCode).HasMaxLength(45);
            entity.Property(e => e.JobType).HasMaxLength(45);
            entity.Property(e => e.LanguageCodes).HasMaxLength(200);
            entity.Property(e => e.Location).HasMaxLength(500);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Per).HasMaxLength(45);
            entity.Property(e => e.Salary).HasMaxLength(150);
            entity.Property(e => e.Status).HasDefaultValueSql("'1'");
            entity.Property(e => e.Type).HasMaxLength(250);
        });

        modelBuilder.Entity<Sbu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("sbu");

            entity.HasIndex(e => e.Code, "SBUCode_UNIQUE").IsUnique();

            entity.Property(e => e.Code).HasMaxLength(45);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.IsDelete).HasDefaultValueSql("'0'");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Status).HasDefaultValueSql("'1'");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("users");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.Firstname).HasMaxLength(100);
            entity.Property(e => e.IsDelete).HasDefaultValueSql("'0'");
            entity.Property(e => e.Lastname).HasMaxLength(100);
            entity.Property(e => e.Middlename).HasMaxLength(100);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Sbucode)
                .HasMaxLength(45)
                .HasColumnName("SBUCode");
            entity.Property(e => e.Status).HasDefaultValueSql("'1'");
            entity.Property(e => e.Type).HasMaxLength(10);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder.Entity<Userlanguage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("userlanguage");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.LangCode).HasMaxLength(45);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VLanguageRecruiter>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_LanguageRecruiter");

            entity.Property(e => e.Firstname).HasMaxLength(100);
            entity.Property(e => e.Fullname).HasMaxLength(203);
            entity.Property(e => e.LangCode).HasMaxLength(45);
            entity.Property(e => e.Lastname).HasMaxLength(100);
            entity.Property(e => e.Middlename).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(45);
            entity.Property(e => e.Status).HasDefaultValueSql("'1'");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
