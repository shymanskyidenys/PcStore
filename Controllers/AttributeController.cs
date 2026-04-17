using Microsoft.AspNetCore.Mvc;
using PcStoreApp.Repositories;
using PcStoreApp.ViewModels;

namespace PcStoreApp.Controllers;

public class AttributeController : Controller
{
    private readonly AttributesRepository _attributeRepo;

    public AttributeController(AttributesRepository attributesRepo)
    {
        _attributeRepo = attributesRepo;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var attributes = await _attributeRepo.GetAttributesListAsync();
        return View(attributes);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var viewModel = new AttributeFormViewModel();
        return View("AttributeForm", viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(AttributeFormViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View("AttributeForm", vm);
        }

        var attribute = new Models.Attribute
        {
            Name = vm.Name!
        };

        bool success = await _attributeRepo.SaveAttributeAsync(attribute);
        if (success)
        {
            TempData["Message"] = "Attribute added successfully!";
        }

        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var attribute = await _attributeRepo.GetAttributeByIdAsync(id);

        if (attribute == null)
        {
            return RedirectToAction("Index");
        }

        var vm = new AttributeFormViewModel
        {
            Id = attribute.Id,
            Name = attribute.Name
        };

        return View("AttributeForm", vm);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(AttributeFormViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View("CategoryForm", vm);
        }

        var attribute = new Models.Attribute
        {
            Id = vm.Id ?? 0,
            Name = vm.Name!
        };

        bool success = await _attributeRepo.SaveAttributeAsync(attribute);
        if (success)
        {
            TempData["Message"] = "Attribute saved successfully!";
        }

        return RedirectToAction("Index");
    }
}
