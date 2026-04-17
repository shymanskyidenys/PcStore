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
        var products = await _productRepo.GetListAsync();
        return View(products);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var categories = await _productRepo.GetCategoriesAsync();

        var viewModel = new ProductFormViewModel
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
    public async Task<IActionResult> Create(ProductFormViewModel vm)
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

        bool success = await _productRepo.SaveAsync(product);
        if (success)
        {
            TempData["Message"] = "The product has been added successfully!";
        }

        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var product = await _productRepo.GetByIdAsync(id);

        if (product == null)
        {
            return RedirectToAction("Index");
        }

        var categories = await _productRepo.GetCategoriesAsync();
        var attributes = await _productRepo.GetAttributesByCategoryIdAsync(product.CategoryId);

        var viewModel = new ProductFormViewModel
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

    [HttpPost]
    public async Task<IActionResult> Edit(ProductFormViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            var cats = await _productRepo.GetCategoriesAsync();
            vm.Categories = cats.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(), Text = c.Name
            });
            var attrs = await _productRepo.GetAttributesByCategoryIdAsync(vm.CategoryId);
            vm.Attributes = attrs.Select(a => new ProductFormAttributeViewModel
            {
                Id = a.Key.Id, Name = a.Key.Name,
                AttributeValues = a.Value.Select(v => new SelectListItem
                {
                    Value = v.ValueId.ToString(), Text = v.Value
                })
            });
            return View("ProductFrom", vm);
        }

        var product = new Product
        {
            ProductId = vm.ProductId,
            Name = vm.Name,
            Price = vm.Price,
            Description = vm.Description,
            CategoryId = vm.CategoryId,
            Attributes = vm.SelectedAtributes
        };

        bool success = await _productRepo.SaveAsync(product);
        if (success) TempData["Message"] = "Product updated successfully!";

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        bool deleted = await _productRepo.DeleteAsync(id);

        if (deleted)
        {
            TempData["Message"] = "Product deleted successfully!";
        }

        return RedirectToAction(nameof(Index));
    }
}
