using ASPNET_RESTAPI.DbModel;
using ASPNET_RESTAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace ASPNET_RESTAPI.DAL {
    public class CourseRepository {
        private readonly UniDbContext _dbContext;

        public CourseRepository(UniDbContext dbContext) {
            this._dbContext = dbContext;
        }

        public async Task<IReadOnlyCollection<Course>> ListAsync() {
            var courses = await _dbContext.Courses.ToListAsync();
            return courses.Select(c => new Course(c)).ToList();
        }

        public async Task<(bool, Course?)> GetCourseByIdAsync(int id) {
            var dbCourse = await _dbContext.Courses.FirstOrDefaultAsync(c => c.ID == id);
            if (dbCourse == null)
                return (false, null);
            else
                return (true, new Course(dbCourse));
        }

        public async Task<bool> AddCourseAsync(Course course) {
            var dbCourse = new DbCourse {
                Name = course.Name,
                Semester = course.Semester,
                Credit = course.Credit,
            };
            try {
                _dbContext.Courses.Add(dbCourse);
                await _dbContext.SaveChangesAsync();
                await _dbContext.Entry(dbCourse).ReloadAsync();
                return true;
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> DeleteCourseAsync(int id) {
            var delCourse = await _dbContext.Courses.FirstOrDefaultAsync(c =>  c.ID == id);
            if (delCourse == null)
                return false;

            try {
                _dbContext.Courses.Remove(delCourse);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}
