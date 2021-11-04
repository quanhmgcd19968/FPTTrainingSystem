using DemoProject1.Models;
using DemoProject1.Util;
using System.Linq;
using System.Web.Mvc;

namespace DemoProject1.Controllers
{
    [Authorize(Roles = Role.Staff)]
    public class CategoryController : Controller
    {
        private ApplicationDbContext _context;
        public CategoryController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: Category
        [HttpGet]
        public ActionResult Index(string SearchCategory)
        {
            var category = _context.CategoryDb.ToList();
            if (!string.IsNullOrEmpty(SearchCategory))
            {
                category = category
                    .Where(t => t.Name.ToLower().Contains(SearchCategory.ToLower())).
                    ToList();
            }
            return View(category);
        }
        [HttpGet]
        public ActionResult CreateCategory()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateCategory(Category category)
        {
            var newCategory = new Category()
            {
                Name = category.Name,
                Description = category.Description
            };
            _context.CategoryDb.Add(newCategory);
            _context.SaveChanges();
            return RedirectToAction("Index", "Category");
        }
        [HttpGet]
        public ActionResult DeleteCategory(int id)
        {
            var categoryInDb = _context.CategoryDb.SingleOrDefault(t => t.Id == id);
            if (categoryInDb == null)
            {
                return HttpNotFound();
            }
            _context.CategoryDb.Remove(categoryInDb);
            _context.SaveChanges();
            return RedirectToAction("Index", "Category");
        }
        [HttpGet]
        public ActionResult EditCategory(int id)
        {
            var categoryInDb = _context.CategoryDb.SingleOrDefault(t => t.Id == id);
            if (categoryInDb == null)
            {
                return HttpNotFound();
            }
            return View(categoryInDb);
        }
        [HttpPost]
        public ActionResult EditCategory(Category category)
        {
            var categoryInDb = _context.CategoryDb.SingleOrDefault(t => t.Id == category.Id);
            if (categoryInDb == null)
            {
                return HttpNotFound();
            }
            categoryInDb.Name = category.Name;
            categoryInDb.Description = category.Description;
            _context.SaveChanges();
            return RedirectToAction("Index", "Category");
        }

    }
}