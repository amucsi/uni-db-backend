using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPNET_RESTAPI.DbModel {
    [Table("Student")]
    public class DbStudent {

        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(6)]
        public required string NEPTUN { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MaxLength(20)]
        public string Phone { get; set; }

        [Required]
        [Column("major_id")]
        public int MajorId { get; set; }
        [ForeignKey("MajorId")]
        public DbMajor Major { get; set; }
    }
}
