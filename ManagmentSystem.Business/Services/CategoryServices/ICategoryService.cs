using ManagmentSystem.Business.DTOs.CategoryDTOs;
using ManagmentSystem.Domain.Utilities.Interfaces;

namespace ManagmentSystem.Business.Services.CategoryServices
{
    public interface ICategoryService
    {
        Task<IResult> AddAsync(CategoryCreateDTO categoryCreateDTO);
        Task<IDataResult<List<CategoryListDTO>>> GetAllAsync();
        Task<IResult> DeleteAsync(Guid id);
        Task<IDataResult<CategoryDTO>> GetByIdAsync(Guid id);
        Task<IResult> UpdateAsync(CategoryUpdateDTO categoryUpdateDTO);
    }
}
