using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rentcar.Models.Db
{
	public class SewaBiayaDbModel
	{
        [Key]
        [ForeignKey("Sewa")]
        public int Id_Sewa { get; set; }

        [Key]
        [ForeignKey("JenisBiaya")]
        public int Id_JenisBiaya { get; set; }

        [Column(TypeName = "Money")]
        public decimal Biaya { get; set; }

        public bool Lunas { get; set; }

        [StringLength(500)]
        public string? Catatan { get; set; }

        [Column(TypeName = "Image")]
        public byte[]? FotoBukti { get; set; }

        public SewaDbModel Sewa { get; set; } = default!;
        public JenisBiayaDbModel JenisBiaya { get; set; } = default!;
    }
}