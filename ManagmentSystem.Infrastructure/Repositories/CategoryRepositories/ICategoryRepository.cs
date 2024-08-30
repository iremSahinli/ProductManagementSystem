using Castle.Core.Resource;
using ManagmentSystem.Domain.Entities;
using ManagmentSystem.Infrastructure.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagmentSystem.Infrastructure.Repositories.CategoryRepositories
{
    public interface ICategoryRepository : IAsyncRepository, IAsyncFindableRepository<Category>, IAsyncInsertableRepository<Category>, IAsyncQueryableRepository<Category>, IAsyncDeletableRepository<Category>, IAsyncUpdatebleRepository<Category>, IAsyncTransactionsRepository
    {
    }
}
