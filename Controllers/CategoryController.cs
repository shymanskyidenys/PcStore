using Microsoft.AspNetCore.Mvc;
using PcStoreApp.Services;
using PcStoreApp.ViewModels;

namespace PcStoreApp.Controllers;

public class CategoryController : Controller
{
    private readonly CategoryService _categoryService;

    public CategoryController(CategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var viewModel = await _categoryService.GetCategoryListAsync();
        return View(viewModel);
    }

    [HttpGet]
    public IActionResult Create()
    {
        var viewModel = _categoryService.GetCreateFormData();
        return View("CategoryForm", viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CategoryFormViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View("CategoryForm", vm);
        }

        bool success = await _categoryService.SaveCategoryAsync(vm);
        if (success)
        {
            TempData["Message"] = "Category added successfully!";
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var viewModel = await _categoryService.GetEditFormDataAsync(id);
        if (viewModel == null)
        {
            return RedirectToAction(nameof(Index));
        }

        return View("CategoryForm", viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(CategoryFormViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View("CategoryForm", vm);
        }

        bool success = await _categoryService.SaveCategoryAsync(vm);
        if (success)
        {
            TempData["Message"] = "Category saved successfully!";
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        bool deleted = await _categoryService.DeleteCategoryAsync(id);
        if (deleted)
        {
            TempData["Message"] = "Category deleted successfully!";
        }

        return RedirectToAction(nameof(Index));
    }
}
