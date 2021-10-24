using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoProject1.Models
{
    public class Trainer
    {

        public string Name { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        public string Specialty { get; set; }
        [Key]
        [ForeignKey("User")]
        public string TrainerId { get; set; }
        public ApplicationUser User { get; set; }
    }
}