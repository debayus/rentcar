using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using rentcar.Data;

namespace rentcar.Controllers;

public class LaporanController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<LaporanController> _logger;

    public LaporanController(ApplicationDbContext db, ILogger<LaporanController> logger)
    {
        _logger = logger;
        _db = db;
    }

    public IActionResult Index()
    {
        return View();
    }
}

