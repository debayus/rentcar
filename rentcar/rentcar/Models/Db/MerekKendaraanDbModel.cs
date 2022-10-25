using System;
using System.ComponentModel.DataAnnotations;

namespace rentcar.Models.Db
{
	public class MerekKendaraanDbModel
	{
        [Key]
		public int Id { get; set; }

        [StringLength(100)]
		public string Nama { get; set; } = "";
    }
}