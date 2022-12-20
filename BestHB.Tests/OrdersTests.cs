using BestHB.Domain.Commands;
using BestHB.Domain.Entities;
using BestHB.Domain.Repositories;
using BestHB.Domain.Service;
using BestHB.Domain.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BestHB.Tests
{
    [TestClass]
    public class OrdersTest
    {
        [TestMethod]
        public async Task invalid_lot_size_on_create_order_test()
        {
            //given
            var command = new CreateOrderCommand {
                Price = 10,
                Quantity = 40,
                Side = Domain.Commands.OrderSide.Sell,
                Symbol = "PETR4",
                Type = Domain.Commands.OrderType.Market,
                UserId = 123
            };

            var instrumentInfo = new InstrumentInfo
            {
                Type = InstrumentType.Stock,
                Symbol = "PETR4",
                Description = "PETROBRAS",
                Exchange = "BOVESPA",
                ISIN = "123456",
                LotStep = 100,
                MaxLot = 100000,
                MinLot = 100
            };

            
            var instrumentInfoRepositoryMock = new Mock<IInstrumentInfoRepository>();

            instrumentInfoRepositoryMock.Setup(i => i.GetAsync(It.IsAny<string>())).ReturnsAsync(instrumentInfo);

            var orderRepositoryMock = new Mock<IOrderRepository>();

            var message = string.Empty;

            try
            {
                var orderService = new OrderService(orderRepositoryMock.Object, instrumentInfoRepositoryMock.Object);
                
                await orderService.CreateAsync(command);
            }
            catch(Exception ex)
            {
                message = ex.Message;
            }

            Assert.AreEqual("Quantidade inválida.", message);
        }

        [TestMethod]
        public async Task OrderSerive_GetListByUserAsync_UserIdInvalid_ReturnError()
        {
            var instrumentInfoRepositoryMock = new Mock<IInstrumentInfoRepository>();

            var orderRepositoryMock = new Mock<IOrderRepository>();

            var orderService = new OrderService(orderRepositoryMock.Object, instrumentInfoRepositoryMock.Object);

            var result = await orderService.GetListByUserAsync(0);

            Assert.IsTrue(result.HasError);
        }

        [TestMethod]
        public async Task OrderSerive_GetListByUserAsync_UserIdValid_ReturnSuccess()
        {
            var userId = 1;
            var orders = new List<Order> 
            {
                new Order
                {
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
                }
            };

            var instrumentInfoRepositoryMock = new Mock<IInstrumentInfoRepository>();

            var orderRepositoryMock = new Mock<IOrderRepository>();
            orderRepositoryMock.Setup(o=>o.GetByUserAsync(userId)).ReturnsAsync(orders);

            var orderService = new OrderService(orderRepositoryMock.Object, instrumentInfoRepositoryMock.Object);

            var result = await orderService.GetListByUserAsync(userId);

            Assert.IsFalse(result.HasError);
            Assert.IsTrue(result.Data.Any());
        }
    }
}
