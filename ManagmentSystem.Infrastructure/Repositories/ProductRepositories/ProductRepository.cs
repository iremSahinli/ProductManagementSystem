using ManagmentSystem.Domain.Entities;
using ManagmentSystem.Infrastructure.AppContext;
using ManagmentSystem.Infrastructure.DataAccess.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagmentSystem.Infrastructure.Repositories.ProductRepositories
{
    public class ProductRepository:EFBaseRepository<Product>,IProductRepository
    {
        public ProductRepository(AppDbContext context):base(context) { }
        
    }
}
