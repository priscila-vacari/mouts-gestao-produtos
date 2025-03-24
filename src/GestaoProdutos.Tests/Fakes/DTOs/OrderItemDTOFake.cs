using Bogus;
using GestaoProdutos.Application.DTOs;

namespace GestaoProdutos.Tests.Fakes.DTO
{
    public class OrderItemDTOFake : Faker<OrderItemDTO>
    {
        public OrderItemDTOFake()
        {
            RuleFor(p => p.Id, f => f.Random.Int(1, 9999));
            RuleFor(p => p.ProductId, f => f.Random.Int(1000, 9999));
            RuleFor(p => p.Quantity, f => f.Random.Int(1, 100));
            RuleFor(p => p.Amount, f => f.Random.Decimal(10, 1000));
        }
    }
}
