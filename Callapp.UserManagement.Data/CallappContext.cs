using System;
using System.Collections.Generic;
using Callapp.UserManagement.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Callapp.UserManagement.Data;

public class CallappContext : DbContext
{
    public CallappContext()
    {
    }

    public CallappContext(DbContextOptions<CallappContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserProfile> UserProfiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(200);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.ToTable("UserProfile");

            entity.Property(e => e.Firstname).HasMaxLength(50);
            entity.Property(e => e.Lastname).HasMaxLength(50);
            entity.Property(e => e.PersonalNumber)
                .HasMaxLength(11)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.User).WithOne(p => p.UserProfile)
                .HasForeignKey<UserProfile>(d => d.UserId)
                .HasConstraintName("FK_UserProfile_User");
        });

    }

}
