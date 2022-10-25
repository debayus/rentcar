using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using rentcar.Data;

namespace rentcar.Controllers;

public class KendaraanVendorController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<KendaraanVendorController> _logger;

    public KendaraanVendorController(ApplicationDbContext db, ILogger<KendaraanVendorController> logger)
    {
        _logger = logger;
        _db = db;
    }

    public IActionResult Index()
    {
        return View();
    }
}

