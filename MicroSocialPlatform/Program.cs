using System.Runtime.InteropServices;
using MicroSocialPlatform.Data;
using MicroSocialPlatform.Misc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MicroSocialPlatform.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Check if the platform is linux.
if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
{
    //Use mysql server on this platform.
    var server_version = ServerVersion.AutoDetect(connectionString);
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseMySql(connectionString, server_version));
}
else
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));
}

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<AppUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.User.RequireUniqueEmail = true;
    })
    .AddSignInManager<MySignInManager>()
    .AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    SeedData.Initialize(services);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "start",
    pattern: "/",
    defaults:
    new { controller = "Start", action = "Index" }
);

app.MapControllerRoute(
    name: "home",
    pattern: "/home",
    defaults:
    new { controller = "Home", action = "Index" }
);

app.MapControllerRoute(
    name: "profile",
    pattern: "/profile/{id}",
    defaults:
    new { controller = "Profile", action = "Index" }
);
app.MapControllerRoute(
    name: "edit_profile",
    pattern: "/profile/edit/{id}",
    defaults:
    new { controller = "Profile", action = "Edit" }
);
app.MapRazorPages();

app.Run();