using BestHB.Domain.Entities;
using BestHB.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BestHB.Domain.Repositories
{
    public interface IOrderRepository: IRepositoryBase<Order>
    {
        Task<List<Order>> GetAsync(QueryOrders queryOrders);
        Task<IEnumerable<Order>> GetByUserAsync(int userId);
    }
}
