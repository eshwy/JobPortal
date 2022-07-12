using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace JopPortal.Models
{
    public partial class JobPortal2Context : DbContext
    {
        public JobPortal2Context()
        {
        }

        public JobPortal2Context(DbContextOptions<JobPortal2Context> options)
            : base(options)
        {
        }

        public virtual DbSet<ArticlePinedTbl> ArticlePinedTbls { get; set; }
        public virtual DbSet<ArticleTbl> ArticleTbls { get; set; }
        public virtual DbSet<FollowersAndFollowingTbl> FollowersAndFollowingTbls { get; set; }
        public virtual DbSet<LoginTbl> LoginTbls { get; set; }
        public virtual DbSet<PersonalDetailsTbl> PersonalDetailsTbl { get; set; }
        public virtual DbSet<SortedProfilesTbl> SortedProfilesTbls { get; set; }
        public virtual DbSet<UserAdressTbl> UserAdressTbls { get; set; }
        public virtual DbSet<UserEducationTbl> UserEducationTbls { get; set; }
        public virtual DbSet<UserProfilePhotoTbl> UserProfilePhotoTbls { get; set; }
        public virtual DbSet<UserWorkTbl> UserWorkTbls { get; set; }
        public virtual DbSet<SignUpData> SignUpData { get; set; }
        public virtual DbSet<ArticleTitleTbl> ArticleTitleTbl { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<SignUpData>(entity =>
            {
                entity.HasNoKey();                
            });
            modelBuilder.Entity<ArticleTitleTbl>(entity =>
            {
                entity.HasKey(c => c.Id);
            });
            //modelBuilder.Entity<SignUpData>(entity =>
            //{

            //});
            modelBuilder.Entity<ArticlePinedTbl>(entity =>
            {
                entity.HasKey(e => e.RowId)
                    .HasName("PK__ArticleP__FFEE7431117885B2");

                entity.ToTable("ArticlePinedTbl");

                //entity.HasOne(d => d.User)
                //    .WithMany(p => p.ArticlePinedTbls)
                //    .HasForeignKey(d => d.UserId)
                //    .HasConstraintName("FK__ArticlePi__UserI__4222D4EF");
            });

            modelBuilder.Entity<ArticleTbl>(entity =>
            {
                entity.HasKey(e => e.RowId)
                    .HasName("PK__ArticleT__FFEE74317030CC36");

                entity.ToTable("ArticleTbl");

                entity.Property(e => e.Category)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Content)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                //entity.HasOne(d => d.User)
                //    .WithMany(p => p.ArticleTbls)
                //    .HasForeignKey(d => d.UserId)
                //    .HasConstraintName("FK__ArticleTb__UserI__3F466844");
            });

            modelBuilder.Entity<FollowersAndFollowingTbl>(entity =>
            {
                entity.HasKey(e => e.RowId)
                    .HasName("PK__Follower__FFEE7431E5FA398B");

                entity.ToTable("FollowersAndFollowingTbl");
            });

            modelBuilder.Entity<LoginTbl>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__LoginTbl__1788CC4CD92671ED");

                entity.ToTable("LoginTbl");

                entity.Property(e => e.PassWord).HasMaxLength(50);

                entity.Property(e => e.UserName).HasMaxLength(50);
            });

            modelBuilder.Entity<PersonalDetailsTbl>(entity =>
            {
                entity.HasKey(e => e.RowId)
                    .HasName("PK__Personal__FFEE74318AB5DE74");

                entity.ToTable("PersonalDetailsTbl");

                entity.HasIndex(e => e.Email, "UQ__Personal__A9D10534843D7845")
                    .IsUnique();

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.Email)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(75)
                    .IsUnicode(false);

                entity.Property(e => e.Gender)
                    .HasMaxLength(7)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(75)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Skills)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                //entity.HasOne(d => d.User)
                //    .WithMany(p => p.PersonalDetailsTbls)
                //    .HasForeignKey(d => d.UserId)
                //    .HasConstraintName("FK__PersonalD__UserI__2C3393D0");
            });

            modelBuilder.Entity<SortedProfilesTbl>(entity =>
            {
                entity.HasKey(e => e.RowId)
                    .HasName("PK__SortedPr__FFEE7431D220AC38");

                entity.ToTable("SortedProfilesTbl");
            });

            modelBuilder.Entity<UserAdressTbl>(entity =>
            {
                entity.HasKey(e => e.RowId)
                    .HasName("PK__UserAdre__FFEE74311258F164");

                entity.ToTable("UserAdressTbl");

                entity.Property(e => e.CurrentArea)
                    .HasMaxLength(75)
                    .IsUnicode(false);

                entity.Property(e => e.CurrentCity)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CurrentDoorNumber)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.CurrentStreetName)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.PermantentArea)
                    .HasMaxLength(75)
                    .IsUnicode(false);

                entity.Property(e => e.PermantentCity)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PermantentDoorNumber)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.PermantentStreetName)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                //entity.HasOne(d => d.User)
                //    .WithMany(p => p.UserAdressTbls)
                //    .HasForeignKey(d => d.UserId)
                //    .HasConstraintName("FK__UserAdres__UserI__31EC6D26");
            });

            modelBuilder.Entity<UserEducationTbl>(entity =>
            {
                entity.HasKey(e => e.RowId)
                    .HasName("PK__UserEduc__FFEE74316E5B7C6F");

                entity.ToTable("UserEducationTbl");

                entity.Property(e => e.CompletedEducationIn)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.EducationType)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.GroupName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PercentageObtained).HasColumnType("decimal(18, 0)");

                //entity.HasOne(d => d.User)
                //    .WithMany(p => p.UserEducationTbls)
                //    .HasForeignKey(d => d.UserId)
                //    .HasConstraintName("FK__UserEduca__UserI__37A5467C");
            });

            modelBuilder.Entity<UserProfilePhotoTbl>(entity =>
            {
                entity.HasKey(e => e.RowId)
                    .HasName("PK__UserProf__FFEE74318434F6BF");

                entity.ToTable("UserProfilePhotoTbl");

                entity.Property(e => e.PhotoPath)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                //entity.HasOne(d => d.User)
                //    .WithMany(p => p.UserProfilePhotoTbls)
                //    .HasForeignKey(d => d.UserId)
                //    .HasConstraintName("FK__UserProfi__UserI__2F10007B");
            });

            modelBuilder.Entity<UserWorkTbl>(entity =>
            {
                entity.HasKey(e => e.RowId)
                    .HasName("PK__UserWork__FFEE7431A173C821");

                entity.ToTable("UserWorkTbl");

                entity.Property(e => e.CompanyName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                //entity.HasOne(d => d.User)
                //    .WithMany(p => p.UserWorkTbls)
                //    .HasForeignKey(d => d.UserId)
                //    .HasConstraintName("FK__UserWorkT__UserI__34C8D9D1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
