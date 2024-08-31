using ManagmentSystem.Business.DTOs.CategoryDTOs;
using ManagmentSystem.Domain.Entities;
using ManagmentSystem.Domain.Utilities.Concretes;
using ManagmentSystem.Domain.Utilities.Interfaces;
using ManagmentSystem.Infrastructure.Repositories.CategoryRepositories;
using Mapster;
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


        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }




        public async Task<IResult> AddAsync(CategoryCreateDTO categoryCreateDTO)
        {
            if (await _categoryRepository.AnyAsync(x => x.CategoryName.ToLower() == categoryCreateDTO.CategoryName.ToLower()))
            {
                return new ErrorResult("Kategori Sistemde Mevcut");

            }
            var newCategory = categoryCreateDTO.Adapt<Category>();
            await _categoryRepository.AddAsync(newCategory);
            await _categoryRepository.SaveChangeAsync();
            return new SuccessResult("Kategori Ekleme işlemi başarılı");
        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            //if (await _categoryRepository.AnyAsync(c =>c.Id == id))
            //{
            //    return new ErrorResult("Kategori Kullanılıyor");
            //}
            var deletingCategory = await _categoryRepository.GetByIdAsync(id);
            if (deletingCategory is null)
            {
                return new ErrorResult("Kategori Bulunamadı");
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
            try
            {
                var updatedCategory = categoryUpdateDTO.Adapt(updatingCategory);
                await _categoryRepository.UpdateAsync(updatedCategory);
                await _categoryRepository.SaveChangeAsync();
                return new SuccessResult("Kategori Güncelleme Başarılı");
            }
            catch (Exception ex)
            {

                return new ErrorResult("Hata: " + ex.Message);
            }
        }
    }
}
