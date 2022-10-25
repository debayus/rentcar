using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rentcar.Models.Db
{
	public class TipeKendaraanDbModel
	{
        [Key]
		public int Id { get; set; }

        [ForeignKey("MerekKendaraan")]
        public int Id_MerekKendaraan { get; set; }

        [ForeignKey("JenisBahanBakar")]
        public int? Id_JenisBahanBakar { get; set; }

        [StringLength(100)]
		public string Nama { get; set; } = "";

        [StringLength(10)]
        public string Jenis { get; set; } = ""; // Motor, Mobil

        [StringLength(10)]
        public string Transmisi { get; set; } = ""; // Matic, Manual

        [Column(TypeName = "Money")]
        public decimal Harga { get; set; }

        public MerekKendaraanDbModel MerekKendaraan { get; set; } = default!;
        public JenisBahanBakarDbModel? JenisBahanBakar { get; set; }

    }
}