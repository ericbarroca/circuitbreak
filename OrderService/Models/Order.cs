using OrderService.Models;

namespace OrderService.Models
{
    public record Order
    {
        public int Id { get; set; }

        public User User { get; set; }

    }
}