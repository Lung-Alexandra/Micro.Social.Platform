using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MicroSocialPlatform.Models;

namespace MicroSocialPlatform.Data;

public class ApplicationDbContext : IdentityDbContext<AppUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Post>().HasOne(p => p.User).WithMany(u => u.UserPosts);
        builder.Entity<Profile>().HasOne(p => p.User).WithOne(u => u.UserProfile).HasForeignKey<Profile>(p => p.UserId);
        builder.Entity<Comment>().HasOne(c => c.Post).WithMany(p => p.Comments);
        builder.Entity<Comment>().HasOne(c => c.User).WithMany(p => p.UserComments);
        base.OnModelCreating(builder);
    }

    public DbSet<Profile> Profiles { get; set; }
    public DbSet<Post> Posts { get; set; }

    public DbSet<Comment> Comments { get; set; }
}