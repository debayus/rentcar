using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rentcar.Data;

namespace rentcar.Controllers;

[Authorize(Roles = "Admin")]
public class KondisiKendaraanController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<KondisiKendaraanController> _logger;

    public KondisiKendaraanController(ApplicationDbContext db, ILogger<KondisiKendaraanController> logger)
    {
        _logger = logger;
        _db = db;
    }

    public IActionResult Index()
    {
        return View();
    }
}

