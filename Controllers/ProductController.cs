using Microsoft.AspNetCore.Mvc;
using PcStoreApp.Repositories;
using PcStoreApp.Models;
using PcStoreApp.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PcStoreApp.Controllers;

public class ProductController : Controller
{
    private readonly ProductRepository _repo;

    public ProductController(ProductRepository repo) { _repo = repo; }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var products = await _repo.GetProductsAsync();
        return View(products);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var categories = await _repo.GetCategoriesAsync();
        
        var viewModel = new ProductCreateViewModel();
        viewModel.Categories = categories.Select(c => new SelectListItem
        {
            Value = c.CategoryId.ToString(),
            Text = c.Name
        }).ToList();

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductCreateViewModel vm)
    {
        if (!ModelState.IsValid) 
        {
            var categories = await _repo.GetCategoriesAsync();
            vm.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.CategoryId.ToString(),
                Text = c.Name
            }).ToList();

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
