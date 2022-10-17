#nullable disable
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant.Areas.Admin.ViewModels;
using Restaurant.Data;
using Restaurant.Models;

namespace Restaurant.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public ReviewsController(ApplicationDbContext db,
            IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _db.Reviews
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return View(_mapper.Map<IEnumerable<Review>, IEnumerable<ReviewViewModel>>(items));
        }

        public async Task<IActionResult> Details(int? id)
        {
            var review = await _db.Reviews.FindAsync(id);
            if (review == null)
                return NotFound();

            return View(_mapper.Map<ReviewViewModel>(review));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReviewViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newReview = new Review()
                {
                    Text = model.Text,
                    ReviewerName = model.ReviewerName,
                    Rating = model.Rating,
                    CreatedAt = DateTime.Now,
                    IsPublished = model.IsPublished
                };

                _db.Reviews.Add(newReview);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var review = await _db.Reviews.FindAsync(id);
            if (review == null)
                return NotFound();

            return View(_mapper.Map<ReviewViewModel>(review));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ReviewViewModel model)
        {
            if (ModelState.IsValid)
            {
                var reviewToEdit = await _db.Reviews.FindAsync(id);
                if (reviewToEdit == null)
                    return NotFound();

                reviewToEdit.Text = model.Text;
                reviewToEdit.ReviewerName = model.ReviewerName;
                reviewToEdit.TextEn = model.TextEn;
                reviewToEdit.ReviewerNameEn = model.ReviewerNameEn;
                reviewToEdit.Rating = model.Rating;
                reviewToEdit.IsPublished = model.IsPublished;

                _db.Reviews.Update(reviewToEdit);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var review = await _db.Reviews.FindAsync(id);
            if (review == null)
                return NotFound();

            _db.Reviews.Remove(review);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost("Admin/Reviews/{id}/Publish")]
        public async Task<IActionResult> Publish(int id)
        {
            var review = await _db.Reviews.FindAsync(id);
            if (review == null)
                return NotFound();

            review.IsPublished = true;

            _db.Reviews.Update(review);
            await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("Admin/Reviews/{id}/Unpublish")]
        public async Task<IActionResult> Unpublish(int id)
        {
            var review = await _db.Reviews.FindAsync(id);
            if (review == null)
                return NotFound();

            review.IsPublished = false;

            _db.Reviews.Update(review);
            await _db.SaveChangesAsync();

            return Ok();
        }
    }
}
