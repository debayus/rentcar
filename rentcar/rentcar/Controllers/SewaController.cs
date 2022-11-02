using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Mahas.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using rentcar.Data;
using rentcar.Models.Db;
using rentcar.Models.View;

namespace rentcar.Controllers;

[Authorize]
public class SewaController : Controller
{

    private readonly ApplicationDbContext _db;
    private readonly ILogger<DataSewaController> _logger;

    public SewaController(ApplicationDbContext db, ILogger<DataSewaController> logger)
    {
        _logger = logger;
        _db = db;
    }

    public IActionResult Index()
    {
        return View();
    }
}
