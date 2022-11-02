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
public class KendaraanController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<KendaraanController> _logger;

    public KendaraanController(ApplicationDbContext db, ILogger<KendaraanController> logger)
    {
        _logger = logger;
        _db = db;
    }

    private new IEnumerable<KendaraanViewModel> ViewData
    {
        get
        {
            return _db.mKendaraan.Select(x => new KendaraanViewModel
            {
                NoPolisi = x.NoPolisi,
                Foto = null,
                Id = x.Id,
                Id_TipeKendaraan = x.Id_TipeKendaraan,
                Id_TipeKendaraan_Text = $"{x.TipeKendaraan.MerekKendaraan.Nama} - {x.TipeKendaraan.Nama} - {x.TipeKendaraan.Transmisi}",
                Id_Vendor = x.Id_Vendor,
                Id_Vendor_Text = x.Vendor.Nama,
                NomorMesin = x.NomorMesin,
                STNKAtasNama = x.STNKAtasNama,
                TahunPembuatan = x.TahunPembuatan,
                TanggalSamsat = x.TanggalSamsat,
                TanggalSamsat5Tahun = x.TanggalSamsat5Tahun,
                Warna = x.Warna,
                Jenis = x.TipeKendaraan.Jenis,
                HasFoto = x.Foto != null
            });
        }
    }

    private void ViewToDbModel(KendaraanDbModel dbModel, KendaraanPostPutViewModel viewModel)
    {
        dbModel.Id_TipeKendaraan = viewModel.Id_TipeKendaraan;
        dbModel.Id_Vendor = viewModel.Id_Vendor;
        dbModel.TahunPembuatan = viewModel.TahunPembuatan;
        dbModel.TanggalSamsat = viewModel.TanggalSamsat;
        dbModel.TanggalSamsat5Tahun = viewModel.TanggalSamsat5Tahun;
        dbModel.Warna = viewModel.Warna?.Trim()?.ToUpper();
        dbModel.NoPolisi = viewModel.NoPolisi.Trim().ToUpper();
        dbModel.NomorMesin = viewModel.NomorMesin?.Trim()?.ToUpper();
        dbModel.STNKAtasNama = viewModel.STNKAtasNama?.Trim()?.ToUpper();
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Datas(int pageSize, int pageIndex, int orderBy, OrderByTypeEnum orderByType, List<FilterModel> filter)
    {
        IEnumerable<KendaraanViewModel> query = ViewData;

        foreach (var _filter in filter)
        {
            switch (_filter.Key)
            {
                case "Filter":
                    if (string.IsNullOrEmpty(_filter.Value)) break;
                    _filter.Value = _filter.Value.ToUpper();
                    query = query.Where(x =>
                        (x.NoPolisi ?? "").ToUpper().Contains(_filter.Value) ||
                        (x.NomorMesin ?? "").ToUpper().Contains(_filter.Value) ||
                        (x.STNKAtasNama ?? "").ToUpper().Contains(_filter.Value) ||
                        (x.Warna ?? "").ToUpper().Contains(_filter.Value)
                    );
                    break;
                case "Vendor":
                    if (string.IsNullOrEmpty(_filter.Value)) break;
                    var idVendor = int.Parse(_filter.Value);
                    query = query.Where(x => x.Id_Vendor == idVendor);
                    break;
                case "Jenis":
                    if (string.IsNullOrEmpty(_filter.Value)) break;
                    query = query.Where(x => x.Jenis == _filter.Value);
                    break;
                case "TipeKendaraan":
                    if (string.IsNullOrEmpty(_filter.Value)) break;
                    var idTipeKendaraan = int.Parse(_filter.Value);
                    query = query.Where(x => x.Id_TipeKendaraan == idTipeKendaraan);
                    break;
            }
        }

        var totalRowCount = query.Count();
        Func<KendaraanViewModel, dynamic> _orderBy = (x =>
            orderBy == 0 ? x.Id :
            orderBy == 1 ? x.NoPolisi :
            orderBy == 2 ? x.Id_TipeKendaraan_Text :
            orderBy == 3 ? x.Warna! :
            orderBy == 4 ? x.TanggalSamsat! :
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
    public IActionResult Post(KendaraanPostPutViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var dbModel = new KendaraanDbModel();
        ViewToDbModel(dbModel, model);

        _db.mKendaraan.Add(dbModel);
        _db.SaveChanges();

        return Ok();
    }

    [HttpPut]
    [ValidateAntiForgeryToken]
    [ActionName("Data")]
    public IActionResult Put(int Id, KendaraanPostPutViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var dbModel = _db.mKendaraan.FirstOrDefault(x => x.Id == Id);
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
        _db.mKendaraan.Remove(_db.mKendaraan.First(x => x.Id == Id));
        _db.SaveChanges();
        return Ok();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Select2_Vendor(int pageSize, int pageIndex, string filter)
    {
        IEnumerable<VendorDbModel> query = _db.mVendor;

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
    public IActionResult Select2_TipeKendaraan(int pageSize, int pageIndex, string filter, string jenis)
    {
        IEnumerable<MahasSelectListItem> query = _db.mTipeKendaraan.Where(x => x.Jenis == jenis).Select(x => new MahasSelectListItem()
        {
            Value = x.Id,
            Text = $"{x.MerekKendaraan.Nama} - {x.Nama} - {x.Transmisi}"
        });

        if (!String.IsNullOrEmpty(filter))
        {
            filter = filter.ToUpper();
            query = query.Where(x =>
                (x.Text ?? "").ToUpper().Contains(filter)
            );
        }

        var totalRowCount = query.Count();
        query = query.OrderBy(x => x.Text);
        var datas = query.Skip(pageSize * pageIndex).Take(pageSize).ToList();

        return Json(new
        {
            Datas = datas.ToList(),
            PageIndex = pageIndex,
            PageSize = pageSize,
            TotalCount = totalRowCount
        });
    }

    [HttpPost]
    public IActionResult UploadMedia(int id, string name, IFormFile file)
    {
        var data = _db.mKendaraan.FirstOrDefault(x => x.Id == id);
        if (data == null) return NotFound();
        using var ms = new MemoryStream();
        file.CopyTo(ms);
        data.Foto = ms.ToArray();
        data.FotoFileName = name;
        _db.SaveChanges();
        return Ok();
    }

    [HttpGet]
    public IActionResult Media(int Id)
    {
        var data = _db.mKendaraan.FirstOrDefault(x => x.Id == Id);
        if (data?.Foto == null) return NotFound();
        return File(data.Foto, Mahas.Helpers.MahasConverter.GetMimeType(data.FotoFileName!));
    }
}

