using BestHB.Domain.Commands;
using BestHB.Domain.Entities;
using BestHB.Domain.Models;
using BestHB.Domain.Queries;
using BestHB.Domain.Repositories;
using BestHB.Domain.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BestHB.Domain.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IInstrumentInfoRepository _instrumentInfoRepository;

        private readonly OrderType[] _ordersThatShouldHaveExpireDate = new OrderType[] {
            OrderType.Limit,
            OrderType.Stop
        };

        public OrderService(IOrderRepository orderRepository, IInstrumentInfoRepository instrumentInfoRepository)
        {
            _orderRepository = orderRepository;
            _instrumentInfoRepository = instrumentInfoRepository;
        }

        public async Task<int> CreateAsync(CreateOrderCommand createOrderCommand)
        {

            if (createOrderCommand.UserId <= 0)
                throw new Exception("Usuário inválido.");

            if (createOrderCommand.Price < 0)
                throw new Exception("O preço não pode ser menor do que zero.");

            if (createOrderCommand.Quantity <= 0)
                throw new Exception("A quantidade não pode ser menor do que zero.");

            if (string.IsNullOrWhiteSpace(createOrderCommand.Symbol))
                throw new Exception("O instrumento deve conter valor.");

            var instrumentInfo = await _instrumentInfoRepository.GetAsync(createOrderCommand.Symbol);

            if (!createOrderCommand.ExpiresAt.HasValue && _ordersThatShouldHaveExpireDate.Contains(createOrderCommand.Type))
                throw new Exception("Para o tipo de ordem especificado a data de validade deve ser preenchida.");

            if (createOrderCommand.ExpiresAt < DateTime.Now && _ordersThatShouldHaveExpireDate.Contains(createOrderCommand.Type))
                throw new Exception("Data de expiração inválida.");

            if (createOrderCommand.Type == OrderType.Stop && createOrderCommand.TriggerPrice <= 0)
                throw new Exception("O preço de gatilho deve ser preenchido quando a ordem é de stop.");

            if(createOrderCommand.Quantity % instrumentInfo.LotStep != 0 ||
                createOrderCommand.Quantity < instrumentInfo.MinLot ||
                createOrderCommand.Quantity > instrumentInfo.MaxLot)
                throw new Exception("Quantidade inválida.");

            var order = new Order {
                ExpiresAt = createOrderCommand.ExpiresAt,
                CreatedAt = DateTime.Now,
                Symbol = createOrderCommand.Symbol,
                Price = createOrderCommand.Price,
                Quantity = createOrderCommand.Quantity,
                Side = createOrderCommand.Side,
                Status = OrderStatus.Open,
                TriggerPrice = createOrderCommand.TriggerPrice,
                Type = createOrderCommand.Type,
                UserId = createOrderCommand.UserId
            };

            return await _orderRepository.AddAsync(order);
        }

        public Task<DeleteOrderStatus> DeleteAsync(DeleteOrderCommand deleteOrderCommand)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(UpdateOrderCommand updateOrderCommand)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<string>> AsCsvAsync(QueryOrders queryOrders)
        {
            var orders = await _orderRepository.GetAsync(queryOrders);

            var csv = new List<string>();

            Parallel.ForEach(orders, (order) => {
                csv.Add(order.ToCSV(";"));
            });

            return csv;
        }

        public async Task<IList<Order>> GetAsync(QueryOrders queryOrders)
        {
            var orders = await _orderRepository.GetAsync(queryOrders);

            return orders;
        }

        public async Task<ResultService<IEnumerable<Order>>> GetListByUserAsync(int userId)
        {
            var result = new ResultService<IEnumerable<Order>>();

            if (userId == 0) {
                result.Errors.Add("Identificador do usuário não informado.");
                return result;
            }

            var orders = await _orderRepository.GetByUserAsync(userId);

            result.Data = orders;

            return result;
        }
    }
}
