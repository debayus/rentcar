using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rentcar.Models.Db
{
	public class KondisiKendaraanDbModel
	{
        [Key]
		public int Id { get; set; }

        [ForeignKey("Sewa")]
        public int? Id_Sewa { get; set; }

        [ForeignKey("Kendaraan")]
        public int Id_Kendaraan { get; set; }

        [ForeignKey("Admin")]
        public int Id_Admin { get; set; }

        public DateTime Tanggal { get; set; }

        public int? Kilometer { get; set; }

        public int? Bensin { get; set; }

        [StringLength(500)]
        public string? Catatan { get; set; }

        [StringLength(500)]
        public string? Kelengkapan { get; set; }

        public SewaDbModel? Sewa { get; set; }
        public KendaraanDbModel Kendaraan { get; set; } = default!;
        public AdminDbModel Admin { get; set; } = default!;
    }
}