using Bogus;
using GestaoProdutos.Application.DTOs;
using GestaoProdutos.Domain.Enum;

namespace GestaoProdutos.Tests.Fakes.DTO
{
    public class OrderDTOFake : Faker<OrderDTO>
    {
        public OrderDTOFake()
        {
            RuleFor(p => p.Id, f => f.Random.Int(1, 9999));
            RuleFor(p => p.OrderId, f => f.Random.Int(1000, 9999));
            RuleFor(p => p.ClientId, f => f.Random.Int(1000, 9999));
            RuleFor(p => p.Status, OrderStatus.Criado);
            RuleFor(p => p.Itens, f => new OrderItemDTOFake().Generate(3));
            RuleFor(p => p.Tax, f => f.Random.Decimal(10, 10000));
        }
    }
}
