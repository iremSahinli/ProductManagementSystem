using ManagmentSystem.Business.DTOs.CategoryDTOs;
using ManagmentSystem.Business.DTOs.ProductDTOs;
using ManagmentSystem.Business.Services.CategoryServices;
using ManagmentSystem.Domain.Entities;
using ManagmentSystem.Domain.Utilities.Concretes;
using ManagmentSystem.Domain.Utilities.Interfaces;
using ManagmentSystem.Infrastructure.Repositories.CategoryRepositories;
using ManagmentSystem.Infrastructure.Repositories.ProductCategoryRepositories;
using ManagmentSystem.Infrastructure.Repositories.ProductRepositories;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagmentSystem.Business.Services.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly ICategoryService _categoryService;
        private readonly ICategoryRepository _categoryRepository;
        public ProductService(IProductRepository productRepository, IProductCategoryRepository productCategoryRepository, ICategoryService categoryService, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _productCategoryRepository = productCategoryRepository;
            _categoryService = categoryService;
            _categoryRepository = categoryRepository;
        }

        public async Task<IResult> AddAsync(ProductCreateDTO productCreateDTO)
        {
            if (await _productRepository.AnyAsync(x => x.ProductName.ToLower() == productCreateDTO.ProductName.ToLower()))
            {
                return new ErrorResult("Ürün Sistemde Mevcut");
            }

            // Transaction oluşturma
            using (var transaction = await _productRepository.BeginTransactionAsync())
            {
                try
                {
                    // Yeni ürünü ekleme
                    var newProduct = productCreateDTO.Adapt<Product>();
                    await _productRepository.AddAsync(newProduct);

                    // Seçilen kategori için ProductCategory tablosuna kayıt ekleme
                    if (productCreateDTO.SelectedCategoryId != Guid.Empty)
                    {
                        var newProductCategory = new ProductCategory
                        {
                            CategoryId = productCreateDTO.SelectedCategoryId,
                            ProductId = newProduct.Id
                        };
                        await _productCategoryRepository.AddAsync(newProductCategory);
                    }

                    // Değişiklikleri kaydetme
                    await _productRepository.SaveChangeAsync();
                    await _productCategoryRepository.SaveChangeAsync();

                    // Transaction'ı onaylama
                    await transaction.CommitAsync();

                    return new SuccessResult("Ürün Ekleme Başarılı");
                }
                catch (Exception ex)
                {
                    // Hata durumunda transaction'ı geri alma
                    await transaction.RollbackAsync();
                    return new ErrorResult("Hata: " + ex.Message);
                }
            }
        }
        public async Task<IResult> DeleteAsync(Guid productId)
        {
            var deletingProduct = await _productRepository.GetByIdAsync(productId);

            foreach (var item in deletingProduct.ProductCategories)
            {
                await _productCategoryRepository.DeleteAsync(item);
            }
            if (deletingProduct == null)
            {
                return new ErrorResult("Silinecek Ürün Bulunamadı");
            }
            try
            {
                await _productRepository.DeleteAsync(deletingProduct);
                await _productRepository.SaveChangeAsync();
                return new SuccessResult("Ürün Silme İşlemi Başarılı");
            }
            catch (Exception ex)
            {
                return new ErrorResult("Hata: " + ex.Message);
            }
        }

        public async Task<IDataResult<List<ProductListDTO>>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();
            var productListDtos = products.Adapt<List<ProductListDTO>>();
            if (productListDtos.Count() <= 0)
            {
                return new ErrorDataResult<List<ProductListDTO>>(productListDtos, "Listelenecek Ürün Bulunamadı");
            }
            return new SuccessDataResult<List<ProductListDTO>>(productListDtos, "Ürün Listeleme Başarılı");
        }

        public async Task<IDataResult<List<ProductListDTOForSelect>>> GetAllForSelectAsync()
        {
            var products = await _productRepository.GetAllAsync();
            var productSelectListDtos = products.Adapt<List<ProductListDTOForSelect>>();
            if (productSelectListDtos.Count() <= 0)
            {
                return new ErrorDataResult<List<ProductListDTOForSelect>>(productSelectListDtos, "Listelenecek Ürün Bulunamadı");
            }
            return new SuccessDataResult<List<ProductListDTOForSelect>>(productSelectListDtos, "Ürün Listeleme Başarılı");
        }

        public async Task<IDataResult<ProductDTO>> GetByIdAsync(Guid prodcutId)
        {
            var product = await _productRepository.GetByIdAsync(prodcutId);

            var productDto = product.Adapt<ProductDTO>();
            foreach (var item in product.ProductCategories)
            {
                if (item.Status != Domain.Enums.Status.Deleted)
                {
                    productDto.CategoryName.Add(await _categoryService.GetNameById(item.CategoryId));
                }

            }

            if (product is null)
            {
                return new ErrorDataResult<ProductDTO>(productDto, "Görüntülenecek Kategori Bulunamadı");
            }
            return new SuccessDataResult<ProductDTO>(productDto, "Kategori Görüntüleme Başarılı");
        }

        public async Task<IDataResult<List<Guid>>> GetCategoryIdsByProductId(Guid? productId)
        {
            var categoryIds = new List<Guid>();
            var productCategories = await _productCategoryRepository.GetAllAsync(x => x.ProductId == productId);
            foreach (var item in productCategories)
            {
                categoryIds.Add(item.CategoryId);
            }
            return new SuccessDataResult<List<Guid>>(categoryIds, "Kategori ID'ler listelendi.");
        }



        public async Task<double> GetPriceByProductId(Guid productId)
        {
            return (await _productRepository.GetByIdAsync(productId))!.ProductPrice;
        }

        public async Task<IResult> UpdateAsync(ProductUpdateDTO productUpdateDTO)
        {
            var updateingProduct = await _productRepository.GetByIdAsync(productUpdateDTO.Id);

            foreach (var item in updateingProduct.ProductCategories)
            {
                await _productCategoryRepository.DeleteAsync(item);
            }

            if (updateingProduct is null)
            {
                return new ErrorResult("Güncellenecek Ürün Bulunamadı");
            }
            try
            {
                var updatedProduct = productUpdateDTO.Adapt(updateingProduct);
                foreach (var categoryId in productUpdateDTO.SelectedCategories)
                {
                    var newProductCategory = new ProductCategory()
                    {
                        CategoryId = categoryId,
                        ProductId = updateingProduct.Id
                    };

                    await _productCategoryRepository.AddAsync(newProductCategory);
                    // await _productCategoryRepository.SaveChangeAsync();
                }
                await _productRepository.UpdateAsync(updatedProduct);
                await _productRepository.SaveChangeAsync();
                return new SuccessResult("Ürün Güncelleme Başarılı");
            }
            catch (Exception ex)
            {
                return new ErrorResult("Hata: " + ex.Message);
            }
        }

        public async Task<string> GetCategoryNameByProductIdAsync(Guid productId)
        {
            var productCategory = await _productCategoryRepository.GetAsync(pc => pc.ProductId == productId);
            var categoryId = productCategory?.CategoryId;

            if (categoryId == null)
            {
                return string.Empty;
            }
            var category = await _categoryRepository.GetByIdAsync(categoryId.Value);
            return category.CategoryName;
        }
    }
}
