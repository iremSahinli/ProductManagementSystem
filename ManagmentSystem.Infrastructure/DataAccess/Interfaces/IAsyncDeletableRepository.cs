using ManagmentSystem.Domain.Core.BaseEntities;
using ManagmentSystem.Domain.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagmentSystem.Infrastructure.DataAccess.Interfaces
{
    public interface IAsyncDeletableRepository<TEntity> where TEntity : BaseEntity
    {
        Task DeleteAsync(TEntity entity);
        Task DeleteRangeAsync(IEnumerable<TEntity> entities);
    }
}
