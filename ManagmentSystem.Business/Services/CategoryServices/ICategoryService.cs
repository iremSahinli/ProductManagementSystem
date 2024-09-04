using ManagmentSystem.Business.DTOs.CategoryDTOs;
using ManagmentSystem.Domain.Utilities.Interfaces;

namespace ManagmentSystem.Business.Services.CategoryServices
{
    public interface ICategoryService
    {
        Task<IResult> AddAsync(CategoryCreateDTO categoryCreateDTO);
        Task<IResult> UpdateAsync(CategoryUpdateDTO categoryUpdateDTO);
        Task<IResult> DeleteAsync(Guid id);
        Task<IDataResult<List<CategoryListDTO>>> GetAllAsync();
        Task<IDataResult<CategoryDTO>> GetByIdAsync(Guid id);
        Task<string>? GetNameById(Guid categoryId);

        /// <summary>
        /// CategoryId nin herhangi bir yerde kullanılıp kullanılmadığını kontrol eden asenkron metot.
        /// </summary>
        /// <param name="categoryId">Kontrol edilecek anahtar property.</param>
        /// <returns>Geriye true veya false döndürür.</returns>
        Task<bool> IsCategoryUsedAsync(Guid categoryId);
    }
}
