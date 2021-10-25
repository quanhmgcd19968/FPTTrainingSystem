using DemoProject1.Models;
using System.Collections.Generic;

namespace DemoProject1.ViewModel
{
    public class TraineeCourseViewModel
    {
        public int CourseId { get; set; }
        public List<Course> Courses { get; set; }
        public string TraineeId { get; set; }
        public List<Trainee> Trainees { get; set; }
    }
}