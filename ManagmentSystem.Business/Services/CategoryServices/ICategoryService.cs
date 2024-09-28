using ManagmentSystem.Business.DTOs.CategoryDTOs;
using ManagmentSystem.Domain.Utilities.Interfaces;

namespace ManagmentSystem.Business.Services.CategoryServices
{
    public interface ICategoryService
    {
        /// <summary>
        /// Sisteme yeni bir kategori ekler.
        /// </summary>
        /// <param name="categoryCreateDTO">Eklenecek kategorinin detaylarını içeren veri transfer nesnesi (DTO).</param>
        /// <returns>
        /// İşlemin başarısını veya başarısızlığını belirten bir IResult nesnesi ile tamamlanan asenkron bir görev döner.
        /// </returns>
        Task<IResult> AddAsync(CategoryCreateDTO categoryCreateDTO);
        /// <summary>
        /// Sistemde var olan bir kategoriyi günceller.
        /// </summary>
        /// <param name="categoryUpdateDTO">Güncellenecek kategorinin detaylarını içeren veri transfer nesnesi (DTO).</param>
        /// <returns>
        /// İşlemin başarısını veya başarısızlığını belirten bir IResult nesnesi ile tamamlanan asenkron bir görev döner.
        /// </returns>
        Task<IResult> UpdateAsync(CategoryUpdateDTO categoryUpdateDTO);
        /// <summary>
        /// Belirtilen kimliğe (ID) sahip kategoriyi sistemden siler.
        /// </summary>
        /// <param name="id">Silinecek kategorinin benzersiz kimliği (ID).</param>
        /// <returns>
        /// İşlemin başarısını veya başarısızlığını belirten bir IResult nesnesi ile tamamlanan asenkron bir görev döner.
        /// </returns>
        Task<IResult> DeleteAsync(Guid id);
        /// <summary>
        /// Sistemden tüm kategorileri getirir.
        /// </summary>
        /// <returns>
        /// Kategorilerin detaylarını içeren CategoryListDTO nesnelerinin listesini döndüren bir IDataResult ile tamamlanan asenkron bir görev döner.
        /// </returns>
        Task<IDataResult<List<CategoryListDTO>>> GetAllAsync();
        /// <summary>
        /// Belirtilen kimliğe (ID) sahip kategorinin detaylarını getirir.
        /// </summary>
        /// <param name="id">Getirilecek kategorinin benzersiz kimliği (ID).</param>
        /// <returns>
        /// İstenen kategorinin detaylarını içeren CategoryDTO nesnesi ile tamamlanan bir IDataResult döner.
        /// </returns>
        Task<IDataResult<CategoryDTO>> GetByIdAsync(Guid id);
        /// <summary>
        /// Belirtilen kimliğe (ID) sahip kategorinin adını getirir.
        /// </summary>
        /// <param name="categoryId">Adı getirilecek kategorinin benzersiz kimliği (ID).</param>
        /// <returns>
        /// Kategorinin adını içeren bir string döndüren asenkron görev. Kategori bulunamazsa null dönebilir.
        /// </returns>
        Task<string>? GetNameById(Guid categoryId);

        /// <summary>
        /// CategoryId nin herhangi bir yerde kullanılıp kullanılmadığını kontrol eden asenkron metot.
        /// </summary>
        /// <param name="categoryId">Kontrol edilecek anahtar property.</param>
        /// <returns>Geriye true veya false döndürür.</returns>
        Task<bool> IsCategoryUsedAsync(Guid categoryId);

        /// <summary>
        /// Belirtilen ürüne ait kategorilerin kimliklerini (ID'lerini) getirir.
        /// </summary>
        /// <param name="productId">Kategorilerinin kimliklerinin getirileceği ürünün benzersiz kimliği (ID).</param>
        /// <returns>
        /// Ürüne ait kategori kimliklerini içeren bir List<Guid> döndüren asenkron bir görev.
        /// </returns>
        Task<List<Guid>> GetCategoryIdsByProductIdAsync(Guid productId);
        /// <summary>
        /// Belirtilen kategori kimliklerine (ID'lerine) göre kategori adlarını getirir.
        /// </summary>
        /// <param name="categoryIds">Adları getirilecek kategorilerin kimliklerinin listesi.</param>
        /// <returns>
        /// Kategori adlarını içeren bir List<string> döndüren asenkron bir görev.
        /// </returns>
        Task<List<string>> GetCategoryNamesByIdsAsync(List<Guid> categoryIds);
        /// <summary>
        /// Tüm kategorileri getirir. Formlarda kategori seçebilmek için kullanılır.
        /// </summary>
        /// <returns>
        /// Kategori adlarını ve diğer gerekli bilgileri içeren GetCategoryNameDTO nesnelerinin listesini döndüren asenkron bir görev.
        /// </returns>
        Task<List<GetCategoryNameDTO>> GetAllCategoriesAsync(); //Formda categorileri getirebilmek için eklendi.
        /// <summary>
        /// Belirtilen kategori adının sistemde zaten var olup olmadığını kontrol eder.
        /// </summary>
        /// <param name="categoryName">Kontrol edilecek kategori adı.</param>
        /// <param name="categoryId">
        /// Güncellenen bir kategori durumu varsa, bu kategori kimliği (ID) kullanılır.
        /// Bu parametre boş bırakılırsa, yeni bir kategori ekleniyormuş gibi kontrol yapılır.
        /// </param>
        /// <returns>
        /// Kategori adının var olup olmadığını belirten bir boolean değer döndüren asenkron bir görev.
        /// </returns>
        Task<bool> IsCategoryNameExistAsync(string categoryName, Guid? categoryId = null);
        /// <summary>
        /// Belirtilen alt kategorinin herhangi bir üründe kullanılıp kullanılmadığını kontrol eder.
        /// </summary>
        /// <param name="categoryId">Kontrol edilecek alt kategorinin benzersiz kimliği (ID).</param>
        /// <returns>
        /// Alt kategorinin ürün tablosunda kullanılıp kullanılmadığını belirten bir boolean değer döndüren asenkron bir görev.
        /// </returns>
        Task<bool> IsSubCategoryUsedAsync(Guid categoryId);  //silinmek istenen category product tablosunda kullanılıyormu?.

    }
}
