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
public class MasterJenisBahanBakarController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<MasterJenisBahanBakarController> _logger;

    public MasterJenisBahanBakarController(ApplicationDbContext db, ILogger<MasterJenisBahanBakarController> logger)
    {
        _logger = logger;
        _db = db;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Datas(int pageSize, int pageIndex, int orderBy, OrderByTypeEnum orderByType, List<FilterModel> filter)
    {
        IEnumerable<JenisBahanBakarDbModel> query = _db.mJenisBahanBakar;

        foreach (var _filter in filter)
        {
            switch (_filter.Key)
            {
                case "Filter":
                    if (string.IsNullOrEmpty(_filter.Value)) break;
                    _filter.Value = _filter.Value.ToUpper();
                    query = query.Where(x => (x.Nama ?? "").ToUpper().Contains(_filter.Value));
                    break;
            }
        }

        var totalRowCount = query.Count();
        Func<JenisBahanBakarDbModel, dynamic> _orderBy = (x =>
            orderBy == 0 ? x.Id :
            orderBy == 1 ? x.Nama :
            x.Id
        );
        query = orderByType == OrderByTypeEnum.ASC ? query.OrderBy(_orderBy) : query.OrderByDescending(_orderBy);
        var datas = query.Skip(pageSize * pageIndex).Take(pageSize).ToList();

        return Json(new
        {
            Datas = datas,
            PageIndex = pageIndex,
            PageSize = pageSize,
            TotalCount = totalRowCount
        });
    }

    [HttpGet]
    public IActionResult Data(int Id)
    {
        var model = _db.mJenisBahanBakar.FirstOrDefault(x => x.Id == Id);
        if (model == null) return NotFound();
        return Json(new
        {
            data = model,
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("Data")]
    public IActionResult Post(JenisBahanBakarDbModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        model.Nama = model.Nama.Trim();

        var hasName = _db.mJenisBahanBakar.Where(x => x.Nama.ToUpper() == model.Nama.ToUpper()).Any();
        if (hasName)
        {
            ModelState.AddModelError("Name", "Name already exists");
            return BadRequest(ModelState);
        }

        _db.mJenisBahanBakar.Add(model);
        _db.SaveChanges();

        return Ok();
    }

    [HttpPut]
    [ValidateAntiForgeryToken]
    [ActionName("Data")]
    public IActionResult Put(int Id, JenisBahanBakarDbModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var data = _db.mJenisBahanBakar.FirstOrDefault(x => x.Id == Id);
        if (data == null) return NotFound();

        var hasName = _db.mJenisBahanBakar.Where(x => x.Nama.ToUpper() == model.Nama.ToUpper() && x.Id != Id).Any();
        if (hasName)
        {
            ModelState.AddModelError("Name", "Name already exists");
            return BadRequest(ModelState);
        }

        MahasConverter.Cast(model, data, "Id");

        _db.SaveChanges();

        return Ok();
    }

    [HttpDelete]
    [ValidateAntiForgeryToken]
    [ActionName("Data")]
    public IActionResult Delete(int Id)
    {
        _db.mJenisBahanBakar.Remove(_db.mJenisBahanBakar.First(x => x.Id == Id));
        _db.SaveChanges();
        return Ok();
    }
}

