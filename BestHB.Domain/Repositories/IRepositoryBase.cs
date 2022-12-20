using BestHB.Domain.Entities;
using System;
using BestHB.Domain.Queries;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace BestHB.Domain.Repositories
{
    public interface IRepositoryBase<T>
    {
        Task<int> AddAsync(T entity);
    }
}