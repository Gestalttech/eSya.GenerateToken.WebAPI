using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace eSya.GenerateToken.DL.Entities
{
    public partial class eSyaEnterprise : DbContext
    {
        public static string _connString = "";

        public eSyaEnterprise()
        {
        }

        public eSyaEnterprise(DbContextOptions<eSyaEnterprise> options)
            : base(options)
        {
        }

        public virtual DbSet<GtEcapcd> GtEcapcds { get; set; } = null!;
        public virtual DbSet<GtEcbsln> GtEcbslns { get; set; } = null!;
        public virtual DbSet<GtEcmotp> GtEcmotps { get; set; } = null!;
        public virtual DbSet<GtQsdssy> GtQsdssies { get; set; } = null!;
        public virtual DbSet<GtTokm01> GtTokm01s { get; set; } = null!;
        public virtual DbSet<GtTokm02> GtTokm02s { get; set; } = null!;
        public virtual DbSet<GtTokm03> GtTokm03s { get; set; } = null!;
        public virtual DbSet<GtTokm04> GtTokm04s { get; set; } = null!;
        public virtual DbSet<GtTokm05> GtTokm05s { get; set; } = null!;
        public virtual DbSet<GtTokm06> GtTokm06s { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer(_connString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GtEcapcd>(entity =>
            {
                entity.HasKey(e => e.ApplicationCode)
                    .HasName("PK_GT_ECAPCD_1");

                entity.ToTable("GT_ECAPCD");

                entity.Property(e => e.ApplicationCode).ValueGeneratedNever();

                entity.Property(e => e.CodeDesc).HasMaxLength(50);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.FormId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FormID");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);

                entity.Property(e => e.ShortCode).HasMaxLength(15);
            });

            modelBuilder.Entity<GtEcbsln>(entity =>
            {
                entity.HasKey(e => new { e.BusinessId, e.LocationId });

                entity.ToTable("GT_ECBSLN");

                entity.HasIndex(e => e.BusinessKey, "IX_GT_ECBSLN")
                    .IsUnique();

                entity.Property(e => e.BusinessId).HasColumnName("BusinessID");

                entity.Property(e => e.BusinessName).HasMaxLength(100);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.CurrencyCode).HasMaxLength(4);

                entity.Property(e => e.FormId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FormID");

                entity.Property(e => e.Isdcode).HasColumnName("ISDCode");

                entity.Property(e => e.LocationDescription).HasMaxLength(150);

                entity.Property(e => e.Lstatus).HasColumnName("LStatus");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);

                entity.Property(e => e.ShortDesc).HasMaxLength(15);

                entity.Property(e => e.TocurrConversion).HasColumnName("TOCurrConversion");

                entity.Property(e => e.TolocalCurrency)
                    .IsRequired()
                    .HasColumnName("TOLocalCurrency")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.TorealCurrency).HasColumnName("TORealCurrency");
            });

            modelBuilder.Entity<GtEcmotp>(entity =>
            {
                entity.ToTable("GT_ECMOTP");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.ConfirmedOn).HasColumnType("datetime");

                entity.Property(e => e.GeneratedOn).HasColumnType("datetime");

                entity.Property(e => e.MobileNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Otp)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("OTP");

                entity.Property(e => e.Otptype)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("OTPType")
                    .IsFixedLength();
            });

            modelBuilder.Entity<GtQsdssy>(entity =>
            {
                entity.HasKey(e => new { e.BusinessKey, e.DisplayId });

                entity.ToTable("GT_QSDSSY");

                entity.Property(e => e.DisplayId).HasColumnName("DisplayID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.DisplayIpaddress)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("DisplayIPAddress");

                entity.Property(e => e.DisplayScreenType)
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.DisplayUrl)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("DisplayURL");

                entity.Property(e => e.FormId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FormID");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);

                entity.Property(e => e.QueryString)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<GtTokm01>(entity =>
            {
                entity.HasKey(e => new { e.TokenType, e.TokenPrefix });

                entity.ToTable("GT_TOKM01");

                entity.Property(e => e.TokenType)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.TokenPrefix).HasMaxLength(4);

                entity.Property(e => e.ConfirmationUrl)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("ConfirmationURL");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.DisplaySequence).HasDefaultValueSql("((1))");

                entity.Property(e => e.FormId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FormID");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);

                entity.Property(e => e.QrcodeUrl)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("QRCodeURL");

                entity.Property(e => e.TokenDesc).HasMaxLength(50);
            });

            modelBuilder.Entity<GtTokm02>(entity =>
            {
                entity.HasKey(e => new { e.BusinessKey, e.CounterNumber, e.FloorId })
                    .HasName("PK_GT_TOKM03_1");

                entity.ToTable("GT_TOKM02");

                entity.Property(e => e.CounterNumber).HasMaxLength(10);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.FormId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FormID");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);
            });

            modelBuilder.Entity<GtTokm03>(entity =>
            {
                entity.HasKey(e => new { e.BusinessKey, e.TokenPrefix, e.FloorId, e.CounterNumber })
                    .HasName("PK_GT_TOKM02");

                entity.ToTable("GT_TOKM03");

                entity.Property(e => e.TokenPrefix).HasMaxLength(4);

                entity.Property(e => e.CounterNumber).HasMaxLength(10);

                entity.Property(e => e.CounterKey).HasMaxLength(20);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.FormId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FormID");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);
            });

            modelBuilder.Entity<GtTokm04>(entity =>
            {
                entity.HasKey(e => new { e.BusinessKey, e.CounterKey, e.AddOn })
                    .HasName("PK_GT_TOKM04_1");

                entity.ToTable("GT_TOKM04");

                entity.Property(e => e.CounterKey).HasMaxLength(20);

                entity.Property(e => e.AddOn).HasMaxLength(4);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.FormId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FormID");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);
            });

            modelBuilder.Entity<GtTokm05>(entity =>
            {
                entity.HasKey(e => new { e.BusinessKey, e.TokenDate, e.TokenKey });

                entity.ToTable("GT_TOKM05");

                entity.Property(e => e.TokenDate).HasColumnType("date");

                entity.Property(e => e.TokenKey).HasMaxLength(20);

                entity.Property(e => e.CallingCounter).HasMaxLength(10);

                entity.Property(e => e.CompletedTime).HasColumnType("datetime");

                entity.Property(e => e.ConfirmationTime).HasColumnType("datetime");

                entity.Property(e => e.CounterKey).HasMaxLength(20);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.FormId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FormID");

                entity.Property(e => e.Isdcode).HasColumnName("ISDCode");

                entity.Property(e => e.MobileNumber).HasMaxLength(15);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);

                entity.Property(e => e.TokenCallingTime).HasColumnType("datetime");

                entity.Property(e => e.TokenPrefix).HasMaxLength(4);

                entity.Property(e => e.TokenStatus)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<GtTokm06>(entity =>
            {
                entity.HasKey(e => new { e.BusinessKey, e.TokenDate, e.TokenKey, e.HoldOccurence });

                entity.ToTable("GT_TOKM06");

                entity.Property(e => e.TokenDate).HasColumnType("date");

                entity.Property(e => e.TokenKey).HasMaxLength(20);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.FormId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FormID");

                entity.Property(e => e.HoldTime).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);

                entity.Property(e => e.ReCallTime).HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
