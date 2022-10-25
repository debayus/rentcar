using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using rentcar.Data;

namespace rentcar.Controllers;

public class KendaraanController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<KendaraanController> _logger;

    public KendaraanController(ApplicationDbContext db, ILogger<KendaraanController> logger)
    {
        _logger = logger;
        _db = db;
    }

    public IActionResult Index()
    {
        return View();
    }
}

