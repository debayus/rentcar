using System;
using System.ComponentModel.DataAnnotations;

namespace rentcar.Models.Db
{
	public class KonfigurasiDbModel
	{
        [Key]
		public int Id { get; set; }

        [StringLength(100)]
        public string Nama { get; set; } = "";

        [StringLength(1000)]
        public string? Value { get; set; }
    }
}