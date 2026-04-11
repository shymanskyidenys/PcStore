using Microsoft.AspNetCore.Mvc;
using PcStoreApp.Models;
using PcStoreApp.Repositories;
using PcStoreApp.ViewModels;

public class CategoryController : Controller
{
    private readonly CategoryRepository _categoryRepo;

    public CategoryController(CategoryRepository repo)
    {
        _categoryRepo = repo;
    }

    public async Task<ActionResult> Index()
    {
        var categories = await _categoryRepo.GetListAsync();
        return View(categories);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var viewModel = new CategoryFormViewModel();
        return View("CategoryForm", viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CategoryFormViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View("CategoryForm", vm);
        }

        var category = new Category
        {
            Name = vm.Name!
        };

        bool success = await _categoryRepo.AddAsync(category);
        if (success)
        {
            TempData["Message"] = "Category added successfully!";
        }

        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var category = await _categoryRepo.GetByIdAsync(id);

        if (category == null)
        {
            return RedirectToAction("Index");
        }

        var vm = new CategoryFormViewModel
        {
            Id = id,
            Name = category.Name
        };

        return View("CategoryForm", vm);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(CategoryFormViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View("CategoryForm", vm);
        }

        var category = new Category
        {
            Id = vm.Id ?? 0,
            Name = vm.Name!
        };

        bool success = await _categoryRepo.UpdateAsync(category);
        if (success)
        {
            TempData["Message"] = "Category saved successfully!";
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        bool deleted = await _categoryRepo.DeleteCategoryAsync(id);

        if (deleted)
        {
            TempData["Message"] = "Category deleted successfully!";
        }

        return RedirectToAction(nameof(Index));
    }
}
