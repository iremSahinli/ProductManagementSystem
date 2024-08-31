using ManagmentSystem.Business.DTOs.CategoryDTOs;
using ManagmentSystem.Business.DTOs.ProductDTOs;
using ManagmentSystem.Business.Services.CategoryServices;
using ManagmentSystem.Business.Services.ProductServices;
using ManagmentSystem.Domain.Utilities.Concretes;
using ManagmentSystem.Presentation.Areas.Admin.Models.CategoryVMs;
using ManagmentSystem.Presentation.Areas.Admin.Models.ProductVMs;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace ManagmentSystem.Presentation.Areas.Admin.Controllers
{
    public class ProductController : AdminBaseController
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _productService.GetAllAsync();
            var categoryVMs = result.Data.Adapt<List<AdminProductListVM>>();
            if (!result.IsSucces)
            {
                Console.Out.WriteLineAsync(result.Message);
                return View(categoryVMs);
            }
            Console.Out.WriteLineAsync(result.Message);
            return View(categoryVMs);
        }


        public async Task<IActionResult> Create()
        {

            var productCreateVM = new AdminProductCreateVM()
            {
                Categories = await GetCategories()
            };
            return View(productCreateVM);
        }        

        [HttpPost]
        public async Task<IActionResult> Create(AdminProductCreateVM model)
        {
            if (!ModelState.IsValid)
            {

                return View(model);
            }
            var selectedCategoryIds = model.Categories != null
                    ? model.Categories.Where(x => x.IsSelected == true).Select(x => x.Id).ToList() : new List<Guid>();
            var productCreateDTO = model.Adapt<ProductCreateDTO>();
            productCreateDTO.SelectedCategories = selectedCategoryIds;
            var result = await _productService.AddAsync(productCreateDTO);
            if (!result.IsSucces)
            {
                Console.Out.WriteLineAsync(result.Message);
                return View(model);
            }
            Console.Out.WriteLineAsync(result.Message);
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Update(Guid id)
        {
            var result = await _productService.GetByIdAsync(id);

            if (!result.IsSucces)
            {
                Console.Out.WriteLineAsync(result.Message);
                return RedirectToAction("Index");
            }
            var productUpdateVM = result.Data.Adapt<AdminProductUpdateVM>();
            productUpdateVM.Categories = await GetCategories(id);
            return View(productUpdateVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(AdminProductUpdateVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await _productService.UpdateAsync(model.Adapt<ProductUpdateDTO>());
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
            var result = await _productService.GetByIdAsync(id);
            if (!result.IsSucces)
            {
                Console.Out.WriteLineAsync(result.Message);
                return RedirectToAction("Index");
            }
            return View(result.Data.Adapt<AdminProductDetailVM>());
            //CategoryName listesini tek bir string haline getiriyoruz
            //var adminProductDetailVM = result.Data.Adapt<AdminProductDetailVM>();
            //adminProductDetailVM.CategoryName = string.Join(", ", result.Data.CategoryName);

            //return View(adminProductDetailVM);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _productService.DeleteAsync(id);
            if (!result.IsSucces)
            {
                Console.Out.WriteLineAsync(result.Message);
                return RedirectToAction("Index");
            }
            Console.Out.WriteLineAsync(result.Message);
            return RedirectToAction("Index");

        }
        private async Task<List<CategorySelectModel>> GetCategories(Guid? productId = null)
        {
            var categoriListDTOs = (await _categoryService.GetAllAsync()).Data;
            var categoriSelectModels = categoriListDTOs.Adapt<List<CategorySelectModel>>();
            if (productId == null)
            {
                return categoriSelectModels;
            }
            var result = await _productService.GetCategoryIdsByProductId(productId);
            if (!result.IsSucces)
            {
                Console.Out.WriteLineAsync(result.Message);
                return categoriSelectModels;
            }

            foreach (var item in categoriSelectModels)
            {
                foreach (var categoryId in result.Data)
                {
                    if (item.Id == categoryId)
                    {
                        item.IsSelected = true;
                    }
                }
            }

            return categoriSelectModels;
        }


    }
}
