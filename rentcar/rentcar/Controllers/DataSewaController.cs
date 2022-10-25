using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using rentcar.Data;

namespace rentcar.Controllers;

public class DataSewaController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<DataSewaController> _logger;

    public DataSewaController(ApplicationDbContext db, ILogger<DataSewaController> logger)
    {
        _logger = logger;
        _db = db;
    }

    public IActionResult Index()
    {
        return View();
    }
}
