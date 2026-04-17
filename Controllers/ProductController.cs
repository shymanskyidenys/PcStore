using Microsoft.AspNetCore.Mvc;
using PcStoreApp.Services;
using PcStoreApp.ViewModels;

namespace PcStoreApp.Controllers;

public class ProductController : Controller
{
    private readonly ProductService _productService;

    public ProductController(ProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var viewModel = await _productService.GetProductListAsync();
        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var viewModel = await _productService.GetCreateFormDataAsync();
        return View("ProductForm", viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductFormViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            vm = await _productService.RefreshFormDataAsync(vm);
            return View("ProductForm", vm);
        }

        bool success = await _productService.SaveProductAsync(vm);
        if (success)
        {
            TempData["Message"] = "The product has been added successfully!";
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var viewModel = await _productService.GetEditFormDataAsync(id);
        if (viewModel == null)
        {
            return RedirectToAction(nameof(Index));
        }

        return View("ProductForm", viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ProductFormViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            vm = await _productService.RefreshFormDataAsync(vm);
            return View("ProductForm", vm);
        }

        bool success = await _productService.SaveProductAsync(vm);
        if (success)
        {
            TempData["Message"] = "Product updated successfully!";
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        bool deleted = await _productService.DeleteProductAsync(id);
        if (deleted)
        {
            TempData["Message"] = "Product deleted successfully!";
        }

        return RedirectToAction(nameof(Index));
    }
}
