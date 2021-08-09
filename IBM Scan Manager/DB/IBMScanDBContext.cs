using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace IBM_Scan_Manager.Models
{
    public partial class IBMScanDBContext : DbContext
    {
        public IBMScanDBContext()
        {
        }

        public IBMScanDBContext(DbContextOptions<IBMScanDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblAssessment> TblAssessments { get; set; }
        public virtual DbSet<TblExcel> TblExcels { get; set; }
        public virtual DbSet<TblProject> TblProjects { get; set; }
        public virtual DbSet<TblScan> TblScans { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=IBMScanDB;Integrated Security=True;Pooling=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<TblAssessment>(entity =>
            {
                entity.Property(e => e.Api).IsUnicode(false);

                entity.Property(e => e.Classification).IsUnicode(false);

                entity.Property(e => e.Comment).IsUnicode(false);

                entity.Property(e => e.Context).IsUnicode(false);

                entity.Property(e => e.SourceFile).IsUnicode(false);

                entity.Property(e => e.Vulnerability).IsUnicode(false);

                entity.HasOne(d => d.Scan)
                    .WithMany(p => p.TblAssessments)
                    .HasForeignKey(d => d.ScanId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAssessment_tblScan");
            });

            modelBuilder.Entity<TblExcel>(entity =>
            {
                entity.HasOne(d => d.Scan)
                    .WithMany(p => p.TblExcels)
                    .HasForeignKey(d => d.ScanId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblExcel_tblScan");
            });

            modelBuilder.Entity<TblProject>(entity =>
            {
                entity.Property(e => e.ModuleName).IsUnicode(false);

                entity.Property(e => e.ProjName).IsUnicode(false);
            });

            modelBuilder.Entity<TblScan>(entity =>
            {
                entity.Property(e => e.ScanType).IsUnicode(false);

                entity.HasOne(d => d.Proj)
                    .WithMany(p => p.TblScans)
                    .HasForeignKey(d => d.ProjId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblScan_tblProject");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
