using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPNET_RESTAPI.DbModel {
    [Table("Major")]
    public class DbMajor {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
