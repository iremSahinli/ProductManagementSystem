using ManagmentSystem.Business.DTOs.CategoryDTOs;
using ManagmentSystem.Domain.Entities;
using ManagmentSystem.Domain.Enums;
using ManagmentSystem.Domain.Utilities.Concretes;
using ManagmentSystem.Domain.Utilities.Interfaces;
using ManagmentSystem.Infrastructure.AppContext;
using ManagmentSystem.Infrastructure.Repositories.CategoryRepositories;
using ManagmentSystem.Infrastructure.Repositories.ProductCategoryRepositories;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagmentSystem.Business.Services.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly AppDbContext _context;



        public CategoryService(ICategoryRepository categoryRepository, AppDbContext context, IProductCategoryRepository productCategoryRepository)
        {
            _categoryRepository = categoryRepository;
            _productCategoryRepository = productCategoryRepository;
            _context = context;
        }




        public async Task<IResult> AddAsync(CategoryCreateDTO categoryCreateDTO)
        {
            if (await _categoryRepository.AnyAsync(x => x.CategoryName.ToLower() == categoryCreateDTO.CategoryName.ToLower()))
            {
                return new ErrorResult("Kategori Sistemde Mevcut");
            }

            var newCategory = categoryCreateDTO.Adapt<Category>();


            if (categoryCreateDTO.ParentCategoryId.HasValue)
            {
                newCategory.ParentCategoryId = categoryCreateDTO.ParentCategoryId;
            }
            else
            {
                newCategory.ParentCategoryId = null;
            }

            await _categoryRepository.AddAsync(newCategory);
            await _categoryRepository.SaveChangeAsync();
            return new SuccessResult("Kategori Ekleme işlemi başarılı");
        }


        public async Task<IResult> DeleteAsync(Guid id)
        {
            var deletingCategory = await _categoryRepository.GetByIdAsync(id);
            if (deletingCategory is null)
            {
                return new ErrorResult("Kategori Bulunamadı");
            }


            var subCategories = await _context.Categories
                .Where(c => c.ParentCategoryId == deletingCategory.Id).ToListAsync();

            if (subCategories.Any())
            {

                foreach (var subCategory in subCategories)
                {
                    await _categoryRepository.DeleteAsync(subCategory);
                }
            }


            await _categoryRepository.DeleteAsync(deletingCategory);
            await _categoryRepository.SaveChangeAsync();
            return new SuccessResult("Kategori silme Başarılı");
        }



        public async Task<IDataResult<List<CategoryListDTO>>> GetAllAsync()
        {
            var categories = (await _categoryRepository.GetAllAsync()).Adapt<List<CategoryListDTO>>();
            return new SuccessDataResult<List<CategoryListDTO>>(categories, "Kategori Listeleme Başarılı");
        }

        public async Task<IDataResult<CategoryDTO>> GetByIdAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            return new SuccessDataResult<CategoryDTO>(category.Adapt<CategoryDTO>(), "Kategori getirildi");
        }

        public async Task<string>? GetNameById(Guid categoryId)
        {
            return (await _categoryRepository.GetByIdAsync(categoryId)).CategoryName;
        }


        public async Task<IResult> UpdateAsync(CategoryUpdateDTO categoryUpdateDTO)
        {
            var updatingCategory = await _categoryRepository.GetByIdAsync(categoryUpdateDTO.Id);
            if (updatingCategory is null)
            {
                return new ErrorResult("Güncellenecek Kategori Bulunamadı");
            }

            bool isCategoryUsed = await IsCategoryUsedAsync(categoryUpdateDTO.Id);
            if (isCategoryUsed)
            {
                return new ErrorResult("Güncellenmek istenen kategori sistemde kullanıldığından güncelleme işlemi yapılamaz");
            }

            try
            {
                updatingCategory.CategoryName = categoryUpdateDTO.CategoryName;
                updatingCategory.Description = categoryUpdateDTO.Description;


                updatingCategory.ParentCategoryId = categoryUpdateDTO.ParentCategoryId;

                await _categoryRepository.UpdateAsync(updatingCategory);
                await _categoryRepository.SaveChangeAsync();
                return new SuccessResult("Kategori Güncelleme Başarılı");
            }
            catch (Exception ex)
            {
                return new ErrorResult("Hata: " + ex.Message);
            }
        }


        public async Task<bool> IsCategoryUsedAsync(Guid categoryId)
        {
            return await _context.ProductCategories
                .AnyAsync(pc => pc.CategoryId == categoryId && pc.Status != Status.Deleted);
        }

        public async Task<List<Guid>> GetCategoryIdsByProductIdAsync(Guid productId) //burada categoryıd leri liste oalrak getirmek istedim.
        {
            return await _context.ProductCategories.Where(pc => pc.ProductId == productId).Select(pc => pc.CategoryId).ToListAsync();
        }

        public async Task<List<string>> GetCategoryNamesByIdsAsync(List<Guid> categoryIds) //burada categoryId lere karşılık gelen categoryNameler gelir.
        {
            return await _context.Categories.Where(c => categoryIds.Contains(c.Id)).Select(c => c.CategoryName).ToListAsync();
        }

        public async Task<List<GetCategoryNameDTO>> GetAllCategoriesAsync() //Kategorileri dropdownda listelemek için gereklidir.
        {
            var categories = await _categoryRepository.GetAllAsync();
            return categories.Select(c => new GetCategoryNameDTO
            {
                Id = c.Id,
                CategoryName = c.CategoryName,
                ParentCategoryId = c.ParentCategoryId,

            }).ToList();
        }

        public async Task<bool> IsCategoryNameExistAsync(string categoryName, Guid? categoryId = null)
        {
            return await _context.Categories
      .AnyAsync(c => c.CategoryName.ToLower() == categoryName.ToLower()
                     && (categoryId == null || c.Id != categoryId)  // Güncellenen kategoriyi kontrol dışı bırak
                     && c.Status != Status.Deleted);
        }
    }
}