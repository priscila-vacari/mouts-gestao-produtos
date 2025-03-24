using FluentValidation;
using GestaoProdutos.API.Models;

namespace GestaoProdutos.API.Validators
{
    /// <summary>
    /// Classe de validação do item do pedido
    /// </summary>
    public class OrderItemRequestModelValidator : AbstractValidator<OrderItemRequestModel>
    {
        /// <summary>
        /// Validador de item do pedido
        /// </summary>
        public OrderItemRequestModelValidator()
        {
            RuleFor(item => item.ProductId)
                .GreaterThan(0)
                .WithMessage("O ProductId deve ser maior que zero.");

            RuleFor(item => item.Quantity)
                .GreaterThan(0)
                .WithMessage("A Quantidade deve ser maior que zero.");

            RuleFor(x => x.Amount)
               .GreaterThan(0).WithMessage("O Valor deve ser maior que zero.")
               .PrecisionScale(10, 2, true).WithMessage("O Valor deve ter no máximo 10 dígitos e 2 casas decimais.");
        }
    }
}
