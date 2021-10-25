using DemoProject1.Models;
using System.Collections.Generic;

namespace DemoProject1.ViewModel
{
    public class CourseTraineeViewModel
    {
        public Course Course { get; set; }
        public List<Trainee> Trainees { get; set; }
    }
}