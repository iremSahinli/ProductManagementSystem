
using ManagmentSystem.Business.DTOs.ProductDTOs;
using ManagmentSystem.Domain.Utilities.Interfaces;
namespace ManagmentSystem.Business.Services.ProductServices
{
    public interface IProductService
    {
        /// <summary>
        /// Sisteme yeni bir ürün ekler.
        /// </summary>
        /// <param name="productCreateDTO">Eklenecek ürünün detaylarını içeren veri transfer nesnesi (DTO).</param>
        /// <returns>
        /// İşlemin başarı durumunu belirten bir IResult nesnesi ile tamamlanan asenkron bir görev.
        /// </returns>
        Task<IResult> AddAsync(ProductCreateDTO productCreateDTO);
        /// <summary>
        /// Sistemde var olan bir ürünü günceller.
        /// </summary>
        /// <param name="productUpdateDTO">Güncellenecek ürünün detaylarını içeren veri transfer nesnesi (DTO).</param>
        /// <returns>
        /// İşlemin başarı durumunu belirten bir IResult nesnesi ile tamamlanan asenkron bir görev.
        /// </returns>
        Task<IResult> UpdateAsync(ProductUpdateDTO productUpdateDTO);
        /// <summary>
        /// Belirtilen kimliğe (ID) sahip ürünü sistemden siler.
        /// </summary>
        /// <param name="productId">Silinecek ürünün benzersiz kimliği (ID).</param>
        /// <returns>
        /// İşlemin başarı durumunu belirten bir IResult nesnesi ile tamamlanan asenkron bir görev.
        /// </returns>
        Task<IResult> DeleteAsync(Guid productId);
        /// <summary>
        /// Sistemden tüm ürünleri getirir.
        /// </summary>
        /// <returns>
        /// Ürünlerin detaylarını içeren ProductListDTO nesnelerinin listesini döndüren bir IDataResult ile tamamlanan asenkron bir görev.
        /// </returns>
        Task<IDataResult<List<ProductListDTO>>> GetAllAsync();
        /// <summary>
        /// Seçim için kullanılabilecek tüm ürünleri getirir.
        /// </summary>
        /// <returns>
        /// Ürünlerin seçime uygun detaylarını içeren ProductListDTOForSelect nesnelerinin listesini döndüren bir IDataResult ile tamamlanan asenkron bir görev.
        /// </returns>
        Task<IDataResult<List<ProductListDTOForSelect>>> GetAllForSelectAsync();
        /// <summary>
        /// Belirtilen kimliğe (ID) sahip ürünün detaylarını getirir.
        /// </summary>
        /// <param name="productId">Detayları getirilecek ürünün benzersiz kimliği (ID).</param>
        /// <returns>
        /// İstenen ürünün detaylarını içeren ProductDTO nesnesi ile tamamlanan bir IDataResult döner.
        /// </returns>
        Task<IDataResult<ProductDTO>> GetByIdAsync(Guid prodcutId);
        /// <summary>
        /// Belirtilen kimliğe (ID) sahip ürünün fiyatını getirir.
        /// </summary>
        /// <param name="productId">Fiyatı getirilecek ürünün benzersiz kimliği (ID).</param>
        /// <returns>
        /// Ürünün fiyatını double olarak döndüren bir asenkron görev.
        /// </returns>
        Task<double> GetPriceByProductId(Guid productId);
        /// <summary>
        /// Belirtilen kimliğe (ID) sahip ürünün kategorisinin adını getirir.
        /// </summary>
        /// <param name="productId">Kategorisi getirilecek ürünün benzersiz kimliği (ID).</param>
        /// <returns>
        /// Ürünün kategorisinin adını string olarak döndüren bir asenkron görev.
        /// </returns>
        Task<string> GetCategoryNameByProductIdAsync(Guid productId);
        /// <summary>
        /// Silme işlemi için kategorinin kullanılıp kullanılmadığını kontrol eden Asenkron yapısı.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns>geriye true veya false döner.</returns>
        Task<bool> IsCategoryUsedAsync(Guid categoryId);  //silinmek istenen category product tablosunda kullanılıyormu?.
        /// <summary>
        /// Belirtilen ürün kimliğine (ID) göre ürünün kategorilerinin kimliklerini (ID'lerini) getirir.
        /// </summary>
        /// <param name="productId">Kategorilerin kimliklerinin getirileceği ürünün benzersiz kimliği (ID). Boş olabilir.</param>
        /// <returns>
        /// Ürüne ait kategori kimliklerini içeren bir List<Guid> döndüren asenkron bir IDataResult.
        /// </returns>
        Task<IDataResult<List<Guid>>> GetCategoryIdsByProductId(Guid? productId); //Idlerine göre Categoriyleri getirir.
        /// <summary>
        /// Belirtilen ürün kimliğine (ID) göre ürünün kategorilerinin adlarını getirir.
        /// </summary>
        /// <param name="productId">Adları getirilecek ürünün benzersiz kimliği (ID).</param>
        /// <returns>
        /// Ürünün kategorilerinin adlarını içeren bir string döndüren asenkron bir görev.
        /// </returns>
        Task<string> GetCategoryNamesByProductIdAsync(Guid productId); //birden fazla kategoriler için.

        /// <summary>
        /// Belirtilen kategori kimliğine (ID) göre ürünleri getirir.
        /// </summary>
        /// <param name="categoryId">Ürünleri getirilecek kategorinin benzersiz kimliği (ID).</param>
        /// <returns>
        /// Ürünlerin detaylarını içeren ProductListDTO nesnelerinin listesini döndüren bir IDataResult ile tamamlanan asenkron bir görev.
        /// </returns>
        Task<IDataResult<List<ProductListDTO>>> GetProductsByCategoryAsync(Guid categoryId);

    }
}
