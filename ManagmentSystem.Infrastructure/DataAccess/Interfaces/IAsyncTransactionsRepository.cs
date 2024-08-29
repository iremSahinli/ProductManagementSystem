using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagmentSystem.Infrastructure.DataAccess.Interfaces
{
    public interface IAsyncTransactionsRepository
    {
        Task<IDbContextTransaction> BeginTransaction(CancellationToken cancellationToken = default);

        Task<IExecutionStrategy> CreateExecutionStrategy();
    }
}
