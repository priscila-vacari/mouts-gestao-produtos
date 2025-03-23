using GestaoProdutos.Domain.Enum;

namespace GestaoProdutos.Tests.Fakes.DTO
{
    public class OrderDTOFakeData : OrderDTOFake
    {
        public OrderDTOFakeData(int? id = null, OrderStatus? status = null)
        {
            if (id != null)
                RuleFor(p => p.Id, id);

            if (status != null)
                RuleFor(p => p.Status, status);
        }
    }
}
