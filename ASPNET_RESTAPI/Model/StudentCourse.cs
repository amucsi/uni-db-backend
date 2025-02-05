namespace ASPNET_RESTAPI.Model {
    public class StudentCourse { //egy hallgato altal felvett targyak, es egy adott targy probalkozasai
        public int CourseID { get; set; } //esetleg az egesz course object kene ehelyett
        public string Name { get; set; }
        public int Semester { get; set; }
        public List<CourseAttempt> Attempts { get; set; } = new List<CourseAttempt>();
    }

    public class CourseAttempt {
        public int AttemptNumber { get; set; }
        public int Grade { get; set; }
    }
}
