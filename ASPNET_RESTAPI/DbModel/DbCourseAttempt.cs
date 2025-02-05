using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPNET_RESTAPI.DbModel {
    [Table("Course_attempt")]
    public class DbCourseAttempt {
        [Key]
        public int ID { get; set; }

        [Required]
        [Column("course_id")]
        public int CourseID { get; set; }
        [ForeignKey("CourseID")]
        public DbCourse Course { get; set; }

        [Required]
        [Column("student_id")]
        public int StudentID { get; set; }
        [ForeignKey("StudentID")]
        public DbStudent Student { get; set; }

        [Required]
        public int Grade { get; set; }

        [Required]
        [Column("attempt_number")]
        public int AttemptNumber { get; set; }
    }
}
