using ManagmentSystem.Domain.Core.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagmentSystem.Infrastructure.DataAccess.Interfaces
{
    public interface IAsyncUpdatebleRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity> UpdateAsync(TEntity entity);
    }
}
