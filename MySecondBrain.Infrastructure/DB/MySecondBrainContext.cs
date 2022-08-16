using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace MySecondBrain.Infrastructure.DB
{
    public partial class MySecondBrainContext : DbContext
    {
        public MySecondBrainContext()
        {
        }

        public MySecondBrainContext(DbContextOptions<MySecondBrainContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }
        public virtual DbSet<Dossier> Dossiers { get; set; }
        public virtual DbSet<Note> Notes { get; set; }
        public virtual DbSet<NoteTag> NoteTags { get; set; }
        public virtual DbSet<RechercheUtilisateur> RechercheUtilisateurs { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=LAPTOP-GN9BCEBB\\SQLEXPRESS;Database=MySecondBrain;Trusted_Connection=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "French_CI_AS");

            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetRoleClaim>(entity =>
            {
                entity.Property(e => e.RoleId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<Dossier>(entity =>
            {
                entity.HasKey(e => e.Iddossier);

                entity.ToTable("Dossier");

                entity.Property(e => e.Iddossier).HasColumnName("IDDossier");

                entity.Property(e => e.IddossierParent).HasColumnName("IDDossierParent");

                entity.Property(e => e.Nom).HasMaxLength(50);

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.IddossierParentNavigation)
                    .WithMany(p => p.InverseIddossierParentNavigation)
                    .HasForeignKey(d => d.IddossierParent)
                    .HasConstraintName("FK_Dossier_Dossier");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Dossiers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Dossier_AspNetUsers");
            });

            modelBuilder.Entity<Note>(entity =>
            {
                entity.HasKey(e => e.Idnote);

                entity.ToTable("Note");

                entity.Property(e => e.Idnote).HasColumnName("IDNote");

                entity.Property(e => e.Author)
                    .HasMaxLength(300)
                    .IsFixedLength(true);

                entity.Property(e => e.DateCreation).HasColumnType("datetime");

                entity.Property(e => e.Iddossier).HasColumnName("IDDossier");

                entity.Property(e => e.Title).HasMaxLength(500);

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.IddossierNavigation)
                    .WithMany(p => p.Notes)
                    .HasForeignKey(d => d.Iddossier)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Note_Dossier");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Notes)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Note_AspNetUsers");
            });

            modelBuilder.Entity<NoteTag>(entity =>
            {
                entity.HasKey(e => new { e.Idnote, e.Idtag });

                entity.ToTable("NoteTag");

                entity.Property(e => e.Idnote).HasColumnName("IDNote");

                entity.Property(e => e.Idtag).HasColumnName("IDTag");

                entity.HasOne(d => d.IdnoteNavigation)
                    .WithMany(p => p.NoteTags)
                    .HasForeignKey(d => d.Idnote)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NoteTag_Note");

                entity.HasOne(d => d.IdtagNavigation)
                    .WithMany(p => p.NoteTags)
                    .HasForeignKey(d => d.Idtag)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NoteTag_NoteTag");
            });

            modelBuilder.Entity<RechercheUtilisateur>(entity =>
            {
                entity.HasKey(e => e.IdrechercheUtilisateur);

                entity.ToTable("RechercheUtilisateur");

                entity.Property(e => e.IdrechercheUtilisateur).HasColumnName("IDRechercheUtilisateur");

                entity.Property(e => e.MotsCles).HasMaxLength(200);

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.RechercheUtilisateurs)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_RechercheUtilisateur_AspNetUsers");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.HasKey(e => e.Idtag);

                entity.ToTable("Tag");

                entity.Property(e => e.Idtag).HasColumnName("IDTag");

                entity.Property(e => e.Nom).HasMaxLength(50);

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Tags)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Tag_AspNetUsers");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
