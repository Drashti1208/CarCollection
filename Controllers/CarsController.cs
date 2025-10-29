using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarCollection.Web.Data;
using CarCollection.Web.Models;

namespace CarCollection.Web.Controllers
{
    public class CarsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _env;

        public CarsController(ApplicationDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        // GET: Cars
        public async Task<IActionResult> Index(string? search, string? make, int? year)
        {
            var query = _db.Cars.Include(c=>c.Photos).AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(c => c.Make.Contains(search) || c.Model.Contains(search) || (c.VIN ?? "").Contains(search));
            }

            if (!string.IsNullOrWhiteSpace(make))
                query = query.Where(c => c.Make == make);

            if (year.HasValue)
                query = query.Where(c => c.Year == year.Value);

            var list = await query.OrderBy(c => c.Make).ThenBy(c => c.Model).ToListAsync();
            return View(list);
        }

        // GET: Cars/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var car = await _db.Cars.Include(c => c.Photos).Include(c => c.ServiceRecords)
                       .FirstOrDefaultAsync(c => c.Id == id);
            if (car == null) return NotFound();
            return View(car);
        }

        // GET: Cars/Create
        public IActionResult Create() => View();

        // POST: Cars/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Car car, IFormFile? photo)
        {
            if (!ModelState.IsValid) return View(car);

            _db.Cars.Add(car);
            await _db.SaveChangesAsync();

            if (photo != null && photo.Length > 0)
            {
                var uploads = Path.Combine(_env.WebRootPath, "uploads");
                var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(photo.FileName)}";
                var fullPath = Path.Combine(uploads, fileName);
                using (var fs = new FileStream(fullPath, FileMode.Create))
                {
                    await photo.CopyToAsync(fs);
                }

                var p = new CarPhoto { CarId = car.Id, FilePath = "/uploads/" + fileName, IsPrimary = true };
                _db.CarPhotos.Add(p);
                await _db.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Cars/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var car = await _db.Cars.FirstOrDefaultAsync(c => c.Id == id);
            if (car == null) return NotFound();
            return View(car);
        }

        // POST: Cars/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Car car, IFormFile? photo)
        {
            if (id != car.Id) return BadRequest();
            if (!ModelState.IsValid) return View(car);

            try
            {
                _db.Update(car);
                await _db.SaveChangesAsync();

                if (photo != null && photo.Length > 0)
                {
                    var uploads = Path.Combine(_env.WebRootPath, "uploads");
                    var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(photo.FileName)}";
                    var fullPath = Path.Combine(uploads, fileName);
                    using var fs = new FileStream(fullPath, FileMode.Create);
                    await photo.CopyToAsync(fs);

                    var p = new CarPhoto { CarId = car.Id, FilePath = "/uploads/" + fileName, IsPrimary = false };
                    _db.CarPhotos.Add(p);
                    await _db.SaveChangesAsync();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarExists(car.Id)) return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Cars/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var car = await _db.Cars.FirstOrDefaultAsync(c => c.Id == id);
            if (car == null) return NotFound();
            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var car = await _db.Cars.Include(c => c.Photos).FirstOrDefaultAsync(c => c.Id == id);
            if (car == null) return NotFound();

            // remove photos from disk
            if (car.Photos != null)
            {
                foreach (var ph in car.Photos)
                {
                    var file = ph.FilePath?.TrimStart('/');
                    if (!string.IsNullOrEmpty(file))
                    {
                        var full = Path.Combine(_env.WebRootPath, file.Replace('/', Path.DirectorySeparatorChar));
                        if (System.IO.File.Exists(full)) System.IO.File.Delete(full);
                    }
                }
            }

            _db.Cars.Remove(car);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarExists(int id) => _db.Cars.Any(e => e.Id == id);
    }
}
