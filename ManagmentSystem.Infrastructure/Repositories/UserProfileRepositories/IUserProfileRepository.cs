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
        Task<UserProfile?> FindByIdentityUserIdAsync(string identityUserId);
    }
}
