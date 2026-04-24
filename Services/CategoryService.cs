using PcStoreApp.Models;
using PcStoreApp.Repositories;
using PcStoreApp.ViewModels;

namespace PcStoreApp.Services;

public class CategoryService
{
    private readonly CategoryRepository _categoryRepo;

    public CategoryService(CategoryRepository categoryRepo)
    {
        _categoryRepo = categoryRepo;
    }

    public async Task<CategoryListViewModel> GetCategoryListAsync(int limit = 100, int offset = 0)
    {
        var categories = await _categoryRepo.GetListAsync(limit, offset);

        var viewModel = new CategoryListViewModel
        {
            Categories = categories.Select(c => new CategoryListItemViewModel
            {
                Id = c.Id,
                Name = c.Name,
                ProductCount = c.ProductCount ?? 0
            }).ToList(),
            TotalCount = categories.Count
        };

        return viewModel;
    }

    public CategoryFormViewModel GetCreateFormData()
    {
        return new CategoryFormViewModel();
    }

    public async Task<CategoryFormViewModel?> GetEditFormDataAsync(int categoryId)
    {
        var category = await _categoryRepo.GetByIdAsync(categoryId);
        if (category == null)
        {
            return null;
        }

        return new CategoryFormViewModel
        {
            Id = category.Id,
            Name = category.Name
        };
    }

    public async Task<bool> SaveCategoryAsync(CategoryFormViewModel vm)
    {
        var category = new Category
        {
            Id = vm.Id ?? 0,
            Name = vm.Name!
        };

        if (vm.IsEdit)
        {
            return await _categoryRepo.UpdateAsync(category);
        }
        else
        {
            return await _categoryRepo.AddAsync(category);
        }
    }

    public async Task<bool> DeleteCategoryAsync(int categoryId)
    {
        return await _categoryRepo.DeleteCategoryAsync(categoryId);
    }
}
