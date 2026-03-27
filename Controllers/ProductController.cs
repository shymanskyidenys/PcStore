using Microsoft.AspNetCore.Mvc;
using PcStoreApp.Repositories;
using PcStoreApp.Models;
using PcStoreApp.ViewModels;

namespace PcStoreApp.Controllers;

public class ProductController : Controller
{
    private readonly ProductRepository _repo;

    public ProductController(ProductRepository repo) { _repo = repo; }

    public async Task<IActionResult> Index()
    {
        var products = await _repo.GetAllProductsAsync();
        return View(products);
    }

    public async Task<IActionResult> Create()
    {
        var viewModel = new ProductCreateViewModel
        {
            Categories = await _repo.GetCategoriesForSelectAsync()
        };
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductCreateViewModel vm)
    {
        if (!ModelState.IsValid) 
        {
            vm.Categories = await _repo.GetCategoriesForSelectAsync();
            return View(vm);
        }

        var product = new Product {
            Name = vm.Name,
            Price = vm.Price,
            Description = vm.Description,
            CategoryId = vm.CategoryId
        };

        await _repo.AddProductAsync(product);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _repo.DeleteProductAsync(id);

        if (deleted)
        {
            TempData["Message"] = "Product deleted successfully!";
        }

        return RedirectToAction(nameof(Index));
    }
}
