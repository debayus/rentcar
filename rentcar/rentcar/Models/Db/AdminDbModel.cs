using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace rentcar.Models.Db
{
	public class AdminDbModel
	{
        [Key]
		public int Id { get; set; }

        [ForeignKey("User")]
        [StringLength(450)]
        public string Id_User { get; set; } = "";

        [StringLength(200)]
		public string Nama { get; set; } = "";

        [StringLength(100)]
        public string? Telp { get; set; }

        public IdentityUser User { get; set; } = default!;
    }
}
 