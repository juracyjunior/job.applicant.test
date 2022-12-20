using BestHB.Domain.Commands;
using BestHB.Domain.Entities;
using BestHB.Domain.Models;
using BestHB.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BestHB.Domain.Service
{
    public interface IOrderService
    {
        Task<int> CreateAsync(CreateOrderCommand createOrderCommand);
        Task<int> UpdateAsync(UpdateOrderCommand updateOrderCommand);
        Task<DeleteOrderStatus> DeleteAsync(DeleteOrderCommand deleteOrderCommand);
        Task<IList<string>> AsCsvAsync(QueryOrders queryOrders);
        Task<IList<Order>> GetAsync(QueryOrders queryOrders);
        Task<ResultService<IEnumerable<Order>>> GetListByUserAsync(int userId);
    }
}
