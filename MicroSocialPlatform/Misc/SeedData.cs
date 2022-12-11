using MicroSocialPlatform.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
            context.SaveChanges();
        }
    }
}