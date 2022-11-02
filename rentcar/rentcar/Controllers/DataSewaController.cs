using System;
using System.Collections.Generic;
using System.Data;
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

[Authorize(Roles = "Admin")]
public class DataSewaController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<SewaController> _logger;
    private readonly IUserStore<IdentityUser> _userStore;
    private readonly IUserEmailStore<IdentityUser> _emailStore;
    private readonly UserManager<IdentityUser> _userManager;

    public DataSewaController(
        ApplicationDbContext db,
        ILogger<SewaController> logger,
        IUserStore<IdentityUser> userStore,
        UserManager<IdentityUser> userManager
    )
    {
        _logger = logger;
        _db = db;
        _userStore = userStore;
        _userManager = userManager;
        _emailStore = GetEmailStore();
    }

    private IdentityUser CreateUser()
    {
        try
        {
            return Activator.CreateInstance<IdentityUser>();
        }
        catch
        {
            throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
        }
    }

    private IUserEmailStore<IdentityUser> GetEmailStore()
    {
        if (!_userManager.SupportsUserEmail)
        {
            throw new NotSupportedException("The default UI requires a user store with email support.");
        }
        return (IUserEmailStore<IdentityUser>)_userStore;
    }

    private new IEnumerable<DataSewaViewModel> ViewData
    {
        get
        {
            return _db.trSewa.Select(x => new DataSewaViewModel
            {
                Id = x.Id,
                Batal = x.Batal,
                Harga = x.Harga,
                Id_Admin = x.Id_Admin,
                Id_Admin_Text = x.Admin.Nama,
                Id_Customer = x.Id_Customer,
                Id_Customer_Text = x.Customer.Nama,
                Id_Kendaraan = x.Id_Kendaraan,
                TipeKendaraan = $"{x.Kendaraan.TipeKendaraan.MerekKendaraan.Nama} - {x.Kendaraan.TipeKendaraan.Nama} - {x.Kendaraan.TipeKendaraan.Transmisi}",
                LamaSewa = x.LamaSewa,
                NoBukti = x.NoBukti,
                NoPolisi = x.Kendaraan.NoPolisi,
                Tanggal = x.Tanggal,
                TanggalDiambil = x.TanggalDikembalian,
                TanggalDikembalian = x.TanggalDikembalian,
                TanggalSewa = x.TanggalSewa,
                Warna = x.Kendaraan.Warna,
            });
        }
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Datas(int pageSize, int pageIndex, int orderBy, OrderByTypeEnum orderByType, List<FilterModel> filter)
    {
        IEnumerable<DataSewaViewModel> query = ViewData;

        foreach (var _filter in filter)
        {
            switch (_filter.Key)
            {
                case "Filter":
                    if (string.IsNullOrEmpty(_filter.Value)) break;
                    _filter.Value = _filter.Value.ToUpper();
                    query = query.Where(x =>
                        (x.NoPolisi ?? "").ToUpper().Contains(_filter.Value) ||
                        (x.TipeKendaraan ?? "").ToUpper().Contains(_filter.Value) ||
                        (x.Id_Customer_Text ?? "").ToUpper().Contains(_filter.Value) ||
                        (x.Warna ?? "").ToUpper().Contains(_filter.Value)
                    );
                    break;
                case "Tanggal":
                    if (string.IsNullOrEmpty(_filter.Value)) break;
                    var tgl = DateTime.Parse(_filter.Value);
                    query = query.Where(x => x.TanggalSewa == tgl);
                    break;
            }
        }

        var totalRowCount = query.Count();
        Func<DataSewaViewModel, dynamic> _orderBy = (x =>
            orderBy == 0 ? x.Id :
            orderBy == 1 ? x.NoBukti :
            orderBy == 2 ? x.Id_Customer_Text :
            orderBy == 3 ? x.NoPolisi :
            orderBy == 4 ? x.TipeKendaraan :
            orderBy == 5 ? x.TanggalSewa :
            orderBy == 6 ? x.LamaSewa :
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
    public async Task<IActionResult> Post(DataSewaPostViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // validasi customer baru
        if (model.CustomerBaru)
        {
            bool valid = true;
            if (string.IsNullOrEmpty(model.CustomerBaruEmail))
            {
                ModelState.AddModelError("CustomerBaruEmail", "Email Customer is required");
                valid = false;
            }
            if (string.IsNullOrEmpty(model.CustomerBaruNama))
            {
                ModelState.AddModelError("CustomerBaruNama", "Nama Customer is required");
                valid = false;
            }
            if (string.IsNullOrEmpty(model.CustomerBaruTelp))
            {
                ModelState.AddModelError("CustomerBaruTelp", "Telp Customer is required");
                valid = false;
            }
            if (!valid)
            {
                return BadRequest(ModelState);
            }
        }
        else if (model.Id_Customer == null)
        {
            ModelState.AddModelError("Id_Customer", "Customer is required");
            return BadRequest(ModelState);
        }

        var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var admin = _db.mAdmin.First(x => x.Id_User == userid);
        var dp = double.Parse(_db.mKonfigurasi.First(x => x.Nama == "DP").Value ?? "0");
        var dbJenisBiaya = _db.mJenisBiaya.ToList();
        var jenisBiayaDP = dbJenisBiaya.First(x => x.Nama == "DP");
        var jenisBiayaSisaBayar = dbJenisBiaya.First(x => x.Nama == "Sisa Bayar");

        _db.Database.BeginTransaction();

        // customer
        CustomerDbModel customer;
        if (model.CustomerBaru)
        {
            var user = CreateUser();
            user.PhoneNumber = model.CustomerBaruTelp;
            await _userStore.SetUserNameAsync(user, model.CustomerBaruEmail, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, model.CustomerBaruEmail, CancellationToken.None);
            var result = await _userManager.CreateAsync(user, model.CustomerBaruEmail);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return BadRequest(ModelState);
            }

            customer = new CustomerDbModel()
            {
                Alamat = model.CustomerBaruAlamat,
                Nama = model.CustomerBaruNama!,
                Id_User = user.Id,
            };
            _db.mCustomer.Add(customer);
            _db.SaveChanges();
        }
        else
        {
            customer = _db.mCustomer.First(x => x.Id == model.Id_Customer);
        }

        // No Bukti
        var kodeNoBukti = model.TanggalSewa.ToString("yyMM");
        var lastData = _db.trSewa.Where(x => x.NoBukti.Contains(kodeNoBukti)).OrderByDescending(x => x.NoBukti).FirstOrDefault();
        var no = lastData == null ? 0 : int.Parse(lastData.NoBukti.Replace(kodeNoBukti, ""));
        no++;
        var newNoBukti = $"{kodeNoBukti}{no:C3}";

        // header
        var dbModel = new SewaDbModel()
        {
            Batal = false,
            KondisiKendaraan = null,
            SewaBiaya = null,
            SewaPerjanjian = null,
            TanggalDikembalian = null,
            Tanggal = DateTime.Now,
            Id_Admin = admin.Id,
            Harga = model.Harga,
            Id_Customer = customer.Id,
            Id_Kendaraan = model.Id_Kendaraan,
            LamaSewa = model.LamaSewa,
            NoBukti = newNoBukti,
            TanggalSewa = model.TanggalSewa,
        };

        _db.trSewa.Add(dbModel);
        _db.SaveChanges();

        // biaya
        var nilaiDp = model.Harga / 100 * (decimal)dp;
        var sisalBayar = model.Harga - nilaiDp;
        _db.trSewaBiaya.Add(new SewaBiayaDbModel()
        {
            Biaya = nilaiDp,
            Lunas = false,
            Catatan = "DP",
            FotoBukti = null,
            Id_JenisBiaya = jenisBiayaDP.Id,
            Id_Sewa = dbModel.Id,
        });
        _db.trSewaBiaya.Add(new SewaBiayaDbModel()
        {
            Biaya = sisalBayar,
            Lunas = false,
            Catatan = "Sisa Bayar",
            FotoBukti = null,
            Id_JenisBiaya = jenisBiayaSisaBayar.Id,
            Id_Sewa = dbModel.Id,
        });

        return Ok();
    }

    [HttpPut]
    [ValidateAntiForgeryToken]
    [ActionName("Data")]
    public IActionResult Put(int Id, DataSewaPutViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var dbModel = _db.trSewa.FirstOrDefault(x => x.Id == Id);
        if (dbModel == null) return NotFound();

        dbModel.Id_Kendaraan = model.Id_Kendaraan;
        dbModel.Id_Customer = model.Id_Customer;
        dbModel.TanggalSewa = model.TanggalSewa;
        dbModel.LamaSewa = model.LamaSewa;
        dbModel.Harga = model.Harga;

        _db.SaveChanges();
        return Ok();
    }

    [HttpDelete]
    [ValidateAntiForgeryToken]
    [ActionName("Data")]
    public IActionResult Delete(int Id)
    {
        _db.trSewaBiaya.RemoveRange(_db.trSewaBiaya.Where(x => x.Id_Sewa == Id));
        _db.trSewaPerjanjian.RemoveRange(_db.trSewaPerjanjian.Where(x => x.Id == Id));
        _db.trSewa.Remove(_db.trSewa.First(x => x.Id == Id));
        _db.SaveChanges();
        return Ok();
    }
}