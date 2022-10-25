using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace rentcar.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // roles
        var roleAdmin = new IdentityRole("Admin") { NormalizedName = "Admin" };
        builder.Entity<IdentityRole>().HasData(
            roleAdmin,
            new IdentityRole("Vendor") { NormalizedName = "Vendor" }
        );

        // user
        var hasher = new PasswordHasher<IdentityUser>();
        var user = new IdentityUser("admin")
        {
            NormalizedUserName = "admin",
            Email = "admin@rentcar.com",
            NormalizedEmail = "admin@rentcar.com",
            EmailConfirmed = true,
            SecurityStamp = string.Empty
        };
        user.PasswordHash = hasher.HashPassword(user, "bali123456");
        builder.Entity<IdentityUser>().HasData(user);

        // userrole
        builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
        {
            RoleId = roleAdmin.Id,
            UserId = user.Id
        });

        base.OnModelCreating(builder);
    }
}

