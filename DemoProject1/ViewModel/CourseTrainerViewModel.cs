using DemoProject1.Models;
using System.Collections.Generic;

namespace DemoProject1.ViewModel
{
    public class CourseTrainerViewModel
    {
        public Course Course { get; set; }
        public List<Trainer> Trainers { get; set; }
    }
}