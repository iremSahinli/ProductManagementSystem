using ManagmentSystem.Business.DTOs.CategoryDTOs;
using ManagmentSystem.Business.Services.CategoryServices;
using ManagmentSystem.Business.Services.ProductServices;
using ManagmentSystem.Domain.Entities;
using ManagmentSystem.Domain.Utilities.Concretes;
using ManagmentSystem.Presentation.Areas.Admin.Models.CategoryVMs;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace ManagmentSystem.Presentation.Areas.Admin.Controllers
{
    public class CategoryController : AdminBaseController
    {

        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;

        public CategoryController(ICategoryService categoryService, IProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }


        public async Task<IActionResult> Index()
        {
            var result = await _categoryService.GetAllAsync();
            if (!result.IsSucces)
            {
                ErrorNotyf("Failed");
                return View(result.Data.Adapt<List<AdminCategoryListVM>>());
            }
            SuccesNotyf("Categories Listed");
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
                ErrorNotyf("Lütfen formu doğru şekilde doldurun.");
                return View(model);
            }

            var categoryExist = await _categoryService.IsCategoryNameExistAsync(model.CategoryName, model.Description); //Aynı kategori ve description sistemde varmı kontrol eder.
            if (categoryExist)
            {
                ErrorNotyf("Kaydetmek istedğiniz kategori sistemde bulunmaktadır, Lütfen farklı bir kategori ekleyiniz");
                return View(model);
            }



            var result = await _categoryService.AddAsync(model.Adapt<CategoryCreateDTO>());
            if (!result.IsSucces)
            {

                ErrorNotyf(result.Message); // Mesaj: "Kategori Sistemde Mevcut"
                return View(model);
            }

            SuccesNotyf("Kategori başarıyla eklendi.");
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var isCategoryUsed = await _productService.IsCategoryUsedAsync(id); //Product tablosunda kullanılıyor mu kontrol eder.
            if (isCategoryUsed)
            {
                ErrorNotyf("Silmek İstediğiniz Kategori Üründe Kullanılıyor, Silinemez ");
                return RedirectToAction("Index");
            }

            var result = await _categoryService.DeleteAsync(id);
            if (!result.IsSucces)
            {
                ErrorNotyf("Failed");
                return RedirectToAction("Index");
            }
            SuccesNotyf("Deleted is successfully");
            return RedirectToAction("Index");

        }




        public async Task<IActionResult> Update(Guid id)
        {
            var result = await _categoryService.GetByIdAsync(id);
            if (!result.IsSucces)
            {
                ErrorNotyf(result.Message);
                return RedirectToAction("Index");
            }
            SuccesNotyf("Category updating page.");
            return View(result.Data.Adapt<AdminCategoryUpdateVM>());
        }


        [HttpPost]
        public async Task<IActionResult> Update(AdminCategoryUpdateVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //Kategori sistemde varmı kontrol ediyor ve model dönüyor.
            var categoryExist = await _categoryService.IsCategoryNameExistAsync(model.CategoryName, model.Description);
            if (categoryExist)
            {
                ErrorNotyf("A category with this name or description already exists.");
                return View(model);
            }


            var result = await _categoryService.UpdateAsync(model.Adapt<CategoryUpdateDTO>());
            if (!result.IsSucces)
            {
                ErrorNotyf("Category update failed.");
                return View(model);
            }
            SuccesNotyf("Category updated successfully.");
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _categoryService.GetByIdAsync(id);
            if (!result.IsSucces)
            {
                ErrorNotyf("Category Detail Page Loaded Successfully");
                return RedirectToAction("Index");
            }

            SuccesNotyf("Category Detail Page Loaded Successfully");
            return View(result.Data.Adapt<AdminCategoryDetailVM>());

        }




    }
}



