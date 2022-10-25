using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rentcar.Models.Db
{
	public class SewaPerjanjianDbModel
	{
        [Key]
        [ForeignKey("Sewa")]
		public int Id { get; set; }

        public DateTime Tanggal { get; set; }

        [StringLength(200)]
        public string NamaUsaha { get; set; } = "";

        [StringLength(100)]
        public string? Telp { get; set; }

        [StringLength(500)]
        public string? Alamat { get; set; }

        [StringLength(100)]
        public string? Website { get; set; }

        public SewaDbModel Sewa { get; set; } = default!;
    }
}