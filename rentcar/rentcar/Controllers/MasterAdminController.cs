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
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace rentcar.Controllers;

[Authorize(Roles = "Admin")]
public class MasterAdminController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<MasterAdminController> _logger;
    private readonly IUserStore<IdentityUser> _userStore;
    private readonly IUserEmailStore<IdentityUser> _emailStore;
    private readonly UserManager<IdentityUser> _userManager;

    private string roleName = "Admin";

    public MasterAdminController(
        ApplicationDbContext db,
        ILogger<MasterAdminController> logger,
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

    public IActionResult Index()
    {
        return View();
    }

    private new IEnumerable<MasterAdminViewModel> ViewData { get
        {
            return _db.mAdmin.Select(x => new MasterAdminViewModel
            {
                Id = x.Id,
                Nama = x.Nama,
                Telp = x.User!.PhoneNumber,
                Email = x.User!.Email,
                UserName = x.User!.UserName
            });
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Datas(int pageSize, int pageIndex, int orderBy, OrderByTypeEnum orderByType, List<FilterModel> filter)
    {
        IEnumerable<MasterAdminViewModel> query = ViewData;

        foreach (var _filter in filter)
        {
            switch (_filter.Key)
            {
                case "Filter":
                    if (string.IsNullOrEmpty(_filter.Value)) break;
                    _filter.Value = _filter.Value.ToUpper();
                    query = query.Where(x =>
                        (x.Nama ?? "").ToUpper().Contains(_filter.Value) ||
                        (x.UserName ?? "").ToUpper().Contains(_filter.Value) ||
                        (x.Email ?? "").ToUpper().Contains(_filter.Value) ||
                        (x.Telp ?? "").ToUpper().Contains(_filter.Value)
                    );
                    break;
            }
        }

        var totalRowCount = query.Count();
        Func<MasterAdminViewModel, dynamic> _orderBy = (x =>
            orderBy == 0 ? x.Id :
            orderBy == 1 ? x.Nama :
            orderBy == 2 ? x.UserName :
            orderBy == 3 ? x.Telp! :
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
    public async Task<IActionResult> Post(MasterAdminPostViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        model.Nama = model.Nama.Trim();
        var hasName = _db.mAdmin.Where(x => x.Nama.ToUpper() == model.Nama.ToUpper()).Any();
        if (hasName)
        {
            ModelState.AddModelError("Name", "Name already exists");
            return BadRequest(ModelState);
        }

        var role = _db.Roles.FirstOrDefault(x => x.Name.ToUpper() == roleName.ToUpper());
        if (role == null)
        {
            ModelState.AddModelError("Name", "Admin role is not exsit");
            return BadRequest(ModelState);
        }

        var user = CreateUser();
        user.PhoneNumber = model.Telp;
        await _userStore.SetUserNameAsync(user, model.UserName, CancellationToken.None);
        await _emailStore.SetEmailAsync(user, model.Email, CancellationToken.None);
        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            _db.Database.BeginTransaction();

            _db.UserRoles.Add(new IdentityUserRole<string>()
            {
                UserId = user.Id,
                RoleId = role.Id
            });

            _db.mAdmin.Add(new AdminDbModel()
            {
                Nama = model.Nama,
                Id_User = user.Id,
            });
            _db.SaveChanges();

            _db.Database.CommitTransaction();
        }
        else
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return BadRequest(ModelState);
        }

        return Ok();
    }

    [HttpPut]
    [ValidateAntiForgeryToken]
    [ActionName("Data")]
    public IActionResult Put(int Id, MasterAdminPutViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        model.Nama = model.Nama.Trim();
        var hasName = _db.mAdmin.Where(x => x.Nama.ToUpper() == model.Nama.ToUpper() && x.Id != Id).Any();
        if (hasName)
        {
            ModelState.AddModelError("Name", "Name already exists");
            return BadRequest(ModelState);
        }

        var data = _db.mAdmin.FirstOrDefault(x => x.Id == Id);
        if (data == null) return NotFound();

        var _user = _db.Users.FirstOrDefault(x => x.Id == data.Id_User);
        if (_user == null) return NotFound();

        _db.Database.BeginTransaction();

        data.Nama = model.Nama;
        _user.PhoneNumber = model.Telp;
        _user.UserName = model.UserName;
        _user.Email = model.Email;

        _db.SaveChanges();
        _db.Database.CommitTransaction();

        return Ok();
    }

    [HttpDelete]
    [ValidateAntiForgeryToken]
    [ActionName("Data")]
    public IActionResult Delete(int Id)
    {
        var data = _db.mAdmin.FirstOrDefault(x => x.Id == Id);
        if (data == null) return NotFound();

        var user = _db.Users.FirstOrDefault(x => x.Id == data.Id_User);
        var role = _db.Roles.FirstOrDefault(x => x.Name.ToUpper() == "");

        _db.Database.BeginTransaction();

        _db.mAdmin.Remove(data);
        if (role != null && user != null) {
            _db.UserRoles.Remove(_db.UserRoles.First(x => x.UserId == user.Id && x.RoleId == roleName.ToUpper()));
        };
        if (user != null) {
            _db.Users.Remove(user);
        }

        _db.SaveChanges();
        _db.Database.CommitTransaction();
        return Ok();
    }
}

