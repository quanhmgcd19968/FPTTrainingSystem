using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoProject1.Models
{
    public class Staff
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        [Key]
        [ForeignKey("User")]
        public string StaffId { get; set; }
        public ApplicationUser User { get; set; }
    }
}