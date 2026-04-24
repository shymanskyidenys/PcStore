using Microsoft.AspNetCore.Mvc.Rendering;
using PcStoreApp.Models;
using PcStoreApp.Repositories;
using PcStoreApp.ViewModels;

namespace PcStoreApp.Services;

public class ProductService
{
    private readonly ProductRepository _productRepo;
    private readonly CategoryRepository _categoryRepo;
    private readonly AttributesRepository _attributeRepo;

    public ProductService(
        ProductRepository productRepo,
        CategoryRepository categoryRepo,
        AttributesRepository attributeRepo)
    {
        _productRepo = productRepo;
        _categoryRepo = categoryRepo;
        _attributeRepo = attributeRepo;
    }

    public async Task<ProductListViewModel> GetProductListAsync(int limit = 100, int offset = 0)
    {
        var products = await _productRepo.GetListAsync(limit, offset);

        var viewModel = new ProductListViewModel
        {
            Products = products.Select(p => new ProductListItemViewModel
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                CategoryName = p.CategoryName
            }).ToList(),
            TotalCount = products.Count()
        };

        return viewModel;
    }

    public async Task<ProductFormViewModel> GetCreateFormDataAsync()
    {
        var categories = await _categoryRepo.GetListAsync();

        return new ProductFormViewModel
        {
            Categories = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList()
        };
    }

    public async Task<ProductFormViewModel?> GetEditFormDataAsync(int productId)
    {
        var product = await _productRepo.GetByIdAsync(productId);
        if (product == null)
        {
            return null;
        }

        var categories = await _categoryRepo.GetListAsync();
        var attributes = await _attributeRepo.GetByCategoryIdAsync(product.CategoryId);

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
            }).ToList(),
            Attributes = attributes.Select(a => new ProductFormAttributeViewModel
            {
                Id = a.Key.Id,
                Name = a.Key.Name,
                AttributeValues = a.Value.Select(v => new SelectListItem
                {
                    Value = v.ValueId.ToString(),
                    Text = v.Value
                }).ToList()
            }).ToList()
        };

        return viewModel;
    }

    public async Task<ProductFormViewModel> RefreshFormDataAsync(ProductFormViewModel vm)
    {
        var categories = await _categoryRepo.GetListAsync();
        vm.Categories = categories.Select(c => new SelectListItem
        {
            Value = c.Id.ToString(),
            Text = c.Name
        }).ToList();

        if (vm.CategoryId > 0)
        {
            var attributes = await _attributeRepo.GetByCategoryIdAsync(vm.CategoryId);
            vm.Attributes = attributes.Select(a => new ProductFormAttributeViewModel
            {
                Id = a.Key.Id,
                Name = a.Key.Name,
                AttributeValues = a.Value.Select(v => new SelectListItem
                {
                    Value = v.ValueId.ToString(),
                    Text = v.Value
                }).ToList()
            }).ToList();
        }

        return vm;
    }

    public async Task<bool> SaveProductAsync(ProductFormViewModel vm)
    {
        // Маппинг ViewModel → Model
        var product = new Product
        {
            ProductId = vm.ProductId,
            Name = vm.Name,
            Price = vm.Price,
            Description = vm.Description,
            CategoryId = vm.CategoryId,
            Attributes = vm.SelectedAtributes
        };

        return await _productRepo.SaveAsync(product);
    }

    public async Task<bool> DeleteProductAsync(int productId)
    {
        return await _productRepo.DeleteAsync(productId);
    }
}
