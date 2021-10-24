using DemoProject1.Models;
using DemoProject1.Util;
using DemoProject1.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static DemoProject1.Controllers.ManageController;

namespace DemoProject1.Controllers
{
    [Authorize(Roles = Role.Admin)]
    public class AdminController : Controller
    {
        private ApplicationDbContext _context;
        private ApplicationUserManager _userManager;
        public AdminController()
        {
            _context = new ApplicationDbContext();
        }
        public AdminController(ApplicationUserManager userManager)
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
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult StaffList()
        {
            var Staff = _context.StaffDb.Include(t => t.User).ToList();
            return View(Staff);
        }
        [HttpGet]
        public ActionResult CreateStaff()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateStaff(CreateStaffViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = viewModel.Email, Email = viewModel.Email };
                var result = await UserManager.CreateAsync(user, viewModel.Password);
                var StaffId = user.Id;
                var newStaff = new Staff()
                {
                    StaffId = StaffId,
                    Name = viewModel.Name,
                    Age = viewModel.Age,
                    Address = viewModel.Address
                };
                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user.Id, Role.Staff);
                    _context.StaffDb.Add(newStaff);
                    _context.SaveChanges();
                    return RedirectToAction("StaffList", "Admin");
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
        public ActionResult DeleteStaff(string id)
        {
            var staffInDb = _context.Users
                .SingleOrDefault(t => t.Id == id);
            var staffInfoInDb = _context.StaffDb
                .SingleOrDefault(t => t.StaffId == id);
            if (staffInDb == null || staffInfoInDb == null)
            {
                return HttpNotFound();
            }
            _context.Users.Remove(staffInDb);
            _context.StaffDb.Remove(staffInfoInDb);
            _context.SaveChanges();
            return RedirectToAction("StaffList", "Admin");
        }

        [HttpGet]
        public ActionResult EditStaff(string id)
        {
            var staffInDb = _context.StaffDb
                .SingleOrDefault(t => t.StaffId == id);
            if (staffInDb == null)
            {
                return HttpNotFound();
            }
            return View(staffInDb);
        }

        [HttpPost]
        public ActionResult EditStaff(Staff staff)
        {
            var staffInDb = _context.StaffDb.SingleOrDefault(t => t.StaffId == staff.StaffId);
            if (staffInDb == null)
            {
                return HttpNotFound();
            }
            staffInDb.Name = staff.Name;
            staffInDb.Age = staff.Age;
            staffInDb.Address = staff.Address;

            _context.SaveChanges();
            return RedirectToAction("StaffList", "Admin");
        }

        public ActionResult StaffChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> StaffChangePassword(PasswordViewModel model, string id)
        {
            var userInDb = _context.Users.SingleOrDefault(i => i.Id == id);
            if (userInDb == null)
            {
                return HttpNotFound();
            }
            var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            userId = userInDb.Id;

            if (userId != null)
            {
                UserManager<IdentityUser> userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>());
                userManager.RemovePassword(userId);
                string newPassword = model.NewPassword;
                userManager.AddPassword(userId, newPassword);
            }
            _context.SaveChanges();
            return RedirectToAction("StaffList", "Admin", new { Message = ManageMessageId.ChangePasswordSuccess });
        }
    }
}