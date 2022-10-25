using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using rentcar.Data;

namespace rentcar.Controllers;

public class KonfigurasiController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<KonfigurasiController> _logger;

    public KonfigurasiController(ApplicationDbContext db, ILogger<KonfigurasiController> logger)
    {
        _logger = logger;
        _db = db;
    }

    public IActionResult Index()
    {
        return View();
    }
}

