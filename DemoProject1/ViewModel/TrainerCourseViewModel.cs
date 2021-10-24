using DemoProject1.Models;
using System.Collections.Generic;

namespace DemoProject1.ViewModel
{
    public class TrainerCourseViewModel
    {
        public int CourseId { get; set; }
        public List<Course> Courses { get; set; }
        public string TrainerId { get; set; }
        public List<Trainer> Trainers { get; set; }
    }
}