using ManagmentSystem.Business.DTOs.CategoryDTOs;
using ManagmentSystem.Business.Services.CategoryServices;
using ManagmentSystem.Domain.Utilities.Concretes;
using ManagmentSystem.Presentation.Areas.Admin.Models.CategoryVMs;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace ManagmentSystem.Presentation.Areas.Admin.Controllers
{
    public class CategoryController : AdminBaseController
    {

        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }


        public async Task<IActionResult> Index()
        {
            var result = await _categoryService.GetAllAsync();
            if (!result.IsSucces)
            {
                Console.Out.WriteLineAsync(result.Message);
                return View(result.Data.Adapt<List<AdminCategoryListVM>>());
            }
            Console.Out.WriteLineAsync(result.Message);
            return View(result.Data.Adapt<List<AdminCategoryListVM>>());
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(AdminCategoryCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);

            }
            var result = await _categoryService.AddAsync(model.Adapt<CategoryCreateDTO>());
            if (!result.IsSucces)
            {
                Console.Out.WriteLineAsync(result.Message);
                return View(model);
            }
            Console.Out.WriteLineAsync(result.Message);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _categoryService.DeleteAsync(id);
            if (!result.IsSucces)
            {
                Console.Out.WriteLineAsync(result.Message);
                return RedirectToAction("Index");
            }
            Console.Out.WriteLineAsync(result.Message);
            return RedirectToAction("Index");

        }


        public async Task<IActionResult> Update(Guid id)
        {
            var result = await _categoryService.GetByIdAsync(id);
            if (!result.IsSucces)
            {
                Console.Out.WriteLineAsync(result.Message);
                return RedirectToAction("Index");
            }
            return View(result.Data.Adapt<AdminCategoryUpdateVM>());
        }


        [HttpPost]
        public async Task<IActionResult> Update(AdminCategoryUpdateVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await _categoryService.UpdateAsync(model.Adapt<CategoryUpdateDTO>());
            if (!result.IsSucces)
            {
                Console.Out.WriteLineAsync(result.Message);
                return View(model);
            }
            Console.Out.WriteLineAsync(result.Message);
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _categoryService.GetByIdAsync(id);
            if (!result.IsSucces)
            {
                Console.Out.WriteLineAsync(result.Message);
                return RedirectToAction("Index");
            }
            return View(result.Data.Adapt<AdminCategoryDetailVM>());

        }

    }
}


