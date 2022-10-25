using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace rentcar.Models.Db
{
	public class KendaraanDbModel
	{
        [Key]
		public int Id { get; set; }

        [ForeignKey("Vendor")]
        public int Id_Vendor { get; set; }

        [ForeignKey("TipeKendaraan")]
        public int Id_TipeKendaraan { get; set; }

        [StringLength(50)]
        public string NoPolisi { get; set; } = "";

        public int? TahunPembuatan { get; set; }

        [StringLength(50)]
        public string? Warna { get; set; }

        [Column(TypeName = "Date")]
        public DateTime? TanggalSamsat { get; set; }

        [Column(TypeName = "Date")]
        public DateTime? TanggalSamsat5Tahun { get; set; }

        [StringLength(50)]
        public string? NomorMesin { get; set; }

        [StringLength(200)]
        public string? STNKAtasNama { get; set; }

        [Column(TypeName = "Image")]
        public byte[]? Foto { get; set; }

        public VendorDbModel Vendor { get; set; } = default!;
        public TipeKendaraanDbModel TipeKendaraan { get; set; } = default!;
    }
}