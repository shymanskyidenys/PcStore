using Microsoft.AspNetCore.Mvc;
using PcStoreApp.Models;
using PcStoreApp.Repositories;
using PcStoreApp.ViewModels.Attribute;

public class AttributeValueController : Controller
{
    private readonly AttributesRepository _attributeRepo;

    public AttributeValueController(AttributesRepository attributesRepo)
    {
        _attributeRepo = attributesRepo;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int attributeId)
    {
        var attributeValues = await _attributeRepo.GetAttributeValuesByAttributeIdAsync(attributeId);
        return View(attributeValues);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var vm = new AttributeValueFormViewModel();
        return View("AttributeForm", vm);
    }

    [HttpPost]
    public async Task<IActionResult> Create(AttributeValueFormViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View("AttributeForm", vm);
        }

        var attributeValue = new AttributeValue
        {
            AttributeId = vm.AttributeId,
            Value = vm.Value!
        };

        bool success = await _attributeRepo.SaveAttributeValueAsync(attributeValue);
        if (success)
        {
            TempData["Message"] = "Attribute value added successfully!";
        }

        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var attributeValue = await _attributeRepo.GetAttributeValueByIdAsync(id);

        if (attributeValue == null)
        {
            return RedirectToAction("Index");
        }

        var vm = new AttributeValueFormViewModel
        {
            ValueId = attributeValue.ValueId,
            AttributeId = attributeValue.AttributeId,
            Value = attributeValue.Value
        };

        return View("AttributeForm", vm);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(AttributeValueFormViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View("CategoryForm", vm);
        }

        var attributeValue = new AttributeValue
        {
            AttributeId = vm.AttributeId,
            Value = vm.Value!
        };

        bool success = await _attributeRepo.SaveAttributeValueAsync(attributeValue);
        if (success)
        {
            TempData["Message"] = "Attribute value saved successfully!";
        }

        return RedirectToAction("Index");
    }
}
