using Microsoft.EntityFrameworkCore;
 
namespace EventApp.Models
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options) { }
        public DbSet<User> Users {get; set;}
        public DbSet<ActivityModel> Activities {get; set;}
        public DbSet<Join> Joins {get; set;}
        public DbSet<Message> Messages {get;set;}
    }
}