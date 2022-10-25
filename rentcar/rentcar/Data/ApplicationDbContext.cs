using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using rentcar.Models.Db;

namespace rentcar.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<AdminDbModel> mAdmin { get; set; } = default!;
    public DbSet<CustomerDbModel> mCustomer { get; set; } = default!;
    public DbSet<VendorDbModel> mVendor { get; set; } = default!;
    public DbSet<JenisBahanBakarDbModel> mJenisBahanBakar { get; set; } = default!;
    public DbSet<JenisBiayaDbModel> mJenisBiaya { get; set; } = default!;
    public DbSet<KendaraanDbModel> mKendaraan { get; set; } = default!;
    public DbSet<KonfigurasiDbModel> mKonfigurasi { get; set; } = default!;
    public DbSet<MerekKendaraanDbModel> mMerekKendaraan { get; set; } = default!;
    public DbSet<TipeKendaraanDbModel> mTipeKendaraan { get; set; } = default!;
    public DbSet<KondisiKendaraanDbModel> trKondisiKendaraan { get; set; } = default!;
    public DbSet<KondisiKendaraanFotoDbModel> trKondisiKendaraanFoto { get; set; } = default!;
    public DbSet<SewaBiayaDbModel> trSewaBiaya { get; set; } = default!;
    public DbSet<SewaDbModel> trSewa { get; set; } = default!;
    public DbSet<SewaPerjanjianDbModel> trSewaPerjanjian { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // roles
        var roleAdmin = new IdentityRole("Admin") { NormalizedName = "Admin" };
        builder.Entity<IdentityRole>().HasData(
            roleAdmin,
            new IdentityRole("Vendor") { NormalizedName = "Vendor" }
        );

        // user
        var hasher = new PasswordHasher<IdentityUser>();
        var user = new IdentityUser("admin")
        {
            NormalizedUserName = "admin",
            Email = "admin@rentcar.com",
            NormalizedEmail = "admin@rentcar.com",
            EmailConfirmed = true,
            SecurityStamp = string.Empty
        };
        user.PasswordHash = hasher.HashPassword(user, "bali123456");
        builder.Entity<IdentityUser>().HasData(user);

        // userrole
        builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
        {
            RoleId = roleAdmin.Id,
            UserId = user.Id
        });

        // keys
        builder.Entity<SewaBiayaDbModel>().HasKey(table => new {
            table.Id_Sewa,
            table.Id_JenisBiaya,
        });

        base.OnModelCreating(builder);
    }
}

