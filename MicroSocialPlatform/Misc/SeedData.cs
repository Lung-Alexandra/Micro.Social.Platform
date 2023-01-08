using MicroSocialPlatform.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MicroSocialPlatform.Models;

namespace MicroSocialPlatform.Misc;

// This class adds the default users/roles to the database.
public static class SeedData
{
    // Generate the roles.
    public static void Initialize(IServiceProvider provider)
    {
        using (var context =
               new ApplicationDbContext(
                   provider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()
               ))
        {
            // If there are roles, do nothing.
            if (context.Roles.Any()) return;

            context.Roles.AddRange(
                new IdentityRole
                {
                    Id = "3dd2a19c-e100-4644-afd0-b5bc72f11120",
                    Name = "User",
                    NormalizedName = "USER"
                },
                new IdentityRole
                {
                    Id = "3dd2a19c-e100-4644-afd0-b5bc72f11121",
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                }
            );
            
            // Generate fake users for testing.
            var hasher = new PasswordHasher<AppUser>();
            for (int i = 0; i < 200; i++)
            {
                AppUser user = new AppUser()
                {
                    Id = "FakeUser:" + i.ToString(),
                    UserName = "FakeUser" + i.ToString(),
                    NormalizedEmail = "USER" + i.ToString() + "@YAHOO.COM",
                    Email = "user" + i.ToString() + "@yahoo.com",
                    NormalizedUserName = "USER" + i.ToString(),
                    PasswordHash = hasher.HashPassword(null, "lol")
                };
                context.Users.Add(user);
                var roleuser = new IdentityUserRole<string>
                {
                    RoleId = "3dd2a19c-e100-4644-afd0-b5bc72f11120",
                    UserId = user.Id
                };
                context.UserRoles.Add(roleuser);
                Profile profile = new Profile
                {
                    UserId = user.Id,
                    Gender = Gender.Female,
                    Visibility = Visibility.Private
                };
                context.Profiles.Add(profile);
            }
            //Generate admin
            AppUser admin = new AppUser()
            {
                Id = "8eadd5eb-92a4-49a9-85d4-4c29dd7f86b0",
                UserName = "Admin",
                NormalizedEmail = "ADMIN90@YAHOO.COM",
                Email="admin90@yahoo.com",
                NormalizedUserName = "ADMIN",
                PasswordHash = hasher.HashPassword(null, "!admin90")
            };
            context.Users.Add(admin);
            var roleadmin = new IdentityUserRole<string>
            {
                RoleId = "3dd2a19c-e100-4644-afd0-b5bc72f11121",
                UserId = admin.Id
            };
            context.UserRoles.Add(roleadmin);
            Profile adminprofile = new Profile
            {
                UserId = admin.Id,
                Gender = Gender.Unspecified,
                Visibility = Visibility.Private
            };
            context.Profiles.Add(adminprofile);

            context.SaveChanges();
        }
    }
}