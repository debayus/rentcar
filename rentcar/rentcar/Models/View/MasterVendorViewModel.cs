using System;
using System.ComponentModel.DataAnnotations;

namespace rentcar.Models.View
{
    public class MasterVendorViewModel
    {
        public int Id { get; set; }
        public string Nama { get; set; } = "";
        public string UserName { get; set; } = "";
        public string? Telp { get; set; }
        public string? Alamat { get; set; }
        public string Email { get; set; } = "";
    }

    public class MasterVendorPostViewModel
    {
        [Required]
        public string Nama { get; set; } = "";

        [Required]
        public string UserName { get; set; } = "";

        public string? Telp { get; set; }

        public string? Alamat { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        public string Password { get; set; } = "";
    }

    public class MasterVendorPutViewModel
    {
        [Required]
        public string Nama { get; set; } = "";

        [Required]
        public string UserName { get; set; } = "";

        public string? Telp { get; set; }

        public string? Alamat { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";
    }
}

