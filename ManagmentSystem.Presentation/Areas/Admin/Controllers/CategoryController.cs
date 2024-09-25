using ManagmentSystem.Business.DTOs.CategoryDTOs;
using ManagmentSystem.Business.Services.CategoryServices;
using ManagmentSystem.Business.Services.ProductServices;
using ManagmentSystem.Domain.Entities;
using ManagmentSystem.Domain.Utilities.Concretes;
using ManagmentSystem.Presentation.Areas.Admin.Models.CategoryVMs;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;

namespace ManagmentSystem.Presentation.Areas.Admin.Controllers
{
    public class CategoryController : AdminBaseController
    {

        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;

        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        public CategoryController(ICategoryService categoryService, IProductService productService, IStringLocalizer<SharedResources> stringLocalizer)
        {
            _categoryService = categoryService;
            _productService = productService;
            _stringLocalizer = stringLocalizer;
        }


        public async Task<IActionResult> Index()
        {
            var result = await _categoryService.GetAllAsync();
            if (!result.IsSucces)
            {
                var message = _stringLocalizer["Failed"];
                ErrorNotyf(message);
                return View(result.Data.Adapt<List<AdminCategoryListVM>>());
            }

            var categories = result.Data.Adapt<List<AdminCategoryListVM>>();

            // Alt kategoriler için ana kategoriyi bul ve ParentCategoryName'e ata
            foreach (var category in categories)
            {
                Console.WriteLine($"Category: {category.CategoryName}, ParentCategoryId: {category.ParentCategoryId}");

                if (category.ParentCategoryId != null)
                {
                    var parentCategory = categories.FirstOrDefault(c => c.Id == category.ParentCategoryId);
                    if (parentCategory != null)
                    {
                        category.ParentCategoryName = parentCategory.CategoryName;
                        Console.WriteLine($"Parent Category Found: {parentCategory.CategoryName} for {category.CategoryName}");
                    }
                    else
                    {
                        Console.WriteLine($"Parent Category not found for {category.CategoryName}");
                    }
                }
            }
            SuccesNotyf(_stringLocalizer["Category Listing Success"]);
            return View(categories);
        }




        public async Task<IActionResult> Create()
        {
            
            var categories = await _categoryService.GetAllCategoriesAsync(); 
            var parentCategories = categories.Where(c => c.ParentCategoryId == null).ToList(); 

            ViewBag.Categories = new SelectList(parentCategories, "Id", "CategoryName"); 

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AdminCategoryCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                var message = _stringLocalizer["Please fill out the form correctly."];
                ErrorNotyf(message);
                return View(model);
            }

            var categoryExist = await _categoryService.IsCategoryNameExistAsync(model.CategoryName);
            if (categoryExist)
            {
                var message2 = _stringLocalizer["The category you want to save is in the system, please add a different category."];
                ErrorNotyf(message2);
                return View(model);
            }

           
            var result = await _categoryService.AddAsync(new CategoryCreateDTO
            {
                CategoryName = model.CategoryName,
                Description = model.Description,
                ParentCategoryId = model.ParentCategoryId  
            });

            if (!result.IsSucces)
            {
                ErrorNotyf(result.Message);
                return View(model);
            }

            var message4 = _stringLocalizer["Category successfully added"];
            SuccesNotyf(message4);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(Guid id)
        {

            //Yapılacak işlem eğer categorinin sub kategorisi varsa silinmesin sub kategoriyi bulmak için ilgili kategorinin ıd si ile parentleri karşılaştır eğer bir veya 1den çok parentıd varsa statusuna veya statuslarına bak .statuslarınadan herhangi biri 1 ise bu kategori alt kategoride kullanılıyor de silme işleminden vazgeç

            var isSubCategoryUsed = await _categoryService.IsSubCategoryUsedAsync(id);
            if (isSubCategoryUsed)
            {
                var message = _stringLocalizer["The category you want to delete is used in the subcategory and cannot be deleted."];
                ErrorNotyf(message);
                return RedirectToAction("Index");
            }
            var isCategoryUsed = await _productService.IsCategoryUsedAsync(id); //Product tablosunda kullanılıyor mu kontrol eder.
            if (isCategoryUsed)
            {
                var message = _stringLocalizer["The category you want to delete is used in the product and cannot be deleted."];
                ErrorNotyf(message);
                return RedirectToAction("Index");
            }

            var result = await _categoryService.DeleteAsync(id);
            if (!result.IsSucces)
            {
                var message2 = _stringLocalizer["Failed"];
                ErrorNotyf(message2);
                return RedirectToAction("Index");
            }
            var message3 = _stringLocalizer["Deleted is successfully"];
            SuccesNotyf(message3);
            return RedirectToAction("Index");

        }




        public async Task<IActionResult> Update(Guid id)
        {
            var result = await _categoryService.GetByIdAsync(id);
            if (!result.IsSucces)
            {
                var message = _stringLocalizer["Failed"];
                ErrorNotyf(message);
                return RedirectToAction("Index");
            }

            // Sadece ana kategorileri çekiyoruz, ParentCategoryId'si null olanlar
            var categories = await _categoryService.GetAllCategoriesAsync();
            var parentCategories = categories.Where(c => c.ParentCategoryId == null).ToList();

            // ViewBag ile dropdown'da göstermek için ana kategorileri gönderiyoruz
            ViewBag.Categories = new SelectList(parentCategories, "Id", "CategoryName");

            return View(result.Data.Adapt<AdminCategoryUpdateVM>());
        }



        [HttpPost]
        public async Task<IActionResult> Update(AdminCategoryUpdateVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            
            var categoryExist = await _categoryService.IsCategoryNameExistAsync(model.CategoryName, model.Id);
            if (categoryExist)
            {
                var message3 = _stringLocalizer["A category with this name already exists."];
                ErrorNotyf(message3);
                return View(model);
            }

            var result = await _categoryService.UpdateAsync(model.Adapt<CategoryUpdateDTO>());
            if (!result.IsSucces)
            {
                var message2 = _stringLocalizer["Category update failed."];
                ErrorNotyf(message2);
                return View(model);
            }
            var message = _stringLocalizer["Category updated successfully."];
            SuccesNotyf(message);
            return RedirectToAction("Index");
        }




        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _categoryService.GetByIdAsync(id);
            if (!result.IsSucces)
            {
                var message = _stringLocalizer["Failed"];
                ErrorNotyf(message);
                return RedirectToAction("Index");
            }
            var message2 = _stringLocalizer["Category Detail Page Loaded Successfully"];
            SuccesNotyf(message2);
            return View(result.Data.Adapt<AdminCategoryDetailVM>());

        }




    }
}



