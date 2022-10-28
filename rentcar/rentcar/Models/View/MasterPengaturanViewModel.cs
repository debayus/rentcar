using System;
using System.ComponentModel.DataAnnotations;

namespace rentcar.Models.View
{
    public class MasterPengaturanViewModel
    {
        public string? Perusahaan { get; set; }
        public string? Telp { get; set; }
        public string? Website { get; set; }
        public string? Alamat { get; set; }
        public double? DP { get; set; }
        public string? Bank1 { get; set; }
        public string? Bank2 { get; set; }

        [Display(Name = "Tampilkan Bank 2")]
        public bool? Bank2Tampilkan { get; set; }
    }
}

