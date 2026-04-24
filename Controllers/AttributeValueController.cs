using Microsoft.AspNetCore.Mvc;
using PcStoreApp.Services;
using PcStoreApp.ViewModels;

namespace PcStoreApp.Controllers;

public class AttributeValueController : Controller
{
    private readonly AttributeService _attributeService;

    public AttributeValueController(AttributeService attributeService)
    {
        _attributeService = attributeService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int attributeId)
    {
        var viewModel = await _attributeService.GetAttributeValueListAsync(attributeId);
        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Create(int attributeId)
    {
        var viewModel = await _attributeService.GetCreateValueFormDataAsync(attributeId);
        return View("AttributeValueForm", viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(AttributeValueFormViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            vm = await _attributeService.GetCreateValueFormDataAsync(vm.AttributeId);
            return View("AttributeValueForm", vm);
        }

        bool success = await _attributeService.SaveAttributeValueAsync(vm);
        if (success)
        {
            TempData["Message"] = "Attribute value added successfully!";
        }

        return RedirectToAction(nameof(Index), new { attributeId = vm.AttributeId });
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var viewModel = await _attributeService.GetEditValueFormDataAsync(id);
        if (viewModel == null)
        {
            return RedirectToAction(nameof(Index), new { attributeId = 0 });
        }

        return View("AttributeValueForm", viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(AttributeValueFormViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            var refreshedVm = await _attributeService.GetEditValueFormDataAsync(vm.ValueId ?? 0);
            if (refreshedVm == null)
            {
                return RedirectToAction(nameof(Index), new { attributeId = vm.AttributeId });
            }
            return View("AttributeValueForm", refreshedVm);
        }

        bool success = await _attributeService.SaveAttributeValueAsync(vm);
        if (success)
        {
            TempData["Message"] = "Attribute value saved successfully!";
        }

        return RedirectToAction(nameof(Index), new { attributeId = vm.AttributeId });
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id, int attributeId)
    {
        bool deleted = await _attributeService.DeleteAttributeValueAsync(id);
        if (deleted)
        {
            TempData["Message"] = "Attribute value deleted successfully!";
        }

        return RedirectToAction(nameof(Index), new { attributeId });
    }
}
