using ASPNET_RESTAPI.DbModel;
using System.Text.Json.Serialization;

namespace ASPNET_RESTAPI.Model {
    public class Student {
        [JsonPropertyName("neptun")]
        public string NEPTUN { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [JsonPropertyName("major")]
        public string Major { get; set; }

        [JsonPropertyName("courses")]
        public List<StudentCourse> Courses { get; set; }

        public Student() {}

        public Student(DbStudent dbStudent, string dbMajor, List<StudentCourse> courses) {
            NEPTUN = dbStudent.NEPTUN;
            Name = dbStudent.Name;
            Email = dbStudent.Email;
            Phone = dbStudent.Phone;
            Major = dbMajor;
            Courses = courses ?? new List<StudentCourse>();
        }
    }
}
