using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using rentcar.Models.Db;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rentcar.Models.View
{
    public class KendaraanViewModel
    {
        public int Id { get; set; }
        public int Id_Vendor { get; set; }
        public int Id_TipeKendaraan { get; set; }
        public string NoPolisi { get; set; } = "";
        public int? TahunPembuatan { get; set; }
        public string? Warna { get; set; }
        public DateTime? TanggalSamsat { get; set; }
        public DateTime? TanggalSamsat5Tahun { get; set; }
        public string? NomorMesin { get; set; }
        public string? STNKAtasNama { get; set; }
        public string Jenis { get; set; } = "";
        public byte[]? Foto { get; set; }
        public string Id_Vendor_Text { get; set; } = default!;
        public string Id_TipeKendaraan_Text { get; set; } = default!;
        public string Id_Merek_Text { get; set; } = default!;
        public bool HasFoto { get; set; }
    }

    public class KendaraanPostPutViewModel
    {
        [Required]
        public int Id_Vendor { get; set; }

        [Required]
        public int Id_TipeKendaraan { get; set; }

        [Required]
        public string NoPolisi { get; set; } = "";

        public int? TahunPembuatan { get; set; }

        public string? Warna { get; set; }

        public DateTime? TanggalSamsat { get; set; }

        public DateTime? TanggalSamsat5Tahun { get; set; }

        public string? NomorMesin { get; set; }

        public string? STNKAtasNama { get; set; }

        public byte[]? Foto { get; set; }
    }
}

