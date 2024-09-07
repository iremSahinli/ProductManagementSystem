using ManagmentSystem.Business.DTOs.CategoryDTOs;
using ManagmentSystem.Business.DTOs.ProductDTOs;
using ManagmentSystem.Business.Services.CategoryServices;
using ManagmentSystem.Business.Services.ProductServices;
using ManagmentSystem.Domain.Entities;
using ManagmentSystem.Infrastructure.AppContext;
using ManagmentSystem.Infrastructure.Repositories.CategoryRepositories;
using ManagmentSystem.Infrastructure.Repositories.ProductCategoryRepositories;
using ManagmentSystem.Infrastructure.Repositories.ProductRepositories;
using ManagmentSystem.Presentation.Areas.Admin.Models.CategoryVMs;
using ManagmentSystem.Presentation.Areas.Admin.Models.ProductVMs;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ManagmentSystem.Presentation.Areas.Admin.Controllers
{
    public class ProductController : AdminBaseController
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IProductRepository _productRepository;
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly AppDbContext _context;




        public ProductController(IProductService productService, ICategoryService categoryService, IProductRepository productRepository, IProductCategoryRepository productCategoryRepository, AppDbContext context)
        {
            _productService = productService;
            _categoryService = categoryService;
            _productRepository = productRepository;
            _productCategoryRepository = productCategoryRepository;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _productService.GetAllAsync();
            var categoryVMs = result.Data.Adapt<List<AdminProductListVM>>();
            if (!result.IsSucces)
            {
                ErrorNotyf(result.Message);
                return View(categoryVMs);
            }
            SuccesNotyf(result.Message);
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
               
                model.Categories = await GetCategories();
                return View(model);
            }

            var productCreateDTO = new ProductCreateDTO
            {
                ProductName = model.ProductName,
                ProductDescription = model.ProductDescription,
                ProductPrice = model.ProductPrice,
                SelectedCategories = model.SelectedCategories,
            };

            var result = await _productService.AddAsync(productCreateDTO);
            if (!result.IsSucces)
            {
                
                ErrorNotyf(result.Message);
                model.Categories = await GetCategories(); 
                return View(model);
            }

            SuccesNotyf(result.Message);
            return RedirectToAction("Index");
        }



        public async Task<IActionResult> Update(Guid id)
        {
            var result = await _productService.GetByIdAsync(id);

            if (!result.IsSucces)
            {

                await Console.Out.WriteLineAsync(result.Message);
                return RedirectToAction("Index");
            }


            var productUpdateVM = result.Data.Adapt<AdminProductUpdateVM>();

            // Kategorileri getiriyoruz.
            productUpdateVM.Categories = await GetCategories(id);

            var product = await _context.Products
                    .Include(p => p.ProductCategories)
                    .ThenInclude(pc => pc.Category)
                    .FirstOrDefaultAsync(p => p.Id == id);

            if (product != null)
            {
                productUpdateVM.SelectedCategories = product.ProductCategories
                    .Select(pc => pc.CategoryId)
                    .ToList();
            }

            return View(productUpdateVM);

        }
        [HttpPost]
        public async Task<IActionResult> Update(AdminProductUpdateVM model)
        {
            if (!ModelState.IsValid)
            {
                // Eğer model geçersizse sayfa tekrar yüklenir.
                model.Categories = await GetCategories(model.Id); // Kategoriler yeniden yüklenir.
                return View(model);
            }


            // Modeli DTO'ya dönüştürüp güncelleme işlemini çağırıyoruz.
            var result = await _productService.UpdateAsync(model.Adapt<ProductUpdateDTO>());

            if (!result.IsSucces)
            {
                // Eğer güncelleme başarısız olursa, hata mesajı ile sayfa tekrar yüklenir.
                await Console.Out.WriteLineAsync(result.Message);
                model.Categories = await GetCategories(model.Id); // Kategoriler yeniden yüklenir.
                return View(model);
            }

            // Başarılıysa liste sayfasına yönlendiriyoruz.
            await Console.Out.WriteLineAsync(result.Message);
            return RedirectToAction("Index");
        }



        public async Task<IActionResult> Details(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return RedirectToAction("Index");
            }
            var categoryIds = await _productService.GetCategoryIdsByProductId(id);
            if (!categoryIds.IsSucces)
            {
                ErrorNotyf("Error");
                return RedirectToAction("Index");
            }

            var categoryNames = await _categoryService.GetCategoryNamesByIdsAsync(categoryIds.Data);

            var model = new AdminProductDetailVM
            {
                Id = product.Id,
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                ProductPrice = product.ProductPrice,
                CategoryNames = categoryNames,

            };
            SuccesNotyf("Product Detail Page Loaded Successfully");
            return View(model);

        }




        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _productService.DeleteAsync(id);
            if (!result.IsSucces)
            {
                ErrorNotyf(result.Message);
                return RedirectToAction("Index");
            }
            SuccesNotyf(result.Message);
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
                ErrorNotyf(result.Message);
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
