using ManagmentSystem.Domain.Entities;
using ManagmentSystem.Infrastructure.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagmentSystem.Infrastructure.Repositories.ProductCategoryRepositories
{
    public interface IProductCategoryRepository : IAsyncDeletableRepository<ProductCategory>, IAsyncInsertableRepository<ProductCategory>,
        IAsyncQueryableRepository<ProductCategory>, IAsyncRepository, IAsyncFindableRepository<ProductCategory>, IAsyncUpdatebleRepository<ProductCategory>
    {
    }
}
