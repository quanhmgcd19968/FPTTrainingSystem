using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace DemoProject1.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("AzureDeployApp", throwIfV1Schema: false)
        {
        }
        public DbSet<Staff> StaffDb { get; set; }
        public DbSet<Trainer> TrainerDb { get; set; }
        public DbSet<Trainee> TraineeDb { get; set; }
        public DbSet<Category> CategoryDb { get; set; }
        public DbSet<Course> CourseDb { get; set; }
        public DbSet<TrainerCourse> TrainerCourseDb { get; set; }
        public DbSet<TraineeCourse> TraineeCourseDb { get; set; }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}