using ManagmentSystem.Domain.Entities;
using ManagmentSystem.Infrastructure.AppContext;
using ManagmentSystem.Infrastructure.DataAccess.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagmentSystem.Infrastructure.Repositories.UserProfileRepositories
{
    public class UserProfileRepository : EFBaseRepository<UserProfile>, IUserProfileRepository
    {
        public UserProfileRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<UserProfile?> FindByIdentityUserIdAsync(string identityUserId)
        {
            return await _context.Set<UserProfile>().FirstOrDefaultAsync(up => up.IdentityUserId == identityUserId);
        }
    }
}
