using System;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rentcar.Models.View
{
    public class MasterAdminViewModel
    {
        public int Id { get; set; }
        public string Nama { get; set; } = "";
        public string UserName { get; set; } = "";
        public string? Telp { get; set; }
        public string Email { get; set; } = "";
    }

    public class MasterAdminPostViewModel
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
    }

    public class MasterAdminPutViewModel
    {
        [Required]
        public string Nama { get; set; } = "";

        [Required]
        public string UserName { get; set; } = "";

        public string? Telp { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";
    }
}

