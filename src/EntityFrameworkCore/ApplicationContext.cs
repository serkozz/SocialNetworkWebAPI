using Microsoft.EntityFrameworkCore;
using Models;

namespace EF;
public class ApplicationContext : DbContext
{
    public virtual DbSet<Auth> Auth { get; set; }
    public virtual DbSet<Profile> Profiles { get; set; }
    public virtual DbSet<Friends> Friends { get; set; }
    public virtual DbSet<Subscription> Subscriptions { get; set; }
    // public DbSet<Chat> Chats { get; set; }
    // public DbSet<Message> Messages { get; set; }

    public ApplicationContext() { }
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options) { }
    
}