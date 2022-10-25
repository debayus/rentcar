using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rentcar.Models.Db
{
	public class SewaDbModel
	{
        [Key]
		public int Id { get; set; }

        [StringLength(50)]
		public string NoBukti { get; set; } = "";

        [ForeignKey("Kendaraan")]
        public int Id_Kendaraan { get; set; }

        [ForeignKey("Customer")]
        public int Id_Customer { get; set; }

        [ForeignKey("Admin")]
        public int Id_Admin { get; set; }

        public DateTime Tanggal { get; set; }

        public DateTime TanggalSewa { get; set; }

        public int LamaSewa { get; set; }

        public DateTime? TanggalDiambil { get; set; }

        public DateTime? TanggalDikembalian { get; set; }

        [Column(TypeName = "Money")]
        public decimal Harga { get; set; }

        public bool Batal { get; set; } = false;

        public KendaraanDbModel Kendaraan { get; set; } = default!;
        public CustomerDbModel Customer { get; set; } = default!;
        public AdminDbModel Admin { get; set; } = default!;
    }
}