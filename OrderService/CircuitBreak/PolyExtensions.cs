using System;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;

namespace OrderService.CircuitBreak
{
    public static class PolyExtensions
    {
        public static void AddCircutiBreaker(this IHttpClientBuilder builder)
        {

            var policyException = Policy
                .Handle<Exception>()
                // .OrResult<HttpResponseMessage>(r => r.StatusCode == HttpStatusCode.InternalServerError)
                .CircuitBreakerAsync(2, TimeSpan.FromMinutes(1),
                (ex, ts, context) =>
                {
                    var logger = context["logger"] as ILogger;
                    logger.LogWarning(ex, "Circuit Break");
                },
                (context) =>
                {

                    var logger = context["logger"] as ILogger;
                    logger.LogInformation("Circuit Closed");

                });


            builder.AddPolicyHandler(policyException.AsAsyncPolicy<HttpResponseMessage>());
        }
    }
}