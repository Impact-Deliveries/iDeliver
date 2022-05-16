﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using iDeliverDataAccess;
using IDeliverObjects.Objects;

namespace iDeliverDataAccess.Base
{
    public partial class IDeliverDbContext : DbContext
    {
        public IDeliverDbContext()
        {
        }

        public IDeliverDbContext(DbContextOptions<IDeliverDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Attachment> Attachments { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Driver> Drivers { get; set; }
        public virtual DbSet<DriverDetail> DriverDetails { get; set; }
        public virtual DbSet<DriverSchadule> DriverSchadules { get; set; }
        public virtual DbSet<Enrolment> Enrolments { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Merchant> Merchants { get; set; }
        public virtual DbSet<MerchantBranch> MerchantBranches { get; set; }
        public virtual DbSet<MerchantDeliveryPrice> MerchantDeliveryPrices { get; set; }
        public virtual DbSet<MerchantEmployee> MerchantEmployees { get; set; }
        public virtual DbSet<Organization> Organizations { get; set; }
        public virtual DbSet<OrganizationEmployee> OrganizationEmployees { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Scaffolding:ConnectionString", "Data Source=(local);Initial Catalog=iDeliverDB;Integrated Security=true");

            modelBuilder.Entity<Attachment>(entity =>
            {
                entity.ToTable("Attachment");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.CreatorId).HasColumnName("CreatorID");

                entity.Property(e => e.Extension)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.GroupId)
                    .HasMaxLength(10)
                    .HasColumnName("GroupID")
                    .IsFixedLength();

                entity.Property(e => e.ModuleId).HasColumnName("ModuleID");

                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable("Country");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CountryCode).HasMaxLength(10);

                entity.Property(e => e.CountryFlag).HasMaxLength(50);

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getutcdate()");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getutcdate()");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Driver>(entity =>
            {
                entity.ToTable("Driver");

                entity.HasIndex(e => e.NationalNumber, "UK_DriverNationalNumber")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Birthday).HasColumnType("datetime");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getutcdate()");

                entity.Property(e => e.EnrolmentId).HasColumnName("EnrolmentID");

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("1");

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.Mobile)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getutcdate()");

                entity.Property(e => e.NationalNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.OrganizationId)
                    .HasColumnName("OrganizationID")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.SecondName).HasMaxLength(50);

                entity.HasOne(d => d.Enrolment)
                    .WithMany(p => p.Drivers)
                    .HasForeignKey(d => d.EnrolmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Driver_Enrolment");

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.Drivers)
                    .HasForeignKey(d => d.OrganizationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Driver_Organization");
            });

            modelBuilder.Entity<DriverDetail>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AvancedStudies).HasMaxLength(50);

                entity.Property(e => e.College).HasMaxLength(50);

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getutcdate()");

                entity.Property(e => e.DriverId).HasColumnName("DriverID");

                entity.Property(e => e.Estimate).HasMaxLength(50);

                entity.Property(e => e.FromTime).HasColumnType("datetime");

                entity.Property(e => e.GraduationYear).HasMaxLength(50);

                entity.Property(e => e.Major).HasMaxLength(50);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getutcdate()");

                entity.Property(e => e.StartJob).HasColumnType("datetime");

                entity.Property(e => e.ToTime).HasColumnType("datetime");

                entity.Property(e => e.University).HasMaxLength(50);

