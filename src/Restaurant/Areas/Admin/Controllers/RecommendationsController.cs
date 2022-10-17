#nullable disable
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant.Areas.Admin.Validators;
using Restaurant.Areas.Admin.ViewModels;
using Restaurant.Data;
using Restaurant.Models;
using Restaurant.ViewModels;

namespace Restaurant.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class RecommendationsController : Controller
    {
        private readonly ILogger<RecommendationsController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly IFileValidator _fileValidator;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;

        public RecommendationsController(
            ApplicationDbContext db,
            IFileValidator fileValidator,
            ILogger<RecommendationsController> logger,
            IWebHostEnvironment env,
            IMapper mapper)
        {
            _db = db;
            _fileValidator = fileValidator;
            _logger = logger;
            _env = env;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _db.Recommendations
                .OrderBy(r => r.OrderLevel)
                .ToListAsync();

            return View(_mapper.Map<IEnumerable<Recommendation>, IEnumerable<RecommendationViewModel>>(items));
        }

        public async Task<IActionResult> Details(int? id)
        {
            var recommendation = await _db.Recommendations.FindAsync(id);
            if (recommendation == null)
                return NotFound();

            return View(_mapper.Map<RecommendationViewModel>(recommendation));
        }

        public async Task<IActionResult> Create()
        {
            ViewData["MenuItems"] = await _db.Menus
                .Include(m => m.MenuItems)
                .Select(m => new MenuViewModel()
                {
                    Name = m.Name,
                    MenuItems = m.MenuItems.Select(menuItem => new MenuItemViewModel()
                    {
                        Id = menuItem.Id,
                        Name = menuItem.Name
                    }).ToList()
                }).ToListAsync();

            return View(new AddRecommendationViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddRecommendationViewModel model)
        {
            if (ModelState.IsValid)
            {

                var newRecommendation = new Recommendation()
                {
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price
                };

                //if (model.Picture != null)
                //{
                //    if (!_fileValidator.IsValid(model.Picture))
                //    {
                //        ModelState.AddModelError("", "Invalid file.");
                //        return View(model);
                //    }

                //    var fileName = Guid.NewGuid().ToString("N") + Path.GetExtension(model.Picture.FileName);
                //    var uploaded = await UploadPictureAsync(model.Picture, fileName);
                //    if (uploaded)
                //        newRecommendation.PictureUrl = fileName;
                //}

                _db.Recommendations.Add(newRecommendation);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);

        }

        public async Task<IActionResult> Edit(int? id)
        {
            var recommendation = await _db.Recommendations.FindAsync(id);
            if (recommendation == null)
                return NotFound();

            return View(_mapper.Map<AddRecommendationViewModel>(recommendation));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AddRecommendationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var recommendationToEdit = await _db.Recommendations.FindAsync(id);
                if (recommendationToEdit == null)
                    return NotFound();

                recommendationToEdit.Name = model.Name;
                recommendationToEdit.Description = model.Description;
                recommendationToEdit.NameEn = model.NameEn;
                recommendationToEdit.DescriptionEn = model.DescriptionEn;
                recommendationToEdit.Price = model.Price;

                _db.Recommendations.Update(recommendationToEdit);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recommendation = await _db.Recommendations.FindAsync(id);
            if (recommendation == null)
                return NotFound();

            // Delete picture
            if (!string.IsNullOrEmpty(recommendation.PictureUrl))
                DeletePicture(recommendation.PictureUrl);

            _db.Recommendations.Remove(recommendation);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrderLevel(IEnumerable<OrderLevelViewModel> data)
        {
            var recommendationIds = data.Select(s => s.ItemId).ToList();

            var itemsToUpdate = await _db.Recommendations
                .Where(m => recommendationIds.Contains(m.Id))
                .ToListAsync();

            foreach (var item in itemsToUpdate)
            {
                item.OrderLevel = data.FirstOrDefault(m => m.ItemId == item.Id).ItemLevel;
            }

            _db.Recommendations.UpdateRange(itemsToUpdate);
            await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("Admin/Recommendations/{id}/UpdatePicture")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePicture(int id, IFormFile file)
        {
            var recommendation = await _db.Recommendations.FindAsync(id);
            if (recommendation == null)
                return NotFound();

            //if (!_fileValidator.IsValid(file))
            //    return BadRequest();

            //// Upload the new picture
            //var fileName = Guid.NewGuid().ToString("N") + Path.GetExtension(file.FileName);
            //var uploaded = await UploadPictureAsync(file, fileName);
            //if (uploaded)
            //{
            //    // Delete the previous picture
            //    if (!string.IsNullOrEmpty(recommendation.PictureUrl))
            //        DeletePicture(recommendation.PictureUrl);

            //    // Update database
            //    recommendation.PictureUrl = fileName;

            //    _db.Recommendations.Update(recommendation);
            //    await _db.SaveChangesAsync();
            //}

            return Ok(recommendation.PictureUrl);
        }

        [HttpPost("Admin/Recommendations/{id}/RemovePicture")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemovePicture(int id)
        {
            var recommendation = await _db.Recommendations.FindAsync(id);
            if (recommendation == null || string.IsNullOrEmpty(recommendation.PictureUrl))
                return NotFound();

            //var deleted = DeletePicture(recommendation.PictureUrl);
            //if (deleted)
            //{
            //    recommendation.PictureUrl = null;

            //    _db.Recommendations.Update(recommendation);
            //    await _db.SaveChangesAsync();
            //}

            return Ok();
        }

        private async Task<bool> UploadPictureAsync(IFormFile file, string fileName)
        {
            try
            {
                var folderPath = Path.Combine(_env.WebRootPath, "uploads");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var filePath = Path.Combine(folderPath, fileName);
                using var fs = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(fs);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error uploading {file.FileName} to filePath.", ex.Message);
            }

            return false;
        }

        private bool DeletePicture(string fileName)
        {
            try
            {
                var folderPath = Path.Combine(_env.WebRootPath, "uploads");
                var filePath = Path.Combine(folderPath, Path.GetFileName(fileName));
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting picture: {fileName}.", ex.Message);
            }

            return false;
        }

    }
}
