using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace nbRecruitment.ModelsERP;

public partial class GnbContext : DbContext
{
    public GnbContext()
    {
    }

    public GnbContext(DbContextOptions<GnbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Candidate> Candidates { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<Profile> Profiles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("host=97.74.83.143;database=GNB;username=gnb;password=gnb2023", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.34-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Candidate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("candidates");

            entity.HasIndex(e => e.PositionId, "fk_candidates_position");

            entity.HasIndex(e => e.ProfileId, "fk_candidates_profile");

            entity.HasIndex(e => e.UserId, "fk_candidates_user");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Admin).HasColumnName("admin");
            entity.Property(e => e.Airport).HasColumnName("airport");
            entity.Property(e => e.ArrivalDate).HasColumnName("arrival_date");
            entity.Property(e => e.BCertificate).HasColumnName("b_certificate");
            entity.Property(e => e.Bank).HasColumnName("bank");
            entity.Property(e => e.Bio).HasColumnName("bio");
            entity.Property(e => e.Canadian).HasColumnName("canadian");
            entity.Property(e => e.Caq).HasColumnName("caq");
            entity.Property(e => e.Country).HasColumnName("country");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Deleted)
                .HasColumnType("datetime(3)")
                .HasColumnName("deleted");
            entity.Property(e => e.Diplomas).HasColumnName("diplomas");
            entity.Property(e => e.Dob).HasColumnName("dob");
            entity.Property(e => e.Driver).HasColumnName("driver");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.Employer).HasColumnName("employer");
            entity.Property(e => e.Employment).HasColumnName("employment");
            entity.Property(e => e.Expiration).HasColumnName("expiration");
            entity.Property(e => e.Foreign).HasColumnName("foreign");
            entity.Property(e => e.Funds).HasColumnName("funds");
            entity.Property(e => e.Handbook).HasColumnName("handbook");
            entity.Property(e => e.Healthcard).HasColumnName("healthcard");
            entity.Property(e => e.Housing).HasColumnName("housing");
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.JobOffer).HasColumnName("job_offer");
            entity.Property(e => e.LastUpdate).HasColumnName("last_update");
            entity.Property(e => e.License).HasColumnName("license");
            entity.Property(e => e.Marriage).HasColumnName("marriage");
            entity.Property(e => e.Med).HasColumnName("med");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Nationality).HasColumnName("nationality");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.Number).HasColumnName("number");
            entity.Property(e => e.PCertificate).HasColumnName("p_certificate");
            entity.Property(e => e.Passport).HasColumnName("passport");
            entity.Property(e => e.Paystub).HasColumnName("paystub");
            entity.Property(e => e.Permit).HasColumnName("permit");
            entity.Property(e => e.PositionId).HasColumnName("position_id");
            entity.Property(e => e.PreArrival).HasColumnName("pre_arrival");
            entity.Property(e => e.ProfileId).HasColumnName("profile_id");
            entity.Property(e => e.Remarks).HasColumnName("remarks");
            entity.Property(e => e.Resident).HasColumnName("resident");
            entity.Property(e => e.Resume).HasColumnName("resume");
            entity.Property(e => e.Retainer).HasColumnName("retainer");
            entity.Property(e => e.Settle).HasColumnName("settle");
            entity.Property(e => e.SettlementBy).HasColumnName("settlement_by");
            entity.Property(e => e.SettlementDate).HasColumnName("settlement_date");
            entity.Property(e => e.Sex).HasColumnName("sex");
            entity.Property(e => e.Sin).HasColumnName("sin");
            entity.Property(e => e.Starting).HasColumnName("starting");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.TeamsLink).HasColumnName("teams_link");
            entity.Property(e => e.Ticket).HasColumnName("ticket");
            entity.Property(e => e.Transcript).HasColumnName("transcript");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Visa).HasColumnName("visa");
            entity.Property(e => e.VisaStat).HasColumnName("visa_stat");
            entity.Property(e => e.WCertificate).HasColumnName("w_certificate");

            entity.HasOne(d => d.Position).WithMany(p => p.Candidates)
                .HasForeignKey(d => d.PositionId)
                .HasConstraintName("fk_candidates_position");

            entity.HasOne(d => d.Profile).WithMany(p => p.Candidates)
                .HasForeignKey(d => d.ProfileId)
                .HasConstraintName("fk_candidates_profile");

            entity.HasOne(d => d.User).WithMany(p => p.Candidates)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_candidates_user");
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("positions");

            entity.HasIndex(e => e.ProfileId, "fk_positions_profile");

            entity.HasIndex(e => e.UserId, "fk_positions_user");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Account).HasColumnName("account");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.Charge).HasColumnName("charge");
            entity.Property(e => e.Count).HasColumnName("count");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Deleted)
                .HasColumnType("datetime(3)")
                .HasColumnName("deleted");
            entity.Property(e => e.Demand).HasColumnName("demand");
            entity.Property(e => e.Flight).HasColumnName("flight");
            entity.Property(e => e.Invoice).HasColumnName("invoice");
            entity.Property(e => e.InvoiceAmount).HasColumnName("invoice_amount");
            entity.Property(e => e.InvoiceAmount1).HasColumnName("invoice_amount1");
            entity.Property(e => e.InvoiceAmount2).HasColumnName("invoice_amount2");
            entity.Property(e => e.InvoiceAmount3).HasColumnName("invoice_amount3");
            entity.Property(e => e.InvoiceDate).HasColumnName("invoice_date");
            entity.Property(e => e.InvoiceDate1).HasColumnName("invoice_date1");
            entity.Property(e => e.InvoiceDate2).HasColumnName("invoice_date2");
            entity.Property(e => e.InvoiceDate3).HasColumnName("invoice_date3");
            entity.Property(e => e.InvoiceNotes).HasColumnName("invoice_notes");
            entity.Property(e => e.InvoiceNotes1).HasColumnName("invoice_notes1");
            entity.Property(e => e.InvoiceNotes2).HasColumnName("invoice_notes2");
            entity.Property(e => e.InvoiceNotes3).HasColumnName("invoice_notes3");
            entity.Property(e => e.InvoiceNumber).HasColumnName("invoice_number");
            entity.Property(e => e.InvoiceNumber1).HasColumnName("invoice_number1");
            entity.Property(e => e.InvoiceNumber2).HasColumnName("invoice_number2");
            entity.Property(e => e.InvoiceNumber3).HasColumnName("invoice_number3");
            entity.Property(e => e.JobBank).HasColumnName("job_bank");
            entity.Property(e => e.LastUpdate).HasColumnName("last_update");
            entity.Property(e => e.Lmia).HasColumnName("lmia");
            entity.Property(e => e.Message).HasColumnName("message");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.Notes1).HasColumnName("notes1");
            entity.Property(e => e.Position1).HasColumnName("position");
            entity.Property(e => e.ProfileId).HasColumnName("profile_id");
            entity.Property(e => e.Province).HasColumnName("province");
            entity.Property(e => e.Rate).HasColumnName("rate");
            entity.Property(e => e.Recruitment).HasColumnName("recruitment");
            entity.Property(e => e.Remarks).HasColumnName("remarks");
            entity.Property(e => e.Team).HasColumnName("team");
            entity.Property(e => e.Total).HasColumnName("total");
            entity.Property(e => e.TotalCost).HasColumnName("total_cost");
            entity.Property(e => e.TotalProfit).HasColumnName("total_profit");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Profile).WithMany(p => p.Positions)
                .HasForeignKey(d => d.ProfileId)
                .HasConstraintName("fk_positions_profile");

            entity.HasOne(d => d.User).WithMany(p => p.Positions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_positions_user");
        });

        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("profiles");

            entity.HasIndex(e => e.UserId, "fk_profiles_user");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Deleted)
                .HasColumnType("datetime(3)")
                .HasColumnName("deleted");
            entity.Property(e => e.File1).HasColumnName("file1");
            entity.Property(e => e.File2).HasColumnName("file2");
            entity.Property(e => e.File3).HasColumnName("file3");
            entity.Property(e => e.File4).HasColumnName("file4");
            entity.Property(e => e.File5).HasColumnName("file5");
            entity.Property(e => e.File6).HasColumnName("file6");
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.LastUpdate).HasColumnName("last_update");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Number).HasColumnName("number");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Profiles)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_profiles_user");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("users");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Deleted)
                .HasColumnType("datetime(3)")
                .HasColumnName("deleted");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.Position).HasColumnName("position");
            entity.Property(e => e.Team).HasColumnName("team");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.Username).HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
