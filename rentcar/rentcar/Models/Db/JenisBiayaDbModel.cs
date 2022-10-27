using System;
using System.ComponentModel.DataAnnotations;

namespace rentcar.Models.Db
{
	public class JenisBiayaDbModel
	{
        [Key]
		public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nama { get; set; } = "";
    }
}