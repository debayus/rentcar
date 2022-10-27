using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rentcar.Migrations
{
    public partial class rentcar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "mJenisBahanBakar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nama = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mJenisBahanBakar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "mJenisBiaya",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nama = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mJenisBiaya", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "mKonfigurasi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nama = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mKonfigurasi", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "mMerekKendaraan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nama = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mMerekKendaraan", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mAdmin",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_User = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Nama = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mAdmin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_mAdmin_AspNetUsers_Id_User",
                        column: x => x.Id_User,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mCustomer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_User = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Nama = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Alamat = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FotoKTP = table.Column<byte[]>(type: "Image", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mCustomer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_mCustomer_AspNetUsers_Id_User",
                        column: x => x.Id_User,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mVendor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_User = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Nama = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Alamat = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mVendor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_mVendor_AspNetUsers_Id_User",
                        column: x => x.Id_User,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mTipeKendaraan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_MerekKendaraan = table.Column<int>(type: "int", nullable: false),
                    Id_JenisBahanBakar = table.Column<int>(type: "int", nullable: true),
                    Nama = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Jenis = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Transmisi = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Harga = table.Column<decimal>(type: "Money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mTipeKendaraan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_mTipeKendaraan_mJenisBahanBakar_Id_JenisBahanBakar",
                        column: x => x.Id_JenisBahanBakar,
                        principalTable: "mJenisBahanBakar",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_mTipeKendaraan_mMerekKendaraan_Id_MerekKendaraan",
                        column: x => x.Id_MerekKendaraan,
                        principalTable: "mMerekKendaraan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mKendaraan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_Vendor = table.Column<int>(type: "int", nullable: false),
                    Id_TipeKendaraan = table.Column<int>(type: "int", nullable: false),
                    NoPolisi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TahunPembuatan = table.Column<int>(type: "int", nullable: true),
                    Warna = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TanggalSamsat = table.Column<DateTime>(type: "Date", nullable: true),
                    TanggalSamsat5Tahun = table.Column<DateTime>(type: "Date", nullable: true),
                    NomorMesin = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    STNKAtasNama = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Foto = table.Column<byte[]>(type: "Image", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mKendaraan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_mKendaraan_mTipeKendaraan_Id_TipeKendaraan",
                        column: x => x.Id_TipeKendaraan,
                        principalTable: "mTipeKendaraan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_mKendaraan_mVendor_Id_Vendor",
                        column: x => x.Id_Vendor,
                        principalTable: "mVendor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "trSewa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoBukti = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Id_Kendaraan = table.Column<int>(type: "int", nullable: false),
                    Id_Customer = table.Column<int>(type: "int", nullable: false),
                    Id_Admin = table.Column<int>(type: "int", nullable: false),
                    Tanggal = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TanggalSewa = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LamaSewa = table.Column<int>(type: "int", nullable: false),
                    TanggalDiambil = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TanggalDikembalian = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Harga = table.Column<decimal>(type: "Money", nullable: false),
                    Batal = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trSewa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_trSewa_mAdmin_Id_Admin",
                        column: x => x.Id_Admin,
                        principalTable: "mAdmin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_trSewa_mCustomer_Id_Customer",
                        column: x => x.Id_Customer,
                        principalTable: "mCustomer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_trSewa_mKendaraan_Id_Kendaraan",
                        column: x => x.Id_Kendaraan,
                        principalTable: "mKendaraan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "trKondisiKendaraan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_Sewa = table.Column<int>(type: "int", nullable: true),
                    Id_Kendaraan = table.Column<int>(type: "int", nullable: false),
                    Id_Admin = table.Column<int>(type: "int", nullable: false),
                    Tanggal = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Kilometer = table.Column<int>(type: "int", nullable: true),
                    Bensin = table.Column<int>(type: "int", nullable: true),
                    Catatan = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Kelengkapan = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trKondisiKendaraan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_trKondisiKendaraan_mAdmin_Id_Admin",
                        column: x => x.Id_Admin,
                        principalTable: "mAdmin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_trKondisiKendaraan_mKendaraan_Id_Kendaraan",
                        column: x => x.Id_Kendaraan,
                        principalTable: "mKendaraan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_trKondisiKendaraan_trSewa_Id_Sewa",
                        column: x => x.Id_Sewa,
                        principalTable: "trSewa",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "trSewaBiaya",
                columns: table => new
                {
                    Id_Sewa = table.Column<int>(type: "int", nullable: false),
                    Id_JenisBiaya = table.Column<int>(type: "int", nullable: false),
                    Biaya = table.Column<decimal>(type: "Money", nullable: false),
                    Lunas = table.Column<bool>(type: "bit", nullable: false),
                    Catatan = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FotoBukti = table.Column<byte[]>(type: "Image", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trSewaBiaya", x => new { x.Id_Sewa, x.Id_JenisBiaya });
                    table.ForeignKey(
                        name: "FK_trSewaBiaya_mJenisBiaya_Id_JenisBiaya",
                        column: x => x.Id_JenisBiaya,
                        principalTable: "mJenisBiaya",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_trSewaBiaya_trSewa_Id_Sewa",
                        column: x => x.Id_Sewa,
                        principalTable: "trSewa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "trSewaPerjanjian",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Tanggal = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NamaUsaha = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Telp = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Alamat = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trSewaPerjanjian", x => x.Id);
                    table.ForeignKey(
                        name: "FK_trSewaPerjanjian_trSewa_Id",
                        column: x => x.Id,
                        principalTable: "trSewa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "trKondisiKendaraanFoto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_KondisiKendaraan = table.Column<int>(type: "int", nullable: false),
                    Nama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Foto = table.Column<byte[]>(type: "Image", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trKondisiKendaraanFoto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_trKondisiKendaraanFoto_trKondisiKendaraan_Id_KondisiKendaraan",
                        column: x => x.Id_KondisiKendaraan,
                        principalTable: "trKondisiKendaraan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "048fc5cf-1a1d-42fd-a7be-c8d60964ecd8", "b09903ac-eedf-4311-ac62-2f70dd2d1d4f", "Vendor", "Vendor" },
                    { "3a2bbc2b-b3bb-43ff-ad41-4b613092f780", "a2f5a909-4464-475a-8154-1a0a73c89ec7", "Admin", "Admin" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "acfea0f0-134a-4d41-8b13-33827f3341c1", 0, "a8e797f0-90ca-4318-a78b-0e01ad882acf", "admin@rentcar.com", true, false, null, "ADMIN@RENTCAR.COM", "ADMIN", "AQAAAAEAACcQAAAAEMHqCKx2L3Sj/OOH5Zeqd15rH5Y04PERC1Am35m+LNIwS/HwbIVtDnC2/JVqecI/8w==", null, false, "", false, "admin" });

            migrationBuilder.InsertData(
                table: "mJenisBahanBakar",
                columns: new[] { "Id", "Nama" },
                values: new object[,]
                {
                    { 1, "Bensin" },
                    { 2, "Solar" }
                });

            migrationBuilder.InsertData(
                table: "mJenisBiaya",
                columns: new[] { "Id", "Nama" },
                values: new object[,]
                {
                    { 1, "DP" },
                    { 2, "Sisa Bayar" },
                    { 3, "Kerusakan" }
                });

            migrationBuilder.InsertData(
                table: "mKonfigurasi",
                columns: new[] { "Id", "Nama", "Value" },
                values: new object[,]
                {
                    { 1, "Perusahaan", "RENT CAR" },
                    { 2, "Telp", "081234567890" },
                    { 3, "Website", "debayus.mahas.my.id" },
                    { 4, "Alamat", "Jln. Jalan No.26, Jimbaran, Badung, Bali" },
                    { 5, "DP", "50" },
                    { 6, "Bank1", "" },
                    { 7, "Bank2", "" },
                    { 8, "Bank2Tampilkan", "False" }
                });

            migrationBuilder.InsertData(
                table: "mMerekKendaraan",
                columns: new[] { "Id", "Nama" },
                values: new object[,]
                {
                    { 1, "Toyota" },
                    { 2, "Honda" },
                    { 3, "Suzuki" },
                    { 4, "Mazda" },
                    { 5, "Daihatsu" },
                    { 6, "Chevrolet" },
                    { 7, "Jeep" },
                    { 8, "Mitsubishi" },
                    { 9, "Nissan" },
                    { 10, "Datsun" },
                    { 11, "Yamaha" },
                    { 12, "Kawasaki" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "3a2bbc2b-b3bb-43ff-ad41-4b613092f780", "acfea0f0-134a-4d41-8b13-33827f3341c1" });

            migrationBuilder.InsertData(
                table: "mAdmin",
                columns: new[] { "Id", "Id_User", "Nama" },
                values: new object[] { 1, "acfea0f0-134a-4d41-8b13-33827f3341c1", "Admin" });

            migrationBuilder.InsertData(
                table: "mTipeKendaraan",
                columns: new[] { "Id", "Harga", "Id_JenisBahanBakar", "Id_MerekKendaraan", "Jenis", "Nama", "Transmisi" },
                values: new object[,]
                {
                    { 1, 150000m, 1, 1, "Mobil", "Agya", "Manual" },
                    { 2, 150000m, 1, 1, "Mobil", "Avanza", "Manual" },
                    { 3, 150000m, 1, 1, "Mobil", "Calya", "Manual" },
                    { 4, 150000m, 1, 1, "Mobil", "Agya", "Matic" },
                    { 5, 150000m, 1, 1, "Mobil", "Avanza", "Matic" },
                    { 6, 150000m, 1, 1, "Mobil", "Calya", "Matic" },
                    { 7, 30000m, 1, 2, "Motor", "Beat", "Matic" },
                    { 8, 30000m, 1, 2, "Motor", "Vario", "Matic" },
                    { 9, 30000m, 1, 2, "Motor", "Scoopy", "Matic" },
                    { 10, 30000m, 1, 11, "Motor", "N-Max", "Matic" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_mAdmin_Id_User",
                table: "mAdmin",
                column: "Id_User");

            migrationBuilder.CreateIndex(
                name: "IX_mCustomer_Id_User",
                table: "mCustomer",
                column: "Id_User");

            migrationBuilder.CreateIndex(
                name: "IX_mKendaraan_Id_TipeKendaraan",
                table: "mKendaraan",
                column: "Id_TipeKendaraan");

            migrationBuilder.CreateIndex(
                name: "IX_mKendaraan_Id_Vendor",
                table: "mKendaraan",
                column: "Id_Vendor");

            migrationBuilder.CreateIndex(
                name: "IX_mTipeKendaraan_Id_JenisBahanBakar",
                table: "mTipeKendaraan",
                column: "Id_JenisBahanBakar");

            migrationBuilder.CreateIndex(
                name: "IX_mTipeKendaraan_Id_MerekKendaraan",
                table: "mTipeKendaraan",
                column: "Id_MerekKendaraan");

            migrationBuilder.CreateIndex(
                name: "IX_mVendor_Id_User",
                table: "mVendor",
                column: "Id_User");

            migrationBuilder.CreateIndex(
                name: "IX_trKondisiKendaraan_Id_Admin",
                table: "trKondisiKendaraan",
                column: "Id_Admin");

            migrationBuilder.CreateIndex(
                name: "IX_trKondisiKendaraan_Id_Kendaraan",
                table: "trKondisiKendaraan",
                column: "Id_Kendaraan");

            migrationBuilder.CreateIndex(
                name: "IX_trKondisiKendaraan_Id_Sewa",
                table: "trKondisiKendaraan",
                column: "Id_Sewa");

            migrationBuilder.CreateIndex(
                name: "IX_trKondisiKendaraanFoto_Id_KondisiKendaraan",
                table: "trKondisiKendaraanFoto",
                column: "Id_KondisiKendaraan");

            migrationBuilder.CreateIndex(
                name: "IX_trSewa_Id_Admin",
                table: "trSewa",
                column: "Id_Admin");

            migrationBuilder.CreateIndex(
                name: "IX_trSewa_Id_Customer",
                table: "trSewa",
                column: "Id_Customer");

            migrationBuilder.CreateIndex(
                name: "IX_trSewa_Id_Kendaraan",
                table: "trSewa",
                column: "Id_Kendaraan");

            migrationBuilder.CreateIndex(
                name: "IX_trSewaBiaya_Id_JenisBiaya",
                table: "trSewaBiaya",
                column: "Id_JenisBiaya");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "mKonfigurasi");

            migrationBuilder.DropTable(
                name: "trKondisiKendaraanFoto");

            migrationBuilder.DropTable(
                name: "trSewaBiaya");

            migrationBuilder.DropTable(
                name: "trSewaPerjanjian");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "trKondisiKendaraan");

            migrationBuilder.DropTable(
                name: "mJenisBiaya");

            migrationBuilder.DropTable(
                name: "trSewa");

            migrationBuilder.DropTable(
                name: "mAdmin");

            migrationBuilder.DropTable(
                name: "mCustomer");

            migrationBuilder.DropTable(
                name: "mKendaraan");

            migrationBuilder.DropTable(
                name: "mTipeKendaraan");

            migrationBuilder.DropTable(
                name: "mVendor");

            migrationBuilder.DropTable(
                name: "mJenisBahanBakar");

            migrationBuilder.DropTable(
                name: "mMerekKendaraan");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
