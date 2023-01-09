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

app.MapControllerRoute(
    name: "new_post",
    pattern: "/post/new",
    defaults:
    new { controller = "Post", action = "New" }
);
app.MapControllerRoute(
    name: "post",
    pattern: "/post/{id}",
    defaults:
    new { controller = "Post", action = "Index" }
);

app.MapControllerRoute(
    name: "edit_post",
    pattern: "/post/edit/{id}",
    defaults:
    new { controller = "Post", action = "Edit" }
);

app.MapControllerRoute(
    name: "delete_post",
    pattern: "/post/delete/{id}",
    new { controller = "Post", action = "Delete" }
);

app.MapControllerRoute(
    name: "edit_comment",
    pattern: "/comment/edit/{id}",
    defaults:
    new { controller = "Comment", action = "Edit" }
);

app.MapControllerRoute(
    name: "delete_comment",
    pattern: "/comment/delete/{id}",
    defaults:
    new { controller = "Comment", action = "Delete" }
);

app.MapControllerRoute(
    name: "new_friendship",
    pattern: "/friendship/new/{id}",
    defaults:
    new { controller = "Friendship", action = "New" }
);

app.MapControllerRoute(
    name: "accept_friendship",
    pattern: "/friendship/accept/{id}",
    defaults:
    new { controller = "Friendship", action = "Accept" }
);

app.MapControllerRoute(
    name: "delete_friendship",
    pattern: "/friendship/delete/{id}",
    defaults:
    new { controller = "Friendship", action = "Delete" }
);

app.MapControllerRoute(
    name: "Friends",
    pattern: "/friends/",
    defaults:
    new { controller = "Friend", action = "Index" }
);

app.MapControllerRoute(
    name: "people",
    pattern: "/people/",
    defaults:
    new { controller = "People", action = "Index" }
);

app.MapControllerRoute(
    name: "all_groups",
    pattern: "/groups",
    defaults:
    new { controller = "Group", action = "All" }
);

app.MapControllerRoute(
    name: "new_group",
    pattern: "/group/new",
    defaults:
    new { controller = "Group", action = "New" }
);

app.MapControllerRoute(
    name: "edit_group",
    pattern: "/group/edit/{id}",
    defaults:
    new { controller = "Group", action = "Edit" }
);

app.MapControllerRoute(
    name: "delete_group",
    pattern: "/group/delete/{id}",
    defaults:
    new { controller = "Group", action = "Delete" }
);

app.MapControllerRoute(
    name: "group",
    pattern: "/group/{id}",
    defaults:
    new { controller = "Group", action = "Index" }
);

app.MapControllerRoute(
    name: "new_group_membership",
    pattern: "/group_membership/new/{id}",
    defaults:
    new { controller = "GroupMembership", action = "New" }
);

app.MapControllerRoute(
    name: "accept_group_membership",
    pattern: "/group_membership/accept/{id}",
    defaults:
    new { controller = "GroupMembership", action = "Accept" }
);

app.MapControllerRoute(
    name: "delete_group_membership",
    pattern: "/group_membership/delete/{id}",
    defaults:
    new { controller = "GroupMembership", action = "Delete" }
);

app.MapControllerRoute(
    name: "edit_message",
    pattern: "/message/edit/{id}",
    defaults: new { controller = "Message", action = "Edit" }
);

app.MapControllerRoute(
    name: "delete_message",
    pattern: "/message/delete/{id}",
    defaults: new { controller = "Message", action = "Delete" }
);

app.MapRazorPages();

app.Run();