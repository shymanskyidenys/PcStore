using Microsoft.AspNetCore.Mvc.Rendering;
using PcStoreApp.Models;
using PcStoreApp.Repositories;
using PcStoreApp.ViewModels;

namespace PcStoreApp.Services;

public class AttributeService
{
    private readonly AttributesRepository _attributeRepo;

    public AttributeService(AttributesRepository attributeRepo)
    {
        _attributeRepo = attributeRepo;
    }

    public async Task<AttributesListViewModel> GetAttributeListAsync(int limit = 100, int offset = 0)
    {
        var attributes = await _attributeRepo.GetAttributesListAsync(limit, offset);

        var viewModel = new AttributesListViewModel
        {
            Attributes = attributes.Select(a => new AttributeListItemViewModel
            {
                Id = a.Id,
                Name = a.Name,
                ValuesCount = a.AttributeValueCount ?? 0
            }).ToList()
        };

        return viewModel;
    }

    public AttributeFormViewModel GetCreateFormData()
    {
        return new AttributeFormViewModel();
    }

    public async Task<AttributeFormViewModel?> GetEditFormDataAsync(int attributeId)
    {
        var attribute = await _attributeRepo.GetAttributeByIdAsync(attributeId);
        if (attribute == null)
        {
            return null;
        }

        return new AttributeFormViewModel
        {
            Id = attribute.Id,
            Name = attribute.Name
        };
    }

    public async Task<bool> SaveAttributeAsync(AttributeFormViewModel vm)
    {
        var attribute = new Models.Attribute
        {
            Id = vm.Id ?? 0,
            Name = vm.Name!
        };

        return await _attributeRepo.SaveAttributeAsync(attribute);
    }

    public async Task<bool> DeleteAttributeAsync(int attributeId)
    {
        return await _attributeRepo.DeleteAttributeAsync(attributeId);
    }

    public async Task<AttributeValuesListViewModel> GetAttributeValueListAsync(int attributeId)
    {
        var attribute = await _attributeRepo.GetAttributeByIdAsync(attributeId);
        var values = await _attributeRepo.GetAttributeValuesByAttributeIdAsync(attributeId);

        var viewModel = new AttributeValuesListViewModel
        {
            AttributeId = attributeId,
            AttributeName = attribute?.Name ?? "",
            Values = values.Select(v => new AttributeValueListItemViewModel
            {
                ValueId = v.ValueId,
                Value = v.Value
            }).ToList()
        };

        return viewModel;
    }

    public async Task<AttributeValueFormViewModel> GetCreateValueFormDataAsync(int attributeId)
    {
        var attributes = await _attributeRepo.GetAttributesListAsync();

        return new AttributeValueFormViewModel
        {
            AttributeId = attributeId,
            Attributes = attributes.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Name,
                Selected = a.Id == attributeId
            }).ToList()
        };
    }

    public async Task<AttributeValueFormViewModel?> GetEditValueFormDataAsync(int valueId)
    {
        var attributeValue = await _attributeRepo.GetAttributeValueByIdAsync(valueId);
        if (attributeValue == null)
        {
            return null;
        }

        var attributes = await _attributeRepo.GetAttributesListAsync();

        return new AttributeValueFormViewModel
        {
            ValueId = attributeValue.ValueId,
            AttributeId = attributeValue.AttributeId,
            Value = attributeValue.Value,
            Attributes = attributes.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Name,
                Selected = a.Id == attributeValue.AttributeId
            }).ToList()
        };
    }

    public async Task<bool> SaveAttributeValueAsync(AttributeValueFormViewModel vm)
    {
        var attributeValue = new AttributeValue
        {
            ValueId = vm.ValueId ?? 0,
            AttributeId = vm.AttributeId,
            Value = vm.Value!
        };

        return await _attributeRepo.SaveAttributeValueAsync(attributeValue);
    }

    public async Task<bool> DeleteAttributeValueAsync(int valueId)
    {
        return await _attributeRepo.DeleteAttributeValueAsync(valueId);
    }
}
