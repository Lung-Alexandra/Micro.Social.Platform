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
        builder.Entity<Post>().HasOne(p => p.User).WithMany(u => u.UserPosts).OnDelete(DeleteBehavior.Cascade);
        builder.Entity<Profile>().HasOne(p => p.User).WithOne(u => u.UserProfile).HasForeignKey<Profile>(p => p.UserId).OnDelete(DeleteBehavior.Cascade);
        builder.Entity<Comment>().HasOne(c => c.Post).WithMany(p => p.Comments).OnDelete(DeleteBehavior.Cascade);
        builder.Entity<Comment>().HasOne(c => c.User).WithMany(p => p.UserComments).OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Friendship>().HasOne(f => f.User1).WithMany(u => u.UserSentFriendships);
        builder.Entity<Friendship>().HasOne(f => f.User2).WithMany(u => u.UserReceivedFriendships);


        builder.Entity<GroupMembership>().HasOne(m => m.Group).WithMany(g => g.Memberships);
        builder.Entity<GroupMembership>().HasOne(m => m.User).WithMany(u => u.UserMemberships);

        builder.Entity<Group>().HasOne(g => g.User).WithMany(u => u.UserGroups).OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Message>().HasOne(m => m.User).WithMany(u => u.UserMessages).OnDelete(DeleteBehavior.Cascade);
        builder.Entity<Message>().HasOne(m => m.Group).WithMany(g => g.Messages).OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(builder);
    }

    public DbSet<Profile> Profiles { get; set; }
    public DbSet<Post> Posts { get; set; }

    public DbSet<Comment> Comments { get; set; }

    public DbSet<Friendship> Friendships { get; set; }

    public DbSet<Group> Groups { get; set; }

    public DbSet<GroupMembership> GroupMemberships { get; set; }

    public DbSet<Message> Messages { get; set; }
}