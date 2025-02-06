using ASPNET_RESTAPI.DbModel;
using ASPNET_RESTAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace ASPNET_RESTAPI.DAL {
    public class StudentRepository {
        private readonly UniDbContext _dbContext; //Dependency Inj

        public StudentRepository(UniDbContext dbContext) {
            this._dbContext = dbContext;
        }

        public async Task<IReadOnlyCollection<StudentPreview>> ListAsync() {
            var students = await _dbContext.Students.ToListAsync();
            return students.Select(s => new StudentPreview(s)).ToList();
        }

        public async Task<(bool, Student?)> GetStudentByNeptunAsync(string neptunCode) {
            var studentEntity = await _dbContext.Students.FirstOrDefaultAsync(s => s.NEPTUN == neptunCode);
            if (studentEntity == null)
                return (false, null);

            var dbMajor = await _dbContext.Majors.FirstOrDefaultAsync(m => m.ID == studentEntity.MajorId);
            if (dbMajor == null)
                return (false, null);

            var studentCourses = new List<StudentCourse>();

            var query = _dbContext.CourseAttempts
                .Include(ca => ca.Course)
                .Include(ca => ca.Student)
                .Where(ca => ca.Student.NEPTUN == neptunCode)
                .GroupBy(ca => ca.Course.ID);

            await foreach (var group in query.AsAsyncEnumerable()) {
                var course = group.First().Course;

                studentCourses.Add(new StudentCourse {
                    CourseID = course.ID,
                    Name = course.Name,
                    Semester = course.Semester,
                    Attempts = group.Select(ca => new CourseAttempt {
                        AttemptNumber = ca.AttemptNumber,
                        Grade = ca.Grade
                    }).OrderBy(ca => ca.AttemptNumber).ToList()
                });
            }

            var student = new Student(studentEntity, dbMajor.Name, studentCourses);
            return (true, student);
        }

        public async Task<bool> AddStudentAsync(Student student) {
            var major = await _dbContext.Majors.FirstOrDefaultAsync(m => m.Name == student.Major);
            if (major == null)
                return false;

            var newStudent = new DbStudent {
                NEPTUN = student.NEPTUN,
                Name = student.Name,
                Email = student.Email,
                Phone = student.Phone,
                MajorId = major.ID,
            };

            try {
                _dbContext.Students.Add(newStudent);
                await _dbContext.SaveChangesAsync();
                await _dbContext.Entry(newStudent).ReloadAsync();
                return true;
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> DeleteStudentAsync(string neptun) {
            var delStudent = await _dbContext.Students.FirstOrDefaultAsync(m => m.NEPTUN == neptun);
            if (delStudent == null)
                return false;

            try {
                _dbContext.Students.Remove(delStudent);
                await _dbContext.SaveChangesAsync();
                _dbContext.ChangeTracker.Clear(); //refresh db cascade után
                return true;
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> AddCourseAttemptAsync(string neptun, int courseId, CourseAttempt newAttempt) {
            var student = await _dbContext.Students.FirstOrDefaultAsync(s => s.NEPTUN == neptun);
            if (student == null)
                return false;

            var course = await _dbContext.Courses.FirstOrDefaultAsync(c => c.ID == courseId);
            if (course == null)
                return false;

            var dbAttempt = new DbCourseAttempt {
                StudentID = student.ID,
                CourseID = course.ID,
                Grade = newAttempt.Grade,
            };

            try {
                _dbContext.CourseAttempts.Add(dbAttempt);
                await _dbContext.SaveChangesAsync();
                await _dbContext.Entry(dbAttempt).ReloadAsync();
                return true;
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}
