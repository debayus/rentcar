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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.EntityFrameworkCore.Metadata;

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

    private new IEnumerable<KondisiKendaraanViewModel> ViewData
    {
        get
        {
            return _db.trKondisiKendaraan.Select(x => new KondisiKendaraanViewModel
            {
                AdminNama = x.Admin.Nama,
                Bensin = x.Bensin,
                Catatan = x.Catatan,
                Id = x.Id,
                Id_Admin = x.Id_Admin,
                Id_Kendaraan = x.Id_Kendaraan,
                Id_Sewa = x.Id_Sewa,
                Kelengkapan = x.Kelengkapan,
                KendaraanNoPolisi = x.Kendaraan.NoPolisi,
                KendaraanTipeKendaraan = $"{x.Kendaraan.TipeKendaraan.Nama} - {x.Kendaraan.TipeKendaraan.MerekKendaraan.Nama} - {x.Kendaraan.TipeKendaraan.Transmisi}",
                KendaraanWarna = x.Kendaraan.Warna,
                Kilometer = x.Kilometer,
                Jenis = x.Kendaraan.TipeKendaraan.Jenis,
            });
        }
    }

    private void ViewToDbModel(KondisiKendaraanDbModel dbModel, KondisiKendaraanPostPutViewModel viewModel)
    {
        dbModel.Bensin = viewModel.Bensin;
        dbModel.Catatan = viewModel.Catatan;
        dbModel.Id_Admin = viewModel.Id_Admin;
        dbModel.Id_Kendaraan = viewModel.Id_Kendaraan;
        dbModel.Id_Sewa = viewModel.Id_Sewa;
        dbModel.Kelengkapan = viewModel.Kelengkapan;
        dbModel.Kilometer = viewModel.Kilometer;
        dbModel.Tanggal = viewModel.Tanggal;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Datas(int pageSize, int pageIndex, int orderBy, OrderByTypeEnum orderByType, List<FilterModel> filter)
    {
        IEnumerable<KondisiKendaraanViewModel> query = ViewData;

        foreach (var _filter in filter)
        {
            switch (_filter.Key)
            {
                case "Filter":
                    if (string.IsNullOrEmpty(_filter.Value)) break;
                    _filter.Value = _filter.Value.ToUpper();
                    query = query.Where(x =>
                        (x.AdminNama ?? "").ToUpper().Contains(_filter.Value) ||
                        (x.Catatan ?? "").ToUpper().Contains(_filter.Value) ||
                        (x.Kelengkapan ?? "").ToUpper().Contains(_filter.Value) ||
                        (x.KendaraanWarna ?? "").ToUpper().Contains(_filter.Value) ||
                        (x.KendaraanTipeKendaraan ?? "").ToUpper().Contains(_filter.Value) ||
                        (x.KendaraanNoPolisi ?? "").ToUpper().Contains(_filter.Value)
                    );
                    break;
                case "Kendaraan":
                    if (string.IsNullOrEmpty(_filter.Value)) break;
                    var idKendaraan = int.Parse(_filter.Value);
                    query = query.Where(x => x.Id_Kendaraan == idKendaraan);
                    break;
                case "Jenis":
                    if (string.IsNullOrEmpty(_filter.Value)) break;
                    query = query.Where(x => x.Jenis == _filter.Value);
                    break;
                case "Sewa":
                    if (string.IsNullOrEmpty(_filter.Value)) break;
                    var idSewa = int.Parse(_filter.Value);
                    query = query.Where(x => x.Id_Kendaraan == idSewa);
                    break;
            }
        }

        var totalRowCount = query.Count();
        Func<KondisiKendaraanViewModel, dynamic> _orderBy = (x =>
            orderBy == 0 ? x.Id :
            orderBy == 1 ? x.Tanggal :
            orderBy == 2 ? x.KendaraanNoPolisi :
            orderBy == 3 ? x.KendaraanTipeKendaraan :
            orderBy == 4 ? x.KendaraanWarna! :
            orderBy == 5 ? x.Kilometer! :
            orderBy == 6 ? x.AdminNama :
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
    public IActionResult Post(KondisiKendaraanPostPutViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var dbModel = new KondisiKendaraanDbModel();
        ViewToDbModel(dbModel, model);

        _db.trKondisiKendaraan.Add(dbModel);
        _db.SaveChanges();

        return Ok();
    }

    [HttpPut]
    [ValidateAntiForgeryToken]
    [ActionName("Data")]
    public IActionResult Put(int Id, KondisiKendaraanPostPutViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var dbModel = _db.trKondisiKendaraan.FirstOrDefault(x => x.Id == Id);
        if (dbModel == null) return NotFound();

        ViewToDbModel(dbModel, model);

        _db.SaveChanges();
        return Ok();
    }

    [HttpDelete]
    [ValidateAntiForgeryToken]
    [ActionName("Data")]
    public IActionResult Delete(int Id)
    {
        _db.trKondisiKendaraan.Remove(_db.trKondisiKendaraan.First(x => x.Id == Id));
        _db.SaveChanges();
        return Ok();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Select2_Kendaraan(int pageSize, int pageIndex, string filter, string jenis)
    {
        IEnumerable<KendaraanDbModel> query = _db.mKendaraan.Where(x => x.TipeKendaraan.Jenis == jenis);

        if (!String.IsNullOrEmpty(filter))
        {
            filter = filter.ToUpper();
            query = query.Where(x =>
                (x.NoPolisi ?? "").ToUpper().Contains(filter)
            );
        }

        var totalRowCount = query.Count();
        query = query.OrderBy(x => x.NoPolisi);
        var datas = query.Skip(pageSize * pageIndex).Take(pageSize).ToList();

        return Json(new
        {
            Datas = datas.Select(x => new MahasSelectListItem(x.NoPolisi, x.Id)).ToList(),
            PageIndex = pageIndex,
            PageSize = pageSize,
            TotalCount = totalRowCount
        });
    }

    [HttpPost]
    public IActionResult UploadMedia(int id, string name, IFormFile file)
    {
        var data = _db.trKondisiKendaraan.FirstOrDefault(x => x.Id == id);
        if (data == null) return NotFound();
        using var ms = new MemoryStream();
        file.CopyTo(ms);
        _db.trKondisiKendaraanFoto.Add(new KondisiKendaraanFotoDbModel()
        {
            Id_KondisiKendaraan = id,
            Foto = ms.ToArray(),
            FotoFileName = name,
        });
        _db.SaveChanges();
        return Ok();
    }

    [HttpPost]
    public IActionResult HapusMedia(int id)
    {
        var data = _db.trKondisiKendaraanFoto.FirstOrDefault(x => x.Id == id);
        if (data == null) return NotFound();
        _db.trKondisiKendaraanFoto.Remove(data);
        _db.SaveChanges();
        return Ok();
    }

    [HttpGet]
    public IActionResult Media(int Id)
    {
        var data = _db.trKondisiKendaraanFoto.FirstOrDefault(x => x.Id == Id);
        if (data?.Foto == null) return NotFound();
        return File(data.Foto, Mahas.Helpers.MahasConverter.GetMimeType(data.FotoFileName!));
    }
}

