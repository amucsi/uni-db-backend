using ASPNET_RESTAPI.DbModel;
using ASPNET_RESTAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace ASPNET_RESTAPI.DAL {
    public class StudentRepository {
        private readonly UniDbContext _dbContext; //Dependency Inj

        public StudentRepository(UniDbContext dbContext) {
            this._dbContext = dbContext;
        }

        public IReadOnlyCollection<StudentPreview> List() {
            var Students = new List<StudentPreview>();
            foreach (var dbStudent in _dbContext.Students) {
                Students.Add(new StudentPreview(dbStudent));
            }
            return Students;
        }

        public bool TryGetStudentByNeptun(string neptunCode, out Student? Student) {
            var studentEntity = _dbContext.Students.FirstOrDefault(s => s.NEPTUN == neptunCode);
            if (studentEntity == null) {
                Student = null;
                return false;
            }

            var dbMajor = _dbContext.Majors.FirstOrDefault(m => m.ID == studentEntity.MajorId);
            if (dbMajor == null) {
                Student = null;
                return false;
            }

            var studentCourses = new List<StudentCourse>();

            var query = _dbContext.CourseAttempts
                .Include(ca => ca.Course)
                .Include(ca => ca.Student)
                .Where(ca => ca.Student.NEPTUN == neptunCode)
                .GroupBy(ca => ca.Course.ID);

            foreach (var group in query) {
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

            Student = new Student(studentEntity, dbMajor.Name, studentCourses);
            return true;
        }

        public bool TryGetStudentNeptunByName(string studentName, out string neptun) {
            var studentEntity = _dbContext.Students.FirstOrDefault(s => s.Name == studentName);
            if (studentEntity == null) {
                neptun = "";
                return false;
            }

            neptun = studentEntity.NEPTUN;
            return true;
        }

        public bool AddStudent(Student student) {
            var major = _dbContext.Majors.FirstOrDefault(m => m.Name == student.Major);
            if (major == null) return false;

            var newStudent = new DbStudent {
                NEPTUN = student.NEPTUN,
                Name = student.Name,
                Email = student.Email,
                Phone = student.Phone,
                MajorId = major.ID,
            };

            _dbContext.Students.Add(newStudent);
            _dbContext.SaveChanges();
            _dbContext.Entry(newStudent).Reload();
            return true;
        }

        public bool DeleteStudent(string neptun) {
            var delStudent = _dbContext.Students.FirstOrDefault(m => m.NEPTUN == neptun);
            if (delStudent == null) return false;

            try {
                _dbContext.Students.Remove(delStudent);
                _dbContext.SaveChanges();
                _dbContext.ChangeTracker.Clear(); //refresh db cascade után
                return true;
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
                return false;
            }
        }

        public bool AddCourseAttempt(string neptun, int courseId, CourseAttempt newAttempt) {
            var student = _dbContext.Students.FirstOrDefault(s => s.NEPTUN == neptun);
            if (student == null) return false;

            var course = _dbContext.Courses.FirstOrDefault(c => c.ID == courseId);
            if (course == null) return false;

            var dbAttempt = new DbCourseAttempt {
                StudentID = student.ID,
                CourseID = course.ID,
                Grade = newAttempt.Grade,
            };

            try {
                _dbContext.CourseAttempts.Add(dbAttempt);
                _dbContext.SaveChanges();
                _dbContext.Entry(dbAttempt).Reload();
                return true;
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}
