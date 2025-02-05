using ASPNET_RESTAPI.DbModel;
using ASPNET_RESTAPI.Model;

namespace ASPNET_RESTAPI.DAL {
    public class CourseRepository {
        private readonly UniDbContext _dbContext;

        public CourseRepository(UniDbContext dbContext) {
            this._dbContext = dbContext;
        }

        public async Task<IReadOnlyCollection<Course>> ListAsync() {
            var Courses = new List<Course>();
            foreach (var course in _dbContext.Courses) {
                Courses.Add(new Course(course));
            }
            return Courses;

            //var courses = await _dbContext.Courses.ToListAsync();
            //return courses.Select(course => new Course(course)).ToList();
        }

        public bool TryGetCourseById(int id, out Course? course) {
            var dbCourse = _dbContext.Courses.FirstOrDefault(c => c.ID == id);
            if (dbCourse == null) {
                course = null;
                return false;
            }

            course = new Course(dbCourse);
            return true;
        }

        public bool AddCourse(Course course) {
            var dbCourse = new DbCourse {
                Name = course.Name,
                Semester = course.Semester,
                Credit = course.Credit,
            };
            try {
                _dbContext.Courses.Add(dbCourse);
                _dbContext.SaveChanges();
                _dbContext.Entry(dbCourse).Reload();
                return true;
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
                return false;
            }
        }

        public bool DeleteCourse(int id) {
            var delCourse = _dbContext.Courses.FirstOrDefault(c =>  c.ID == id);
            if (delCourse == null) return false;

            try {
                _dbContext.Courses.Remove(delCourse);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}
