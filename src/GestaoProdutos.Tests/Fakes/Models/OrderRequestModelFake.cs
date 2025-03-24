using Bogus;
using GestaoProdutos.API.Models;

namespace GestaoProdutos.Tests.Fakes.Entities
{
    public class OrderRequestModelFake : Faker<OrderRequestModel>
    {
        public OrderRequestModelFake()
        {
            RuleFor(p => p.OrderId, f => f.Random.Int(1000, 9999));
            RuleFor(p => p.ClientId, f => f.Random.Int(1000, 9999));
            RuleFor(p => p.Itens, f => new OrderItemRequestModelFake().Generate(3));
        }
    }
}
