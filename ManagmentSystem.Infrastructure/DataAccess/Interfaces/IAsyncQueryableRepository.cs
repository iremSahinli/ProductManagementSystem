using ManagmentSystem.Domain.Core.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ManagmentSystem.Infrastructure.DataAccess.Interfaces
{
    public interface IAsyncQueryableRepository<TEntity> where TEntity : BaseEntity
    {
        Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = true); 
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> expression, bool tracking = true);
    }
}
