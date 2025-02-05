using ASPNET_RESTAPI.DbModel;

namespace ASPNET_RESTAPI.Model {
    public class Course {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Semester { get; set; }
        public int TotalAttempts { get; set; }
        public int Credit { get; set; }

        public Course() { }

        public Course(DbCourse dbCourse) {
            ID = dbCourse.ID;
            Name = dbCourse.Name;
            Semester = dbCourse.Semester;
            TotalAttempts = dbCourse.TotalAttempts;
            Credit = dbCourse.Credit;
        }
    }
}
