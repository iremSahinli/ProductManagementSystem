using ManagmentSystem.Domain.Entities;
using ManagmentSystem.Infrastructure.AppContext;
using ManagmentSystem.Infrastructure.DataAccess.EntityFramework;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagmentSystem.Infrastructure.Repositories.ProductCategoryRepositories
{
    public class ProductCategoryRepository:EFBaseRepository<ProductCategory>,IProductCategoryRepository
    {
        public ProductCategoryRepository(AppDbContext context):base(context) { }
        
    }
}
