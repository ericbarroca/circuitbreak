using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrderService.Models;
using OrderService.Settings;
using Polly;

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

        public OrdersService(IOptions<OrdersSettings> settings,
        IHttpClientFactory httpClientFactory,
        ILogger<OrdersService> logger)
        {
            _settings = settings.Value;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<Order> LoadOrder(int id)
        {
            try
            {
                Order order = _settings.Orders.SingleOrDefault(o => o.Id == id) with { };
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get,
                $"user/{order.User.Id}");

                Context context = new Context();
                context["logger"] = _logger;
                request.SetPolicyExecutionContext(context);

                HttpClient userServiceClient = _httpClientFactory.CreateClient("userclient");
                HttpResponseMessage response = await userServiceClient.SendAsync(request);

                if (response.StatusCode == HttpStatusCode.OK)
                    order.User = await response.Content.ReadFromJsonAsync<User>();


                return order;
            }
            catch
            {
                return null;
            }
        }
    }
}