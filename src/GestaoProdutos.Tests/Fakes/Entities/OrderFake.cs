using Bogus;
using GestaoProdutos.Domain.Entities;
using GestaoProdutos.Domain.Enum;

namespace GestaoProdutos.Tests.Fakes.Entities
{
    public class OrderFake : Faker<Order>
    {
        public OrderFake()
        {
            RuleFor(p => p.Id, f => f.Random.Int(1, 9999));
            RuleFor(p => p.OrderId, f => f.Random.Int(1000, 9999));
            RuleFor(p => p.ClientId, f => f.Random.Int(1000, 9999));
            RuleFor(p => p.Status, OrderStatus.Criado);
            RuleFor(p => p.Itens, f => new OrderItemFake().Generate(3));
            RuleFor(p => p.Tax, f => f.Random.Decimal(10, 10000));
        }
    }
}
