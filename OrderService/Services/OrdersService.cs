using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrderService.CircuitBreak;
using OrderService.Models;
using OrderService.Settings;
using Polly;
using Polly.CircuitBreaker;

namespace OrderService.Services
{
    public interface IOrdersService
    {
        Task<Order> LoadOrder(int id);
    }

    public class OrdersService : IOrdersService
    {
        private readonly OrdersSettings _settings;

        private readonly IHttpClientFactory _httpClientFactory;

        private readonly ILogger<OrdersService> _logger;

        private readonly IPolicyManager _policyManager;

        public OrdersService(IOptions<OrdersSettings> settings,
        IHttpClientFactory httpClientFactory,
        IPolicyManager policyManager,
        ILogger<OrdersService> logger)
        {
            _settings = settings.Value;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _policyManager = policyManager;
        }

        public async Task<Order> LoadOrder(int id)
        {
            try
            {
                var policy = _policyManager.GetCircuitBreaker("usercb");

                //Context object for policy logging
                var datacontext = new Dictionary<string, object>();
                datacontext.Add("logger", _logger);

                PolicyResult<Order> policyResult = await policy.ExecuteAndCaptureAsync<Order>(async (context) =>
               {
                   //Expensive pre operation or calculation
                   Thread.Sleep(3000);
                   
                   Order order = _settings.Orders.SingleOrDefault(o => o.Id == id) with { };
                   HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get,
                   $"user/{order.User.Id}");

                   HttpClient userServiceClient = _httpClientFactory.CreateClient("userclient");
                   HttpResponseMessage response = await userServiceClient.SendAsync(request);

                   if (response.StatusCode == HttpStatusCode.OK)
                       order.User = await response.Content.ReadFromJsonAsync<User>();

                   return order;

               }, datacontext);

                if (policy.CircuitState == CircuitState.Open)
                    throw new CircuitBreakException(policyResult.FinalException);

                return policyResult.Result;

            }
            catch
            {
                //In a real scenario the error would be treated
                return null;
            }
        }
    }
}