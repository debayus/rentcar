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

namespace rentcar.Controllers;

[Authorize(Roles = "Admin")]
public class MasterTipeKendaraanController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<MasterTipeKendaraanController> _logger;

    public MasterTipeKendaraanController(ApplicationDbContext db, ILogger<MasterTipeKendaraanController> logger)
    {
        _logger = logger;
        _db = db;
    }

    public IActionResult Index()
    {
        return View();
    }

    private new IEnumerable<MasterTipeKendaraanViewModel> ViewData
    {
        get
        {
            return _db.mTipeKendaraan.Select(x => new MasterTipeKendaraanViewModel
            {
                Id = x.Id,
                Nama = x.Nama,
                Harga = x.Harga,
                Jenis = x.Jenis,
                Id_MerekKendaraan = x.Id_MerekKendaraan,
                Id_JenisBahanBakar = x.Id_JenisBahanBakar,
                Transmisi = x.Transmisi,
                Id_JenisBahanBakar_Text = x.JenisBahanBakar == null ? null : x.JenisBahanBakar.Nama,
                Id_MerekKendaraan_Text = x.MerekKendaraan.Nama
            });
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Datas(int pageSize, int pageIndex, int orderBy, OrderByTypeEnum orderByType, List<FilterModel> filter)
    {
        IEnumerable<MasterTipeKendaraanViewModel> query = ViewData;

        foreach (var _filter in filter)
        {
            switch (_filter.Key)
            {
                case "Jenis":
                    if (string.IsNullOrEmpty(_filter.Value)) break;
                    query = query.Where(x => x.Jenis == _filter.Value);
                    break;
                case "Merek":
                    if (string.IsNullOrEmpty(_filter.Value)) break;
                    var idMerekKendaraan = int.Parse(_filter.Value);
                    query = query.Where(x => x.Id_MerekKendaraan == idMerekKendaraan);
                    break;
                case "Filter":
                    if (string.IsNullOrEmpty(_filter.Value)) break;
                    _filter.Value = _filter.Value.ToUpper();
                    query = query.Where(x => (x.Nama ?? "").ToUpper().Contains(_filter.Value));
                    break;
            }
        }

        var totalRowCount = query.Count();
        Func<MasterTipeKendaraanViewModel, dynamic> _orderBy = (x =>
            orderBy == 0 ? x.Id :
            orderBy == 1 ? x.Id_MerekKendaraan_Text :
            orderBy == 2 ? x.Nama :
            orderBy == 3 ? x.Transmisi! :
            orderBy == 4 ? x.Harga :
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
        var model = ViewData.FirstOrDefault(x => x.Id == Id);
        if (model == null) return NotFound();
        return Json(new
        {
            data = model,
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("Data")]
    public IActionResult Post(MasterTipeKendaraanPostPutViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        model.Nama = model.Nama.Trim();

        var hasName = _db.mTipeKendaraan.Where(x => x.Nama.ToUpper() == model.Nama.ToUpper() && x.Id_MerekKendaraan == model.Id_MerekKendaraan).Any();
        if (hasName)
        {
            ModelState.AddModelError("Name", "Name already exists");
            return BadRequest(ModelState);
        }

        _db.mTipeKendaraan.Add(new TipeKendaraanDbModel()
        {
            Harga = model.Harga,
            Id_JenisBahanBakar = model.Id_JenisBahanBakar,
            Id_MerekKendaraan = model.Id_MerekKendaraan,
            Jenis = model.Jenis,
            Nama = model.Nama,
            Transmisi = model.Transmisi
        });
        _db.SaveChanges();

        return Ok();
    }

    [HttpPut]
    [ValidateAntiForgeryToken]
    [ActionName("Data")]
    public IActionResult Put(int Id, MasterTipeKendaraanPostPutViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var data = _db.mTipeKendaraan.FirstOrDefault(x => x.Id == Id);
        if (data == null) return NotFound();

        var hasName = _db.mTipeKendaraan.Where(x => x.Nama.ToUpper() == model.Nama.ToUpper() && x.Id != Id && x.Id_MerekKendaraan == model.Id_MerekKendaraan).Any();
        if (hasName)
        {
            ModelState.AddModelError("Name", "Name already exists");
            return BadRequest(ModelState);
        }

        data.Harga = model.Harga;
        data.Id_JenisBahanBakar = model.Id_JenisBahanBakar;
        data.Id_MerekKendaraan = model.Id_MerekKendaraan;
        data.Jenis = model.Jenis;
        data.Nama = model.Nama;
        data.Transmisi = model.Transmisi;

        _db.SaveChanges();

        return Ok();
    }

    [HttpDelete]
    [ValidateAntiForgeryToken]
    [ActionName("Data")]
    public IActionResult Delete(int Id)
    {
        _db.mTipeKendaraan.Remove(_db.mTipeKendaraan.First(x => x.Id == Id));
        _db.SaveChanges();
        return Ok();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Select2_MerekKendaraan(int pageSize, int pageIndex, string filter)
    {
        IEnumerable<MerekKendaraanDbModel> query = _db.mMerekKendaraan;

        if (!String.IsNullOrEmpty(filter))
        {
            filter = filter.ToUpper();
            query = query.Where(x =>
                (x.Nama ?? "").ToUpper().Contains(filter)
            );
        }

        var totalRowCount = query.Count();
        query = query.OrderBy(x => x.Nama);
        var datas = query.Skip(pageSize * pageIndex).Take(pageSize).ToList();

        return Json(new
        {
            Datas = datas.Select(x => new MahasSelectListItem(x.Nama, x.Id)).ToList(),
            PageIndex = pageIndex,
            PageSize = pageSize,
            TotalCount = totalRowCount
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Select2_JenisBahanBakar(int pageSize, int pageIndex, string filter)
    {
        IEnumerable<JenisBahanBakarDbModel> query = _db.mJenisBahanBakar;

        if (!String.IsNullOrEmpty(filter))
        {
            filter = filter.ToUpper();
            query = query.Where(x =>
                (x.Nama ?? "").ToUpper().Contains(filter)
            );
        }

        var totalRowCount = query.Count();
        query = query.OrderBy(x => x.Nama);
        var datas = query.Skip(pageSize * pageIndex).Take(pageSize).ToList();

        return Json(new
        {
            Datas = datas.Select(x => new MahasSelectListItem(x.Nama, x.Id)).ToList(),
            PageIndex = pageIndex,
            PageSize = pageSize,
            TotalCount = totalRowCount
        });
    }
}

