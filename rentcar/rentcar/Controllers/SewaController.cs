using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using rentcar.Data;

namespace rentcar.Controllers;

public class SewaController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<SewaController> _logger;

    public SewaController(ApplicationDbContext db, ILogger<SewaController> logger)
    {
        _logger = logger;
        _db = db;
    }

    public IActionResult Index()
    {
        return View();
    }
}

