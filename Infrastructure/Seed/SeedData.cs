using Infrastructure.Constants;
using Infrastructure.Entities;
using Infrastructure.Data; 
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Seed;

public class SeedData
{
    public static void Seed(DataContext context, UserManager<ApplicationUser> _userManager)
    {
        if (context.Roles.Any()) return;

        var roles = new List<IdentityRole<int>>()
        {
            new IdentityRole<int>()
            {
                Id = 1, NormalizedName = Role.Admin.ToUpper(),
                Name = Role.Admin
            },
            new IdentityRole<int>()
            {
                Id = 2, NormalizedName = Role.Manager.ToUpper(),
                Name = Role.Manager
            }
        };

        context.Roles.AddRangeAsync(roles);
        context.SaveChanges();
        if (context.Users.Any()) return;
        var user = new ApplicationUser()
        {
            Id = 1,
            PhoneNumber = "502050903",
            UserName = "Abdullah03",
            Email = "abullohsheralizoda@gmail.com",
            CreatedUserId = 1,
            UpdatedUserId = 1,
            IsPublish = true,
            IsDeleted = false
        };

        _userManager.CreateAsync(user, "Image123");
        context.SaveChanges();
        if (context.UserRoles.Any()) return;
        var userRole = new IdentityUserRole<int>()
        {
            RoleId = 1,
            UserId = 1,
        };
        context.UserRoles.Add(userRole);
        context.SaveChanges();
    }
}


