using DemoProject1.Models;
using DemoProject1.Util;
using DemoProject1.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DemoProject1.Controllers
{
    [Authorize(Roles = Role.Staff)]
    public class StaffController : Controller
    {
        private ApplicationDbContext _context;
        private ApplicationUserManager _userManager;
        public StaffController()
        {
            _context = new ApplicationDbContext();
        }
        public StaffController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
            _context = new ApplicationDbContext();
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: Staff
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult TraineeList(string searchName)
        {
            var Trainee = _context.TraineeDb.Include(t => t.User).ToList();
            if (!string.IsNullOrEmpty(searchName))
            {
                Trainee = Trainee.Where(t => t.Name.ToLower().Contains(searchName.ToLower()) || t.Age.ToString().Contains(searchName.ToLower())).ToList();
            }
            return View(Trainee);
        }
        [HttpGet]
        public ActionResult CreateTrainee()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateTrainee(CreateTraineeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = viewModel.Email, Email = viewModel.Email };
                var result = await UserManager.CreateAsync(user, viewModel.Password);
                var TraineeId = user.Id;
                var newTrainee = new Trainee()
                {
                    TraineeId = TraineeId,
                    Name = viewModel.Name,
                    Age = viewModel.Age,
                    DateOfBirth = viewModel.DateOfBirth,
                    Address = viewModel.Address,
                    Education = viewModel.Education
                };
                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user.Id, Role.Trainee);
                    _context.TraineeDb.Add(newTrainee);
                    _context.SaveChanges();
                    return RedirectToAction("TraineeList", "Staff");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(viewModel);
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        [HttpGet]
        public ActionResult DeleteTrainee(string id)
        {
            var traineeInDb = _context.Users
                .SingleOrDefault(t => t.Id == id);
            var traineeInfoInDb = _context.TraineeDb
                .SingleOrDefault(t => t.TraineeId == id);
            if (traineeInDb == null || traineeInfoInDb == null)
            {
                return HttpNotFound();
            }
            _context.Users.Remove(traineeInDb);
            _context.TraineeDb.Remove(traineeInfoInDb);
            _context.SaveChanges();
            return RedirectToAction("TraineeList", "Staff");
        }
        [HttpGet]
        public ActionResult EditTrainee(string id)
        {
            var traineeInDb = _context.TraineeDb
                .SingleOrDefault(t => t.TraineeId == id);
            if (traineeInDb == null)
            {
                return HttpNotFound();
            }
            return View(traineeInDb);
        }

        [HttpPost]
        public ActionResult EditTrainee(Trainee trainee)
        {
            var traineeInDb = _context.TraineeDb.SingleOrDefault(t => t.TraineeId == trainee.TraineeId);
            if (traineeInDb == null)
            {
                return HttpNotFound();
            }
            traineeInDb.Name = trainee.Name;
            traineeInDb.Age = trainee.Age;
            traineeInDb.Address = trainee.Address;

            _context.SaveChanges();
            return RedirectToAction("TraineeList", "Staff");
        }

    }
}