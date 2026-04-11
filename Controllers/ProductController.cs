using Microsoft.AspNetCore.Mvc;
using PcStoreApp.Repositories;
using PcStoreApp.Models;
using PcStoreApp.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PcStoreApp.Controllers;

public class ProductController : Controller
{
    private readonly ProductRepository _productRepo;

    public ProductController(ProductRepository repo) { _productRepo = repo; }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var products = await _productRepo.GetProductsAsync();
        return View(products);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var categories = await _productRepo.GetCategoriesAsync();

        var viewModel = new ProductFromViewModel
        {
            Categories = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList()
        };

        return View("ProductFrom", viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductFromViewModel vm)
    {
        if (!ModelState.IsValid) 
        {
            var categories = await _productRepo.GetCategoriesAsync();
            vm.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            return View("ProductFrom", vm);
        }

        var product = new Product
        {
            Name = vm.Name,
            Price = vm.Price,
            Description = vm.Description,
            CategoryId = vm.CategoryId
        };

        await _productRepo.AddProductAsync(product);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var product = await _productRepo.GetProductByIdAsync(id);

        if (product == null)
        {
            return RedirectToAction("Index");
        }

        var categories = await _productRepo.GetCategoriesAsync();
        var attributes = await _productRepo.GetAttributesByCategoryIdAsync(product.CategoryId);

        var viewModel = new ProductFromViewModel
        {
            ProductId = product.ProductId,
            Name = product.Name,
            Price = product.Price,
            Description = product.Description,
            CategoryId = product.CategoryId,
            SelectedAtributes = product.Attributes,
            Categories = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }),
            Attributes = attributes.Select(a => new ProductFormAttributeViewModel
            {
                Id = a.Key.Id,
                Name = a.Key.Name,
                AttributeValues = a.Value.Select(v => new SelectListItem
                {
                    Value = v.ValueId.ToString(),
                    Text = v.Value
                })
            })
        };

        return View("ProductFrom", viewModel);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        bool deleted = await _productRepo.DeleteProductAsync(id);

        if (deleted)
        {
            TempData["Message"] = "Product deleted successfully!";
        }

        return RedirectToAction(nameof(Index));
    }
}
