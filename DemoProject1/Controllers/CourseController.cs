using DemoProject1.Models;
using DemoProject1.Util;
using DemoProject1.ViewModel;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace DemoProject1.Controllers
{
    [Authorize(Roles = Role.Staff)]
    public class CourseController : Controller
    {
        private ApplicationDbContext _context;
        public CourseController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: Course
        public ActionResult Index(string SearchCourse)
        {
            var course = _context.CourseDb
                  .Include(t => t.Category)
                  .ToList();

            if (!string.IsNullOrEmpty(SearchCourse))
            {
                course = course
                    .Where(t => t.Name.ToLower().Contains(SearchCourse.ToLower())).
                    ToList();
            }
            return View(course);
        }
        [HttpGet]
        public ActionResult CreateCourse()
        {
            var category = _context.CategoryDb.ToList();
            var viewModel = new CourseViewModel()
            {
                Category = category,
            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult CreateCourse(CourseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new CourseViewModel
                {
                    Course = model.Course,
                    Category = _context.CategoryDb.ToList()
                };
                return View(viewModel);
            }

            var newCourse = new Course()
            {
                Name = model.Course.Name,
                Description = model.Course.Description,
                CategoryId = model.Course.CategoryId,
            };
            _context.CourseDb.Add(newCourse);
            _context.SaveChanges();
            return RedirectToAction("Index", "Course");
        }
        [HttpGet]
        public ActionResult EditCourse(int id)
        {
            var CourseInDb = _context.CourseDb
                .SingleOrDefault(t => t.Id == id);
            if (CourseInDb == null)
            {
                return HttpNotFound();
            }
            var viewModel = new CourseViewModel
            {
                Course = CourseInDb,
                Category = _context.CategoryDb.ToList()
            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult EditCourse(CourseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new CourseViewModel
                {
                    Course = model.Course,
                    Category = _context.CategoryDb.ToList()
                };
                return View(viewModel);
            }
            var CourseInDb = _context.CourseDb
                .SingleOrDefault(t => t.Id == model.Course.Id);
            if (CourseInDb == null)
            {
                return HttpNotFound();
            }
            CourseInDb.Name = model.Course.Name;
            CourseInDb.Description = model.Course.Description;
            CourseInDb.CategoryId = model.Course.CategoryId;
            _context.SaveChanges();
            return RedirectToAction("Index", "Course");
        }
        [HttpGet]
        public ActionResult DeleteCourse(int id)
        {
            var CourseInDb = _context.CourseDb
                .SingleOrDefault(t => t.Id == id);
            /*var CoursesTraineeInDb = _context.TraineesCourses
                .SingleOrDefault(t => t.CourseId == id);
            var CoursesTrainerInDb = _context.TrainersCourses
                .SingleOrDefault(t => t.CourseId == id);*/
            if (CourseInDb == null)
            {
                ModelState.AddModelError("", "Course is not Exist");
                return RedirectToAction("Index", "Course");
            }
            /*            _context.TrainersCourses.Remove(CoursesTrainerInDb);
                        _context.TraineesCourses.Remove(CoursesTraineeInDb);*/
            _context.CourseDb.Remove(CourseInDb);
            _context.SaveChanges();
            return RedirectToAction("Index", "Course");
        }
        [HttpGet]
        public ActionResult GetTrainers(string SearchCourse)
        {
            var course = _context.CourseDb
                .Include(t => t.Category)
                .ToList();
            var trainer = _context.TrainerCourseDb.ToList();

            List<CourseTrainerViewModel> viewModel = _context.TrainerCourseDb
                .GroupBy(i => i.Course)
                .Select(res => new CourseTrainerViewModel
                {
                    Course = res.Key,
                    Trainers = res.Select(u => u.Trainer).ToList()
                })
                .ToList();
            if (!string.IsNullOrEmpty(SearchCourse))
            {
                viewModel = viewModel
                    .Where(t => t.Course.Name.ToLower().Contains(SearchCourse.ToLower())).
                    ToList();
            }
            return View(viewModel);

        }
        [HttpGet]
        public ActionResult AddTrainer()
        {
            var viewModel = new TrainerCourseViewModel
            {
                Courses = _context.CourseDb.ToList(),
                Trainers = _context.TrainerDb.ToList()
            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult AddTrainer(TrainerCourseViewModel viewModel)
        {
            var model = new TrainerCourse
            {
                CourseId = viewModel.CourseId,
                TrainerId = viewModel.TrainerId
            };

            List<TrainerCourse> trainerCourse = _context.TrainerCourseDb.ToList();
            bool alreadyExist = trainerCourse.Any(item => item.CourseId == model.CourseId && item.TrainerId == model.TrainerId);
            if (alreadyExist == true)
            {
                ModelState.AddModelError("", "Trainer is already assigned this Course");
                return RedirectToAction("GetTrainers", "Course");
            }
            _context.TrainerCourseDb.Add(model);
            _context.SaveChanges();

            return RedirectToAction("GetTrainers", "Course");
        }
        [HttpGet]
        public ActionResult RemoveTrainer()
        {
            var trainers = _context.TrainerCourseDb.Select(t => t.Trainer)
                .Distinct()
                .ToList();
            var courses = _context.TrainerCourseDb.Select(t => t.Course)
                .Distinct()
                .ToList();

            var viewModel = new TrainerCourseViewModel
            {
                Courses = courses,
                Trainers = trainers
            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult RemoveTrainer(TrainerCourseViewModel viewModel)
        {
            var userTeam = _context.TrainerCourseDb
                .SingleOrDefault(t => t.CourseId == viewModel.CourseId && t.TrainerId == viewModel.TrainerId);
            if (userTeam == null)
            {
                ModelState.AddModelError("", "Trainer is not assignned in this Course");
                return RedirectToAction("GetTrainers", "Course");
            }

            _context.TrainerCourseDb.Remove(userTeam);
            _context.SaveChanges();

            return RedirectToAction("GetTrainers", "Course");
        }
        [HttpGet]
        public ActionResult AddTrainee()
        {
            var viewModel = new TraineeCourseViewModel
            {
                Courses = _context.CourseDb.ToList(),
                Trainees = _context.TraineeDb.ToList()
            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult AddTrainee(TraineeCourseViewModel viewModel)
        {
            var model = new TraineeCourse
            {
                CourseId = viewModel.CourseId,
                TraineeId = viewModel.TraineeId
            };
            List<TraineeCourse> traineeCourse = _context.TraineeCourseDb.ToList();
            bool alreadyExist = traineeCourse.Any(item => item.CourseId == model.CourseId && item.TraineeId == model.TraineeId);
            if (alreadyExist == true)
            {
                ModelState.AddModelError("", "Trainee is already assigned this Course");
                return RedirectToAction("GetTrainees", "Course");
            }
            _context.TraineeCourseDb.Add(model);
            _context.SaveChanges();

            return RedirectToAction("GetTrainees", "Course");
        }
        [HttpGet]
        public ActionResult GetTrainees(string SearchCourse)
        {
            var courses = _context.CourseDb
                .Include(t => t.Category)
                .ToList();
            var trainee = _context.TraineeCourseDb.ToList();

            List<CourseTraineeViewModel> viewModel = _context.TraineeCourseDb
                .GroupBy(i => i.Course)
                .Select(res => new CourseTraineeViewModel
                {
                    Course = res.Key,
                    Trainees = res.Select(u => u.Trainee).ToList()
                })
                .ToList();
            if (!string.IsNullOrEmpty(SearchCourse))
            {
                viewModel = viewModel
                    .Where(t => t.Course.Name.ToLower().Contains(SearchCourse.ToLower())).
                    ToList();
            }
            return View(viewModel);
        }
        [HttpGet]
        public ActionResult RemoveTrainee()
        {
            var trainees = _context.TraineeCourseDb.Select(t => t.Trainee)
                .Distinct()
                .ToList();
            var courses = _context.TraineeCourseDb.Select(t => t.Course)
                .Distinct()
                .ToList();

            var viewModel = new TraineeCourseViewModel
            {
                Courses = courses,
                Trainees = trainees
            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult RemoveTrainee(TraineeCourseViewModel viewModel)
        {
            var userTeam = _context.TraineeCourseDb
                .SingleOrDefault(t => t.CourseId == viewModel.CourseId && t.TraineeId == viewModel.TraineeId);
            if (userTeam == null)
            {
                ModelState.AddModelError("", "Trainee is not assigned in this Course");
                return RedirectToAction("GetTrainees", "Course");
            }

            _context.TraineeCourseDb.Remove(userTeam);
            _context.SaveChanges();

            return RedirectToAction("GetTrainees", "Course");
        }

    }
}