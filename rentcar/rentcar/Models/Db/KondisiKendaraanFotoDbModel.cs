using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rentcar.Models.Db
{
	public class KondisiKendaraanFotoDbModel
	{
        [Key]
		public int Id { get; set; }

        [ForeignKey("KondisiKendaraan")]
        public int Id_KondisiKendaraan { get; set; }

        [Required]
		public string FotoFileName { get; set; } = "";

        [Column(TypeName = "Image")]
        public byte[]? Foto { get; set; }

        public KondisiKendaraanDbModel KondisiKendaraan { get; set; } = default!;
    }
}