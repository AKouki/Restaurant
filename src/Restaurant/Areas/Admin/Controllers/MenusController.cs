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
    public class MenusController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public MenusController(ApplicationDbContext db,
            IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _db.Menus
                .OrderBy(m => m.OrderLevel)
                .Select(m => new MenuViewModel()
                {
                    Id = m.Id,
                    Name = m.Name,
                    IsPublished = m.IsPublished,
                    OrderLevel = m.OrderLevel,
                    MenuItemsCount = m.MenuItems.Count()
                }).ToListAsync();

            return View(items);
        }

        public async Task<IActionResult> Details(int? id)
        {
            var menu = await _db.Menus
                .Where(m => m.Id == id)
                .Include(m => m.MenuItems)
                .FirstOrDefaultAsync();

            if (menu == null)
                return NotFound();

            return View(_mapper.Map<MenuViewModel>(menu));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MenuViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newMenu = new Menu()
                {
                    Name = model.Name,
                    IsPublished = model.IsPublished
                };

                _db.Add(newMenu);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var menu = await _db.Menus.FindAsync(id);
            if (menu == null)
                return NotFound();

            return View(_mapper.Map<MenuViewModel>(menu));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MenuViewModel model)
        {
            if (ModelState.IsValid)
            {
                var menuToEdit = await _db.Menus.FindAsync(id);
                if (menuToEdit == null)
                    return NotFound();

                menuToEdit.Name = model.Name;
                menuToEdit.NameEn = model.NameEn;
                menuToEdit.IsPublished = model.IsPublished;

                _db.Update(menuToEdit);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var menu = await _db.Menus.FindAsync(id);
            if (menu == null)
                return NotFound();

            _db.Menus.Remove(menu);
            await _db.SaveChangesAsync();

            //return RedirectToAction(nameof(Index));
            return Ok();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOrderLevel(IEnumerable<OrderLevelViewModel> data)
        {
            var ids = data.Select(s => s.ItemId).ToList();

            var itemsToUpdate = await _db.Menus
                .Where(m => ids.Contains(m.Id))
                .ToListAsync();

            foreach (var item in itemsToUpdate)
            {
                item.OrderLevel = data.FirstOrDefault(m => m.ItemId == item.Id).ItemLevel;
            }

            _db.Menus.UpdateRange(itemsToUpdate);
            await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateMenuItemOrderLevel(IEnumerable<OrderLevelViewModel> data)
        {
            var ids = data.Select(s => s.ItemId).ToList();

            var itemsToUpdate = await _db.MenuItems
                .Where(m => ids.Contains(m.Id))
                .ToListAsync();

            foreach (var item in itemsToUpdate)
            {
                item.OrderLevel = data.FirstOrDefault(m => m.ItemId == item.Id).ItemLevel;
            }

            _db.MenuItems.UpdateRange(itemsToUpdate);
            await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("Admin/Menus/{id}/Publish")]
        public async Task<IActionResult> Publish(int id)
        {
            var menu = await _db.Menus.FindAsync(id);
            if (menu == null)
                return NotFound();

            menu.IsPublished = true;

            _db.Menus.Update(menu);
            await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("Admin/Menus/{id}/Unpublish")]
        public async Task<IActionResult> Unpublish(int id)
        {
            var menu = await _db.Menus.FindAsync(id);
            if (menu == null)
                return NotFound();

            menu.IsPublished = false;

            _db.Menus.Update(menu);
            await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("Admin/Menus/ViewMenuItemDetails/{id}")]
        public async Task<IActionResult> ViewMenuItemDetails(int id)
        {
            var menuItem = await _db.MenuItems.FindAsync(id);
            if (menuItem == null)
                return NotFound();

            return Ok(_mapper.Map<MenuItemViewModel>(menuItem));
        }

        [HttpGet("Admin/Menus/{id}/AddMenuItem")]
        public IActionResult AddMenuItem(int id)
        {
            var model = new MenuItemViewModel()
            {
                MenuId = id
            };

            return PartialView("_AddMenuItem", model);
        }

        [HttpPost("Admin/Menus/{id}/AddMenuItem")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMenuItem(int id, MenuItemViewModel model)
        {
            var exists = await _db.Menus.AnyAsync(m => m.Id == id);
            if (!exists)
                return NotFound();

            var newMenuItem = new MenuItem()
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                MenuId = id
            };

            _db.MenuItems.Add(newMenuItem);
            await _db.SaveChangesAsync();

            return Ok(_mapper.Map<MenuItem, MenuItemViewModel>(newMenuItem));
        }

        [HttpGet("Admin/Menus/EditMenuItem/{id}")]
        public async Task<IActionResult> EditMenuItem(int id)
        {
            var menuItem = await _db.MenuItems.FindAsync(id);
            if (menuItem == null)
                return NotFound();

            return PartialView("_EditMenuItem", _mapper.Map<MenuItem, MenuItemViewModel>(menuItem));
        }

        [HttpPost("Admin/Menus/EditMenuItem/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMenuItem(int id, MenuItemViewModel model)
        {
            var menuItem = await _db.MenuItems.FindAsync(id);
            if (menuItem == null)
                return NotFound();

            menuItem.Name = model.Name;
            menuItem.Description = model.Description;
            menuItem.NameEn = model.NameEn;
            menuItem.DescriptionEn = model.DescriptionEn;
            menuItem.Price = model.Price;

            _db.MenuItems.Update(menuItem);
            await _db.SaveChangesAsync();

            return Ok(_mapper.Map<MenuItem, MenuItemViewModel>(menuItem));
        }

        [HttpPost("Admin/Menus/DeleteMenuItem/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMenuItem(int id)
        {
            var menuItem = await _db.MenuItems.FindAsync(id);
            if (menuItem == null)
                return NotFound();

            _db.MenuItems.Remove(menuItem);
            await _db.SaveChangesAsync();

            //return RedirectToAction(nameof(Index));
            return Ok();
        }
    }
}
