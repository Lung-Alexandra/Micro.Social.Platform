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

                Profile profile = new Profile
                {
                    UserId = user.Id,
                    Gender = Gender.Female,
                    Visibility = Visibility.Private
                };
                context.Profiles.Add(profile);
            }


            context.SaveChanges();
        }
    }
}