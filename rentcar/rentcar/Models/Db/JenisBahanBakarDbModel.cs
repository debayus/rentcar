using System;
using System.ComponentModel.DataAnnotations;

namespace rentcar.Models.Db
{
	public class JenisBahanBakarDbModel
	{
        [Key]
		public int Id { get; set; }

        [StringLength(100)]
		public string Nama { get; set; } = "";
    }
}