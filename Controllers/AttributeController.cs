using Microsoft.AspNetCore.Mvc;
using PcStoreApp.Services;
using PcStoreApp.ViewModels;

namespace PcStoreApp.Controllers;

public class AttributeController : Controller
{
    private readonly AttributeService _attributeService;

    public AttributeController(AttributeService attributeService)
    {
        _attributeService = attributeService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var viewModel = await _attributeService.GetAttributeListAsync();
        return View(viewModel);
    }

    [HttpGet]
    public IActionResult Create()
    {
        var viewModel = _attributeService.GetCreateFormData();
        return View("AttributeForm", viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(AttributeFormViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View("AttributeForm", vm);
        }

        bool success = await _attributeService.SaveAttributeAsync(vm);
        if (success)
        {
            TempData["Message"] = "Attribute added successfully!";
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var viewModel = await _attributeService.GetEditFormDataAsync(id);
        if (viewModel == null)
        {
            return RedirectToAction(nameof(Index));
        }

        return View("AttributeForm", viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(AttributeFormViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View("AttributeForm", vm);
        }

        bool success = await _attributeService.SaveAttributeAsync(vm);
        if (success)
        {
            TempData["Message"] = "Attribute saved successfully!";
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        bool deleted = await _attributeService.DeleteAttributeAsync(id);
        if (deleted)
        {
            TempData["Message"] = "Attribute deleted successfully!";
        }

        return RedirectToAction(nameof(Index));
    }
}
