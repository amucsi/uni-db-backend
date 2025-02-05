using ASPNET_RESTAPI.DbModel;

namespace ASPNET_RESTAPI.Model {
    public class StudentPreview {
        public string Neptun { get; set; }
        public string Name { get; set; }

        public StudentPreview(DbStudent dbStudent) {
            Neptun = dbStudent.NEPTUN;
            Name = dbStudent.Name;
        }
    }
}
