using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using rentcar.Data;
using rentcar.Models;
using rentcar.Models.Db;
using Mahas.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace rentcar.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<AdminController> _logger;

    public AdminController(ApplicationDbContext db, ILogger<AdminController> logger)
    {
        _logger = logger;
        _db = db;
    }

    public IActionResult Index()
    {
        _db.mAdmin.Join(_db.Users, x => x.Id_User, y => y.Id, (x , y) => new
        {
            admin = x,
            user = y
        }).Select(x => new
        {
            Nama = x.admin.Nama,
            Email = x.user.Email
        }).Take(10);

        _db.mAdmin.GroupJoin(_db.Users, x => x.Id_User, y => y.Id, (x, y) => new
        {
            admin = x,
            user = y
        }).SelectMany(x => x.user.DefaultIfEmpty(), (x, y) => new
        {
            Nama = x.admin.Nama,
            Email = y == null ? "" : y.Email
        });

        return View();
    }
}