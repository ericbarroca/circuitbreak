using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;

namespace OrderService.CircuitBreak
{
    public interface IPolicyManager
    {
        AsyncCircuitBreakerPolicy GetCircuitBreaker(string name);
    }

    public class PolicyManager : IPolicyManager
    {
        private Dictionary<string, AsyncCircuitBreakerPolicy> policies;

        public PolicyManager()
        {
            policies = new Dictionary<string, AsyncCircuitBreakerPolicy>();
        }


        public AsyncCircuitBreakerPolicy GetCircuitBreaker(string name)
        {

            if (!policies.ContainsKey(name))
            {
                var policy = CreateDefaultCircuitBreakPolicy();
                policies.Add(name, policy);
            }

            return policies[name];
        }

        private AsyncCircuitBreakerPolicy CreateDefaultCircuitBreakPolicy()
        {
            return Policy
                    .Handle<Exception>()
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
        }
    }
}