using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using rentcar.Models.Db;

namespace rentcar.Data;

public class  ApplicationDbContext: IdentityDbContext
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
            NormalizedUserName = "ADMIN",
            Email = "admin@rentcar.com",
            NormalizedEmail = "ADMIN@RENTCAR.COM",
            EmailConfirmed = true,
            SecurityStamp = string.Empty,
        };
        user.PasswordHash = hasher.HashPassword(user, "bali123456");
        builder.Entity<IdentityUser>().HasData(user);

        // userrole
        builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
        {
            RoleId = roleAdmin.Id,
            UserId = user.Id
        });

        // admin
        builder.Entity<AdminDbModel>().HasData(
            new AdminDbModel()
            {
                Id = 1,
                Id_User = user.Id,
                Nama = "Admin",
            }
        );

        // keys
        builder.Entity<SewaBiayaDbModel>().HasKey(table => new
        {
            table.Id_Sewa,
            table.Id_JenisBiaya,
        });

        // foreign key
        builder.Entity<SewaDbModel>().HasOne(c => c.Customer).WithMany().OnDelete(DeleteBehavior.Restrict);
        builder.Entity<SewaDbModel>().HasOne(c => c.Kendaraan).WithMany().OnDelete(DeleteBehavior.Restrict);
        builder.Entity<KondisiKendaraanDbModel>().HasOne(c => c.Kendaraan).WithMany().OnDelete(DeleteBehavior.Restrict);

        // add data
        var bensin = new JenisBahanBakarDbModel() { Nama = "Bensin", Id = 1 };
        var solar = new JenisBahanBakarDbModel() { Nama = "Solar", Id = 2 };
        builder.Entity<JenisBahanBakarDbModel>().HasData(
            bensin,
            solar
        );
        builder.Entity<JenisBiayaDbModel>().HasData(
            new JenisBiayaDbModel() { Nama = "DP", Id = 1 },
            new JenisBiayaDbModel() { Nama = "Sisa Bayar", Id = 2 },
            new JenisBiayaDbModel() { Nama = "Kerusakan", Id = 3 }
        );
        builder.Entity<KonfigurasiDbModel>().HasData(
            new KonfigurasiDbModel() { Nama = "Perusahaan", Value = "RENT CAR", Id = 1 },
            new KonfigurasiDbModel() { Nama = "Telp", Value = "081234567890", Id = 2 },
            new KonfigurasiDbModel() { Nama = "Website", Value = "debayus.mahas.my.id", Id = 3 },
            new KonfigurasiDbModel() { Nama = "Alamat", Value = "Jln. Jalan No.26, Jimbaran, Badung, Bali", Id = 4 },
            new KonfigurasiDbModel() { Nama = "DP", Value = "50", Id = 5 },
            new KonfigurasiDbModel() { Nama = "Bank1", Value = "", Id = 6 },
            new KonfigurasiDbModel() { Nama = "Bank2", Value = "", Id = 7 },
            new KonfigurasiDbModel() { Nama = "Bank2Tampilkan", Value = "False", Id = 8 }
        );

        var toyota = new MerekKendaraanDbModel() { Nama = "Toyota", Id = 1 };
        var honda = new MerekKendaraanDbModel() { Nama = "Honda", Id = 2 };
        var suzuki = new MerekKendaraanDbModel() { Nama = "Suzuki", Id = 3 };
        var mazda = new MerekKendaraanDbModel() { Nama = "Mazda", Id = 4 };
        var daihatsu = new MerekKendaraanDbModel() { Nama = "Daihatsu", Id = 5 };
        var chevrolet = new MerekKendaraanDbModel() { Nama = "Chevrolet", Id = 6 };
        var jeep = new MerekKendaraanDbModel() { Nama = "Jeep", Id = 7 };
        var mitsubishi = new MerekKendaraanDbModel() { Nama = "Mitsubishi", Id = 8 };
        var nissan = new MerekKendaraanDbModel() { Nama = "Nissan", Id = 9 };
        var datsun = new MerekKendaraanDbModel() { Nama = "Datsun", Id = 10 };
        var yamaha = new MerekKendaraanDbModel() { Nama = "Yamaha", Id = 11 };
        var kawasaki = new MerekKendaraanDbModel() { Nama = "Kawasaki", Id = 12 };
        builder.Entity<MerekKendaraanDbModel>().HasData(
            toyota,
            honda,
            suzuki,
            mazda,
            daihatsu,
            chevrolet,
            jeep,
            mitsubishi,
            nissan,
            datsun,
            yamaha,
            kawasaki
        );

        #region Kendaraan
        builder.Entity<TipeKendaraanDbModel>().HasData(
            new TipeKendaraanDbModel()
            {
                Id = 1,
                Id_MerekKendaraan = toyota.Id,
                Id_JenisBahanBakar = bensin.Id,
                Harga = 150000,
                Jenis = "Mobil",
                Transmisi = "Manual",
                Nama = "Agya"
            },
            new TipeKendaraanDbModel()
            {
                Id = 2,
                Id_MerekKendaraan = toyota.Id,
                Id_JenisBahanBakar = bensin.Id,
                Harga = 150000,
                Jenis = "Mobil",
                Transmisi = "Manual",
                Nama = "Avanza"
            },
            new TipeKendaraanDbModel()
            {
                Id = 3,
                Id_MerekKendaraan = toyota.Id,
                Id_JenisBahanBakar = bensin.Id,
                Harga = 150000,
                Jenis = "Mobil",
                Transmisi = "Manual",
                Nama = "Calya"
            },
            new TipeKendaraanDbModel()
            {
                Id = 4,
                Id_MerekKendaraan = toyota.Id,
                Id_JenisBahanBakar = bensin.Id,
                Harga = 150000,
                Jenis = "Mobil",
                Transmisi = "Matic",
                Nama = "Agya"
            },
            new TipeKendaraanDbModel()
            {
                Id = 5,
                Id_MerekKendaraan = toyota.Id,
                Id_JenisBahanBakar = bensin.Id,
                Harga = 150000,
                Jenis = "Mobil",
                Transmisi = "Matic",
                Nama = "Avanza"
            },
            new TipeKendaraanDbModel()
            {
                Id = 6,
                Id_MerekKendaraan = toyota.Id,
                Id_JenisBahanBakar = bensin.Id,
                Harga = 150000,
                Jenis = "Mobil",
                Transmisi = "Matic",
                Nama = "Calya"
            },
            new TipeKendaraanDbModel()
            {
                Id = 7,
                Id_MerekKendaraan = honda.Id,
                Id_JenisBahanBakar = bensin.Id,
                Harga = 30000,
                Jenis = "Motor",
                Transmisi = "Matic",
                Nama = "Beat"
            },
            new TipeKendaraanDbModel()
            {
                Id = 8,
                Id_MerekKendaraan = honda.Id,
                Id_JenisBahanBakar = bensin.Id,
                Harga = 30000,
                Jenis = "Motor",
                Transmisi = "Matic",
                Nama = "Vario"
            },
            new TipeKendaraanDbModel()
            {
                Id = 9,
                Id_MerekKendaraan = honda.Id,
                Id_JenisBahanBakar = bensin.Id,
                Harga = 30000,
                Jenis = "Motor",
                Transmisi = "Matic",
                Nama = "Scoopy"
            },
            new TipeKendaraanDbModel()
            {
                Id = 10,
                Id_MerekKendaraan = yamaha.Id,
                Id_JenisBahanBakar = bensin.Id,
                Harga = 30000,
                Jenis = "Motor",
                Transmisi = "Matic",
                Nama = "N-Max"
            }
        );

        #endregion

        base.OnModelCreating(builder);
    }
}

