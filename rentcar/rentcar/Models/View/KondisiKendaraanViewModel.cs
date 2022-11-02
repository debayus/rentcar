using System;
using rentcar.Models.Db;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rentcar.Models.View
{
    public class KondisiKendaraanViewModel
    {
        public int Id { get; set; }
        public int? Id_Sewa { get; set; }
        public int Id_Kendaraan { get; set; }
        public int Id_Admin { get; set; }
        public DateTime Tanggal { get; set; }
        public int? Kilometer { get; set; }
        public int? Bensin { get; set; }
        public string? Catatan { get; set; }
        public string? Kelengkapan { get; set; }
        public string KendaraanNoPolisi { get; set; } = default!;
        public string KendaraanTipeKendaraan { get; set; } = default!;
        public string? KendaraanWarna { get; set; }
        public string AdminNama { get; set; } = default!;
        public string Jenis { get; set; } = default!;
    }

    public class KondisiKendaraanPostPutViewModel
    {
        public int? Id_Sewa { get; set; }

        [Required]
        public int Id_Kendaraan { get; set; }

        public int Id_Admin { get; set; }

        [Required]
        public DateTime Tanggal { get; set; }

        public int? Kilometer { get; set; }

        public int? Bensin { get; set; }

        public string? Catatan { get; set; }

        public string? Kelengkapan { get; set; }
    }
}

