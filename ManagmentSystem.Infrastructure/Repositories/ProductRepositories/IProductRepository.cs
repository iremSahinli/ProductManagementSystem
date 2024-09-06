using ManagmentSystem.Domain.Entities;
using ManagmentSystem.Infrastructure.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagmentSystem.Infrastructure.Repositories.ProductRepositories
{
    public interface IProductRepository: IAsyncRepository, IRepository, IAsyncFindableRepository<Product>, IAsyncInsertableRepository<Product>, IAsyncQueryableRepository<Product>, IAsyncDeletableRepository<Product>, IAsyncUpdatebleRepository<Product>, IAsyncTransactionRepository, IAsyncOrderableRepository<Product>
    {
    }
}
