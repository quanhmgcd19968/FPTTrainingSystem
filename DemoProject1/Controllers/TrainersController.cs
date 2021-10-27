using DemoProject1.Models;
using DemoProject1.Util;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Mvc;

namespace DemoProject1.Controllers
{
    [Authorize(Roles = Role.Trainer)]
    public class TrainersController : Controller
    {
        private ApplicationDbContext _context;
        public TrainersController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: Trainers
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var trainerInDb = _context.TrainerDb
                .SingleOrDefault(t => t.TrainerId == userId);
            return View(trainerInDb);
        }


        [HttpGet]
        public ActionResult Edit(string id)
        {
            var trainerInDb = _context.TrainerDb
                .SingleOrDefault(t => t.TrainerId == id);
            if (trainerInDb == null)
            {
                return HttpNotFound();
            }
            return View(trainerInDb);
        }

        [HttpPost]
        public ActionResult Edit(Trainer trainer)
        {
            var trainerInDb = _context.TrainerDb.SingleOrDefault(t => t.TrainerId == trainer.TrainerId);
            if (trainerInDb == null)
            {
                return HttpNotFound();
            }
            trainerInDb.Name = trainer.Name;
            trainerInDb.Age = trainer.Age;
            trainerInDb.Address = trainer.Address;
            trainerInDb.Specialty = trainer.Specialty;

            _context.SaveChanges();
            return RedirectToAction("Index", "Trainers");
        }

    }
}