using BestHB.Domain.Commands;
using BestHB.Domain.Entities;
using BestHB.Domain.Queries;
using BestHB.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace BestHB.Repository
{
    public class OrderRepository : IOrderRepository
    {
        public async Task<int> AddAsync(Order order)
        {
            return 123;
        }

        public async Task<List<Order>> GetAsync(QueryOrders queryOrders)
        {
            return new List<Order>();
        }

        public async Task<IEnumerable<Order>> GetByUserAsync(int userId)
        {
            if (userId == 0) return null;

            var order = new Order {
                Id = Guid.NewGuid(),
                UserId = userId,
                Symbol = "",
                Quantity = 1,
                Side = OrderSide.Sell,
                Type = OrderType.Market,
                Status = OrderStatus.Executed,
                Price = 1.0M,
                TriggerPrice = 0,
                ExpiresAt = null,
                CreatedAt = DateTime.Now
            };

            return new List<Order>{ order };
        }
    }
}
