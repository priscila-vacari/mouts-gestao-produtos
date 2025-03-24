using Bogus;
using GestaoProdutos.API.Models;

namespace GestaoProdutos.Tests.Fakes.Entities
{
    public class OrderItemRequestModelFake : Faker<OrderItemRequestModel>
    {
        public OrderItemRequestModelFake()
        {
            RuleFor(p => p.ProductId, f => f.Random.Int(1000, 9999));
            RuleFor(p => p.Quantity, f => f.Random.Int(1, 100));
            RuleFor(p => p.Amount, f => f.Random.Decimal(10, 1000));
            RuleFor(p => p.Amount, f => Math.Round(f.Random.Decimal(10, 1000), 2));
        }
    }
}
