using DemoProject1.Models;
using DemoProject1.Util;
using DemoProject1.ViewModel;
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
    }
}