using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using rentcar.Models.Db;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rentcar.Models.View
{
    public class MasterTipeKendaraanViewModel
    {
        public int Id { get; set; }
        public int Id_MerekKendaraan { get; set; }
        public int? Id_JenisBahanBakar { get; set; }
        public string Nama { get; set; } = "";
        public string Jenis { get; set; } = "";
        public string Transmisi { get; set; } = "";
        public decimal Harga { get; set; }
        public string Id_MerekKendaraan_Text { get; set; } = "";
        public string? Id_JenisBahanBakar_Text { get; set; }
    }

    public class MasterTipeKendaraanPostPutViewModel
    {
        [Required]
        [Display(Name = "Merek")]
        public int Id_MerekKendaraan { get; set; }

        [Display(Name = "Jenis Bahan Bakar")]
        public int? Id_JenisBahanBakar { get; set; }

        [Required]
        public string Nama { get; set; } = "";

        [Required]
        public string Jenis { get; set; } = "";

        [Required]
        public string Transmisi { get; set; } = "";

        [Required]
        public decimal Harga { get; set; }
    }
}

