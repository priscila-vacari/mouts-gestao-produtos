using GestaoProdutos.Application.Interfaces;
using GestaoProdutos.Domain.Entities;

namespace GestaoProdutos.Application.Strategies
{
    public class CalculateTaxReform : ICalculateTaxStrategy
    {
        public decimal Calculate(IEnumerable<OrderItem> itens)
        {
            return itens.Sum(i => i.Amount * i.Quantity) * 0.2m;
        }
    }
}
