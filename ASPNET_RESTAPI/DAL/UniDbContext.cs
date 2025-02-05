using ASPNET_RESTAPI.DbModel;
using Microsoft.EntityFrameworkCore;

namespace ASPNET_RESTAPI.DAL {
    public class UniDbContext : DbContext {
        public UniDbContext(DbContextOptions<UniDbContext> options) : base(options) { }

        public DbSet<DbStudent> Students { get; set; }
        public DbSet<DbCourse> Courses { get; set; }
        public DbSet<DbCourseAttempt> CourseAttempts { get; set; }
        public DbSet<DbMajor> Majors { get; set; }
    }
}
