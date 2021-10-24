using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoProject1.Models
{
    public class Trainee
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Education { get; set; }
        [Key]
        [ForeignKey("User")]
        public String TraineeId { get; set; }
        public ApplicationUser User { get; set; }
    }
}