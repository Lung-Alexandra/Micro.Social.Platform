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

        builder.Entity<Friendship>().HasOne(f => f.User1).WithMany(u => u.UserSentFriendships);
        builder.Entity<Friendship>().HasOne(f => f.User2).WithMany(u => u.UserReceivedFriendships);

        builder.Entity<GroupMembership>().HasOne(m => m.Group).WithMany(g => g.Memberships);
        builder.Entity<GroupMembership>().HasOne(m => m.User).WithMany(u => u.UserMemberships);

        base.OnModelCreating(builder);
    }

    public DbSet<Profile> Profiles { get; set; }
    public DbSet<Post> Posts { get; set; }

    public DbSet<Comment> Comments { get; set; }

    public DbSet<Friendship> Friendships { get; set; }

    public DbSet<Group> Groups { get; set; }

    public DbSet<GroupMembership> GroupMemberships { get; set; }
}