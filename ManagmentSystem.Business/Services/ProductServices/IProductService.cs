
using ManagmentSystem.Business.DTOs.ProductDTOs;
using ManagmentSystem.Domain.Utilities.Interfaces;
namespace ManagmentSystem.Business.Services.ProductServices
{
    public interface IProductService
    {
        Task<IResult> AddAsync(ProductCreateDTO productCreateDTO);
        Task<IResult> UpdateAsync(ProductUpdateDTO productUpdateDTO);
        Task<IResult> DeleteAsync(Guid productId);
        Task<IDataResult<List<ProductListDTO>>> GetAllAsync();
        Task<IDataResult<List<ProductListDTOForSelect>>> GetAllForSelectAsync();
        Task<IDataResult<ProductDTO>> GetByIdAsync(Guid prodcutId);
        Task<double> GetPriceByProductId(Guid productId);
        Task<string> GetCategoryNameByProductIdAsync(Guid productId);
        /// <summary>
        /// Silme işlemi için kategorinin kullanılıp kullanılmadığını kontrol eden Asenkron yapısı.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns>geriye true veya false döner.</returns>
        Task<bool> IsCategoryUsedAsync(Guid categoryId);  //silinmek istenen category product tablosunda kullanılıyormu?.
        Task<IDataResult<List<Guid>>> GetCategoryIdsByProductId(Guid? productId); //Idlerine göre Categoriyleri getirir.
        
    }
}