                entity.HasOne(d => d.Driver)
                    .WithMany(p => p.DriverDetails)
                    .HasForeignKey(d => d.DriverId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DriverDetails_Driver");
            });

            modelBuilder.Entity<DriverSchadule>(entity =>
            {
                entity.ToTable("DriverSchadule");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.DayId).HasColumnName("DayID");

                entity.Property(e => e.DriverId).HasColumnName("DriverID");

                entity.HasOne(d => d.Driver)
                    .WithMany(p => p.DriverSchadules)
                    .HasForeignKey(d => d.DriverId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DriverSchadule_Driver");
            });

            modelBuilder.Entity<Enrolment>(entity =>
            {
                entity.ToTable("Enrolment");

                entity.HasIndex(e => new { e.UserId, e.RoleId }, "UK_tblEnrolments")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Enrolments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Enrolment_User");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable("Location");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getutcdate()");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getutcdate()");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Locations)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Location_Country");
            });

            modelBuilder.Entity<Merchant>(entity =>
            {
                entity.ToTable("Merchant");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getutcdate()");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("1");

                entity.Property(e => e.MerchantName).HasMaxLength(50);

                entity.Property(e => e.Mobile).HasMaxLength(50);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getutcdate()");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.Overview)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.Owner).HasMaxLength(100);

                entity.Property(e => e.OwnerNumber).HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.Position).HasMaxLength(100);

                entity.Property(e => e.QutationNumber).HasMaxLength(100);

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.Merchants)
                    .HasForeignKey(d => d.OrganizationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Merchant_Organization");
            });

            modelBuilder.Entity<MerchantBranch>(entity =>
            {
                entity.ToTable("MerchantBranch");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.BranchName).HasMaxLength(50);

                entity.Property(e => e.BranchPicture).HasMaxLength(50);

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getutcdate()");

                entity.Property(e => e.DeliveryPriceOffer).HasColumnType("money");

                entity.Property(e => e.DeliveryStatus).HasDefaultValueSql("1");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("1");

                entity.Property(e => e.Latitude).HasMaxLength(50);

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.Longitude).HasMaxLength(50);

                entity.Property(e => e.MerchantId).HasColumnName("MerchantID");

                entity.Property(e => e.Mobile).HasMaxLength(50);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getutcdate()");

                entity.Property(e => e.Overview).HasColumnType("text");

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.MerchantBranches)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MerchantBranch_Location");

                entity.HasOne(d => d.Merchant)
                    .WithMany(p => p.MerchantBranches)
                    .HasForeignKey(d => d.MerchantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MerchantBranch_Merchant");
            });

            modelBuilder.Entity<MerchantDeliveryPrice>(entity =>
            {
                entity.ToTable("MerchantDeliveryPrice");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Amount).HasColumnType("money");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getutcdate()");

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.MerchantBranchId).HasColumnName("MerchantBranchID");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getutcdate()");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.MerchantDeliveryPrices)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK_MerchantDeliveryPrice_Location");

                entity.HasOne(d => d.MerchantBranch)
                    .WithMany(p => p.MerchantDeliveryPrices)
                    .HasForeignKey(d => d.MerchantBranchId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MerchantDeliveryPrice_MerchantBranch");
            });

            modelBuilder.Entity<MerchantEmployee>(entity =>
            {
                entity.ToTable("MerchantEmployee");

                entity.HasIndex(e => e.NationalNumber, "UK_MerchantEmployeeNationalNumber")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getutcdate()");

                entity.Property(e => e.EnrolmentId).HasColumnName("EnrolmentID");

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("1");

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.MerchantBranchId).HasColumnName("MerchantBranchID");

                entity.Property(e => e.MiddleName).HasMaxLength(50);

                entity.Property(e => e.Mobile).HasMaxLength(50);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getutcdate()");

                entity.Property(e => e.NationalNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.ProfilePicture).HasMaxLength(50);

                entity.HasOne(d => d.Enrolment)
                    .WithMany(p => p.MerchantEmployees)
                    .HasForeignKey(d => d.EnrolmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MerchantEmployee_Enrolment");

                entity.HasOne(d => d.MerchantBranch)
                    .WithMany(p => p.MerchantEmployees)
                    .HasForeignKey(d => d.MerchantBranchId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MerchantEmployee_MerchantBranch");
            });

            modelBuilder.Entity<Organization>(entity =>
            {
                entity.ToTable("Organization");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getutcdate()");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("1");

                entity.Property(e => e.Mobile).HasMaxLength(50);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getutcdate()");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.Timezone)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Organizations)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Organization_Country");
            });

            modelBuilder.Entity<OrganizationEmployee>(entity =>
            {
                entity.ToTable("OrganizationEmployee");

                entity.HasIndex(e => e.NationalNumber, "UK_OrganizationEmployeeNationalNumber")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getutcdate()");

                entity.Property(e => e.EnrolmentId).HasColumnName("EnrolmentID");

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("1");

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.MiddleName).HasMaxLength(50);

                entity.Property(e => e.Mobile).HasMaxLength(50);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getutcdate()");

                entity.Property(e => e.NationalNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.ProfilePicture).HasMaxLength(50);

                entity.HasOne(d => d.Enrolment)
                    .WithMany(p => p.OrganizationEmployees)
                    .HasForeignKey(d => d.EnrolmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrganizationEmployee_Enrolment");

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.OrganizationEmployees)
                    .HasForeignKey(d => d.OrganizationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrganizationEmployee_Organization");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.HasIndex(e => e.ReferenceNumber, "UK_User")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getutcdate()");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("1");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getutcdate()");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ReferenceNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}