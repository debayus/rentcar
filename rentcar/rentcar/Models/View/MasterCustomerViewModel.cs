using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace rentcar.Models.View
{
    public class MasterCustomerViewModel
    {
        public int Id { get; set; }
        public string Nama { get; set; } = "";
        public string UserName { get; set; } = "";
        public string? Telp { get; set; }
        public string Email { get; set; } = "";
        public string? Alamat { get; set; }
    }

    public class MasterCustomerPostViewModel
    {
        [Required]
        public string Nama { get; set; } = "";

        [Required]
        public string UserName { get; set; } = "";

        public string? Telp { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        public string Password { get; set; } = "";

        public string? Alamat { get; set; }

        public IFormFile? FotoKTP { get; set; }
    }

    public class MasterCustomerPutViewModel
    {
        [Required]
        public string Nama { get; set; } = "";

        [Required]
        public string UserName { get; set; } = "";

        public string? Telp { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        public string? Alamat { get; set; }

        public IFormFile? FotoKTP { get; set; }
    }
}

