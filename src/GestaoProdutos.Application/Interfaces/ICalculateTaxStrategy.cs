using GestaoProdutos.Domain.Entities;

namespace GestaoProdutos.Application.Interfaces
{
    public interface ICalculateTaxStrategy
    {
        decimal Calculate(IEnumerable<OrderItem> itens);
    }
}
