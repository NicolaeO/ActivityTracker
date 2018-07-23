using Microsoft.EntityFrameworkCore;
 
namespace ActivityTracker.Models
{
    public class ActivityContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public ActivityContext(DbContextOptions<ActivityContext> options) : base(options) { }

        public DbSet<Person> Users {get; set;}
        public DbSet<MyActivity> Activities {get; set;}
        public DbSet<UserActivity> UsersActivities {get; set;}

    }
}