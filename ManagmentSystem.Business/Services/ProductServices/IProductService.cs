
using ManagmentSystem.Business.DTOs.ProductDTOs;
using ManagmentSystem.Domain.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        Task<IDataResult<List<Guid>>> GetCategoryIdsByProductId(Guid? productId);
        Task<double> GetPriceByProductId(Guid productId);
        Task<string> GetCategoryNameByProductIdAsync(Guid productId);
    }
}
