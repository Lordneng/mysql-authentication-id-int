using System;
using System.Collections.Generic;
using System.Text;
using DevANCUser.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DevANCUser.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int, ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin, ApplicationRoleClaim, ApplicationUserToken>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>(b =>
            {
                // Primary key
                b.HasKey(u => u.Id);

                // Limit the size of columns to use efficient database types
                b.Property(u => u.UserName).HasMaxLength(128);
                b.Property(u => u.NormalizedUserName).HasMaxLength(128);
                b.Property(u => u.Email).HasMaxLength(128);
                b.Property(u => u.NormalizedEmail).HasMaxLength(128);

                // Indexes for "normalized" username and email, to allow efficient lookups
                b.HasIndex(u => u.NormalizedUserName).HasName("UserNameIndex").IsUnique();
                b.HasIndex(u => u.NormalizedEmail).HasName("EmailIndex");

                // A concurrency token for use with the optimistic concurrency checking
                b.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();                

                // Maps to the AspNetUsers table
                b.ToTable("AspNetUsers");                

                // The relationships between User and other entity types
                // Note that these relationships are configured with no navigation properties

                // Each User can have many UserClaims
                b.HasMany<ApplicationUserClaim>().WithOne().HasForeignKey(uc => uc.UserId).IsRequired();

                // Each User can have many UserLogins
                b.HasMany<ApplicationUserLogin>().WithOne().HasForeignKey(ul => ul.UserId).IsRequired();

                // Each User can have many UserTokens
                b.HasMany<ApplicationUserToken>().WithOne().HasForeignKey(ut => ut.UserId).IsRequired();

                // Each User can have many entries in the UserRole join table
                b.HasMany<ApplicationUserRole>().WithOne().HasForeignKey(ur => ur.UserId).IsRequired();
            });

            builder.Entity<ApplicationUserClaim>(b =>
            {
                // Primary key
                b.HasKey(uc => uc.Id);

                // Maps to the AspNetUserClaims table
                b.ToTable("AspNetUserClaims");
            });

            builder.Entity<ApplicationUserLogin>(b =>
            {
                // Composite primary key consisting of the LoginProvider and the key to use
                // with that provider
                b.HasKey(l => new { l.LoginProvider, l.ProviderKey });

                // Limit the size of the composite key columns due to common DB restrictions
                b.Property(l => l.LoginProvider).HasMaxLength(128);
                b.Property(l => l.ProviderKey).HasMaxLength(128);

                // Maps to the AspNetUserLogins table
                b.ToTable("AspNetUserLogins");
            });

            builder.Entity<ApplicationUserToken>(b =>
            {
                // Composite primary key consisting of the UserId, LoginProvider and Name
                b.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

                // Limit the size of the composite key columns due to common DB restrictions
                b.Property(t => t.LoginProvider).HasMaxLength(128);
                b.Property(t => t.Name).HasMaxLength(128);

                // Maps to the AspNetUserTokens table
                b.ToTable("AspNetUserTokens");
            });

            builder.Entity<ApplicationRole>(b =>
            {
                // Primary key
                b.HasKey(r => r.Id);

                // Limit the size of columns to use efficient database types]
                b.Property(u => u.Name).HasMaxLength(128);
                b.Property(u => u.NormalizedName).HasMaxLength(128);

                // Index for "normalized" role name to allow efficient lookups                
                b.HasIndex(r => r.NormalizedName).HasName("RoleNameIndex").IsUnique();                

                // Maps to the AspNetRoles table
                b.ToTable("AspNetRoles");

                // A concurrency token for use with the optimistic concurrency checking
                b.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();


                // The relationships between Role and other entity types
                // Note that these relationships are configured with no navigation properties

                // Each Role can have many entries in the UserRole join table
                b.HasMany<ApplicationUserRole>().WithOne().HasForeignKey(ur => ur.RoleId).IsRequired();

                // Each Role can have many associated RoleClaims
                b.HasMany<ApplicationRoleClaim>().WithOne().HasForeignKey(rc => rc.RoleId).IsRequired();
            });

            builder.Entity<ApplicationRoleClaim>(b =>
            {
                // Primary key
                b.HasKey(rc => rc.Id);

                b.Property(rc => rc.RoleId).HasMaxLength(128);

                // Maps to the AspNetRoleClaims table
                b.ToTable("AspNetRoleClaims");
            });

            builder.Entity<ApplicationUserRole>(b =>
            {
                // Primary key
                b.HasKey(r => new { r.UserId, r.RoleId });

                // Maps to the AspNetUserRoles table
                b.ToTable("AspNetUserRoles");
            });
        }
    }
}
