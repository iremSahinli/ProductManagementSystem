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
        private readonly AppDbContext _context;




        public ProductController(IProductService productService, ICategoryService categoryService, IProductRepository productRepository, AppDbContext context)
        {
            _productService = productService;
            _categoryService = categoryService;
            _productRepository = productRepository;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _productService.GetAllAsync();
            if (!result.IsSucces)
            {
                ErrorNotyf(result.Message);
                return View(new List<AdminProductListVM>()); // Boş liste döndür
            }


            var productListWithCategoryNames = new List<AdminProductListVM>();

            foreach (var product in result.Data)
            {
                var categoryNames = await _productService.GetCategoryNamesByProductIdAsync(product.Id);

                // ViewModel'e kategori isimlerini ekledim
                var productVm = new AdminProductListVM
                {
                    Id = product.Id,
                    ProductName = product.ProductName,
                    ProductDescription = product.ProductDescription,
                    ProductPrice = product.ProductPrice,
                    ProductImage = product.ProductImage,
                    CategoryNames = categoryNames
                };

                productListWithCategoryNames.Add(productVm);
            }

            SuccesNotyf(result.Message);
            return View(productListWithCategoryNames);
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


            string productImagePath = null; //Görsel kaydetme işlemi için gerekli alan.
            if (model.ProductImage != null)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ProductImage.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProductImage.CopyToAsync(fileStream);
                }

                productImagePath = "/uploads/" + uniqueFileName;
            }


            var productCreateDTO = new ProductCreateDTO
            {
                ProductName = model.ProductName,
                ProductDescription = model.ProductDescription,
                ProductPrice = model.ProductPrice,
                ProductImage = productImagePath,
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


            productUpdateVM.Categories = await GetCategories(id);


            if (string.IsNullOrEmpty(productUpdateVM.ProductImagePath))
            {
                var product = await _context.Products
                    .Include(p => p.ProductCategories)
                    .ThenInclude(pc => pc.Category)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (product != null)
                {
                    productUpdateVM.SelectedCategories = product.ProductCategories
                        .Select(pc => pc.CategoryId)
                        .ToList();

                    productUpdateVM.ProductImagePath = product.ProductImage;
                }
            }

            return View(productUpdateVM);

        }
        [HttpPost]
        public async Task<IActionResult> Update(AdminProductUpdateVM model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = await GetCategories(model.Id);
                return View(model);
                
            }

            if (model.ProductImage != null)
            {

                if (!string.IsNullOrEmpty(model.ProductImagePath))
                {
                    var existingFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", model.ProductImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(existingFilePath))
                    {
                        System.IO.File.Delete(existingFilePath);
                    }
                }
                // Dosya adı benzersiz bir ad ile değiştirilir
                var uniqueFileName = $"{Guid.NewGuid()}_{model.ProductImage.FileName}";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProductImage.CopyToAsync(stream);
                }

                
                model.ProductImagePath = $"/uploads/{uniqueFileName}"; //doğru dosya yolunu DTO ya veriyoruz.
            }

            var productUpdateDTO = model.Adapt<ProductUpdateDTO>();


            
            var result = await _productService.UpdateAsync(model.Adapt<ProductUpdateDTO>());

            if (!result.IsSucces)
            {
                // Eğer güncelleme başarısız olursa, hata mesajı ile sayfa tekrar yüklenir.
                ErrorNotyf(result.Message);
                model.Categories = await GetCategories(model.Id); // Kategoriler yeniden yüklenir.
                return View(model);
            }

           
            SuccesNotyf(result.Message);
            Console.WriteLine(model.ProductImage);
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
                ProductImage = product.ProductImage

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
