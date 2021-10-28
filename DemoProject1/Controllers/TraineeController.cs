using DemoProject1.Models;
using DemoProject1.Util;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Mvc;

namespace DemoProject1.Controllers
{
    [Authorize(Roles = Role.Trainee)]
    public class TraineeController : Controller
    {

        private ApplicationDbContext _context;
        public TraineeController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: Trainee
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var traineeInDb = _context.TraineeDb
                .SingleOrDefault(t => t.TraineeId == userId);
            return View(traineeInDb);
        }

        [HttpGet]
        public ActionResult Courses()
        {
            var userId = User.Identity.GetUserId();
            var catagory = _context.CategoryDb.ToList();
            var courses = _context.TraineeCourseDb
                .Where(t => t.Trainee.TraineeId == userId)
                .Select(t => t.Course)
                .ToList();
            return View(courses);
        }
    }
}