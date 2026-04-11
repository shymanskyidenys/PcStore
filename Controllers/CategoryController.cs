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

        await _categoryRepo.AddAsync(category);
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
            Name = category.Name
        };

        return View("CategoryForm", vm);
    }

    [HttpPut]
    public async Task<IActionResult> Update(CategoryFormViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View("CategoryForm", vm);
        }

        var category = new Category
        {
            Name = vm.Name!
        };
        await _categoryRepo.AddAsync(category);
        return RedirectToAction("Index");
    }

    [HttpDelete]
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
