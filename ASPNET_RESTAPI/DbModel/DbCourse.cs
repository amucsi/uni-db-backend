using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPNET_RESTAPI.DbModel {
    [Table("Course")]
    public class DbCourse {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public int Semester { get; set; }

        [Column("total_attempts")]
        public int TotalAttempts { get; set; }

        public int Credit { get; set; }
    }
}
