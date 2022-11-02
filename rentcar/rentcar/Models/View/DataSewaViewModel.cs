using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using rentcar.Models.Db;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rentcar.Models.View
{
    public class DataSewaViewModel
    {
        public int Id { get; set; }
        public string NoBukti { get; set; } = "";
        public int Id_Kendaraan { get; set; }
        public int Id_Customer { get; set; }
        public int Id_Admin { get; set; }
        public DateTime Tanggal { get; set; }
        public DateTime TanggalSewa { get; set; }
        public int LamaSewa { get; set; }
        public DateTime? TanggalDiambil { get; set; }
        public DateTime? TanggalDikembalian { get; set; }
        public decimal Harga { get; set; }
        public bool Batal { get; set; } = false;
        public string NoPolisi { get; set; } = default!;
        public string TipeKendaraan { get; set; } = default!;
        public string? Warna { get; set; }
        public string Id_Customer_Text { get; set; } = default!;
        public string Id_Admin_Text { get; set; } = default!;
    }

    public class DataSewaPostViewModel
    {
        public bool CustomerBaru { get; set; }
        public string? CustomerBaruNama { get; set; }
        public string? CustomerBaruAlamat { get; set; }
        public string? CustomerBaruEmail { get; set; }
        public string? CustomerBaruTelp { get; set; }

        [Required]
        public int Id_Kendaraan { get; set; }

        public int? Id_Customer { get; set; }

        [Required]
        public DateTime TanggalSewa { get; set; }

        [Required]
        public int LamaSewa { get; set; }

        [Required]
        public decimal Harga { get; set; }
    }

    public class DataSewaPutViewModel
    {
        [Required]
        public int Id_Kendaraan { get; set; }

        [Required]
        public int Id_Customer { get; set; }

        [Required]
        public DateTime TanggalSewa { get; set; }

        [Required]
        public int LamaSewa { get; set; }

        [Required]
        public decimal Harga { get; set; }
    }
}

