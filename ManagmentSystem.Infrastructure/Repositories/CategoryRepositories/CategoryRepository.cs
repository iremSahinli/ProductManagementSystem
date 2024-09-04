using ManagmentSystem.Domain.Entities;
using ManagmentSystem.Infrastructure.AppContext;
using ManagmentSystem.Infrastructure.DataAccess.EntityFramework;


namespace ManagmentSystem.Infrastructure.Repositories.CategoryRepositories
{
    public class CategoryRepository : EFBaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }

       
    }
}
