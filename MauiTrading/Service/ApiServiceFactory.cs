using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiTrading.Service
{
    public class ApiServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ApiServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public IApiService<T> CreateService<T>(string type)
        {
            return type switch
            {
                "user" => _serviceProvider.GetRequiredKeyedService<IApiService<T>>("user"),
                "trade" => _serviceProvider.GetRequiredKeyedService<IApiService<T>>("trade"),
                "asset" => _serviceProvider.GetRequiredKeyedService<IApiService<T>>("asset"),
                "stocks" => _serviceProvider.GetRequiredKeyedService<IApiService<T>>("stocks"),
                "candle" => _serviceProvider.GetRequiredKeyedService<IApiService<T>>("candle"),
                "register" => _serviceProvider.GetRequiredKeyedService<IApiService<T>>("register"),
                "tradehistory" => _serviceProvider.GetRequiredKeyedService<IApiService<T>>("tradehistory"),
                "closetrade" => _serviceProvider.GetRequiredKeyedService<IApiService<T>>("closetrade"),
                _ => throw new ArgumentException($"Service of type '{type}' not found")
            };
        }

    }
}
