using ManagmentSystem.Business.DTOs.UserProfileDTOs;
using ManagmentSystem.Domain.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagmentSystem.Business.Services.UserProfileServices
{
    public interface IUserProfileService
    {
        /// <summary>
        /// Yeni bir kullanıcı profili oluşturur.
        /// </summary>
        /// <param name="userProfileDTO">Oluşturulacak kullanıcının detaylarını içeren veri transfer nesnesi (DTO).</param>
        /// <returns>
        /// Kullanıcı profilinin başarıyla oluşturulup oluşturulmadığını belirten bir IResult nesnesi ile tamamlanan asenkron bir görev.
        /// </returns>
        Task<IResult> CreateUserAsync(UserProfileDTO userProfileDTO);
        /// <summary>
        /// Var olan bir kullanıcı profilini günceller.
        /// </summary>
        /// <param name="userProfileDTO">Güncellenecek kullanıcının detaylarını içeren veri transfer nesnesi (DTO).</param>
        /// <returns>
        /// Kullanıcı profilinin başarıyla güncellenip güncellenmediğini belirten bir IResult nesnesi ile tamamlanan asenkron bir görev.
        /// </returns>
        Task<IResult> UpdateUserAsync(UserProfileDTO userProfileDTO);
        /// <summary>
        /// Belirtilen kullanıcı kimliğine (identityUserId) göre kullanıcı profilini getirir.
        /// </summary>
        /// <param name="identityUserId">Detayları getirilecek kullanıcının kimliği.</param>
        /// <returns>
        /// Kullanıcının profil detaylarını içeren bir UserProfileDTO nesnesi ile tamamlanan asenkron bir görev. 
        /// Kullanıcı bulunamazsa null dönebilir.
        /// </returns>
        Task<UserProfileDTO?> GetUserProfileAsync(string identityUserId);
        /// <summary>
        /// Belirtilen kullanıcı kimliğine (userId) göre kullanıcı profilini getirir.
        /// </summary>
        /// <param name="userId">Detayları getirilecek kullanıcının benzersiz kimliği (ID).</param>
        /// <returns>
        /// Kullanıcının profil detaylarını içeren bir UserProfileDTO nesnesi ile tamamlanan asenkron bir görev.
        /// Kullanıcı bulunamazsa null dönebilir.
        /// </returns>
        Task<UserProfileDTO?> GetUserProfileByIdAsync(Guid userId);
        /// <summary>
        /// Sistemdeki tüm kullanıcı profillerini getirir.
        /// </summary>
        /// <returns>
        /// Tüm kullanıcı profillerini içeren UserDTO nesnelerinin listesini döndüren bir asenkron görev.
        /// </returns>
        Task<List<UserDTO>> GetAllUserProfilesAsync();


    }
}
