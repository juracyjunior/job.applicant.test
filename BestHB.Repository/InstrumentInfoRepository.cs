using BestHB.Domain.Entities;
using BestHB.Domain.Repositories;
using BestHB.Repository.InMemory;
using System;
using System.Threading.Tasks;
using BestHB.Domain.Queries;
using System.Collections.Generic;

namespace BestHB.Repository
{
    public class InstrumentInfoRepository : IInstrumentInfoRepository
    {
        public async Task<InstrumentInfo> GetAsync(string symbol)
        {
            var instrumentInfo = InMemoryIntrumentInfoRepository.Get(symbol);

            if(instrumentInfo == null)
            {
                instrumentInfo = new InstrumentInfo {
                    Description = "PETROBRAS",
                    Exchange = "BOVESPA",
                    ISIN = "ABCDE12345",
                    LotStep = 100,
                    MaxLot = 1000000,
                    MinLot = 100,
                    Symbol = "PETR4",
                    Type = InstrumentType.Stock
                };
                
                InMemoryIntrumentInfoRepository.Add(instrumentInfo);
            }

            return instrumentInfo;
        }
    }
}
