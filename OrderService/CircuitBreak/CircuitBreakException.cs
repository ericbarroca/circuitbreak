using System;

namespace OrderService.CircuitBreak
{
    public class CircuitBreakException : Exception
    {
        public CircuitBreakException(Exception innerException) :
        base("A circuit breaker was triggered.", innerException)
        {
        }
    }
}