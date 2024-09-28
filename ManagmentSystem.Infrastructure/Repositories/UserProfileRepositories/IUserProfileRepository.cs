using ManagmentSystem.Domain.Entities;
using ManagmentSystem.Infrastructure.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagmentSystem.Infrastructure.Repositories.UserProfileRepositories
{
    public interface IUserProfileRepository : IAsyncRepository, IAsyncFindableRepository<UserProfile>, IAsyncInsertableRepository<UserProfile>, IAsyncQueryableRepository<UserProfile>, IAsyncDeletableRepository<UserProfile>, IAsyncUpdatebleRepository<UserProfile>, IAsyncTransactionRepository
    {
        /// <summary>
        /// Belirtilen kimliğe (identityUserId) göre kullanıcı profilini bulur.
        /// </summary>
        /// <param name="identityUserId">Aranacak kullanıcının kimliği.</param>
        /// <returns>
        /// Kullanıcı profilini içeren bir UserProfile nesnesi döndüren asenkron bir görev. 
        /// Kullanıcı bulunamazsa null dönebilir.
        /// </returns>
        Task<UserProfile?> FindByIdentityUserIdAsync(string identityUserId);
    }
}
