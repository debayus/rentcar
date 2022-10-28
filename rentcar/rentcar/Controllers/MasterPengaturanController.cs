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
using rentcar.Models.View;
using System.Reflection.PortableExecutable;

namespace rentcar.Controllers;

[Authorize(Roles = "Admin")]
public class MasterPengaturanController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<MasterPengaturanController> _logger;

    public MasterPengaturanController(ApplicationDbContext db, ILogger<MasterPengaturanController> logger)
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
        IEnumerable<KonfigurasiDbModel> query = _db.mKonfigurasi;

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
        Func<KonfigurasiDbModel, dynamic> _orderBy = (x =>
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
    public IActionResult Data()
    {
        var models = _db.mKonfigurasi.ToList();
        var dp = models.FirstOrDefault(x => x.Nama == "DP")?.Value;
        var bank2Tampilkan = models.FirstOrDefault(x => x.Nama == "Bank2Tampilkan")?.Value;
        return Json(new
        {
            data = new MasterPengaturanViewModel()
            {
                Alamat = models.FirstOrDefault(x => x.Nama == "Alamat")?.Value,
                Bank1 = models.FirstOrDefault(x => x.Nama == "Bank1")?.Value,
                Bank2 = models.FirstOrDefault(x => x.Nama == "Bank2")?.Value,
                Perusahaan = models.FirstOrDefault(x => x.Nama == "Perusahaan")?.Value,
                Telp = models.FirstOrDefault(x => x.Nama == "Telp")?.Value,
                Website = models.FirstOrDefault(x => x.Nama == "Website")?.Value,
                DP = string.IsNullOrEmpty(dp) ? null : double.Parse(dp),
                Bank2Tampilkan = string.IsNullOrEmpty(bank2Tampilkan) ? null : bool.Parse(bank2Tampilkan),
            },
        });
    }

    [HttpPut]
    [ValidateAntiForgeryToken]
    [ActionName("Data")]
    public IActionResult Put(MasterPengaturanViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var models = _db.mKonfigurasi.ToList();

        var properties = model.GetType().GetProperties();
        for (int i = 0; i < properties.Length; i++)
        {
            var objValue = properties[i].GetValue(model);
            var value = objValue == null ? null : objValue.GetType() == typeof(string) ? (string)objValue : objValue.ToString();
            var text = properties[i].Name;

            var dbModel = models.FirstOrDefault(x => x.Nama == text);
            if (dbModel == null)
            {
                _db.mKonfigurasi.Add(new KonfigurasiDbModel()
                {
                    Nama = text,
                    Value = value
                });
            }
            else
            {
                dbModel.Value = value;
            }
        }
        _db.SaveChanges();

        return Ok();
    }
}

