using FluentValidation;
using GestaoProdutos.API.Models;

namespace GestaoProdutos.API.Validators
{
    /// <summary>
    /// Classe de validação de pedido
    /// </summary>
    public class OrderRequestModelValidator : AbstractValidator<OrderRequestModel>
    {
        /// <summary>
        /// Validador de pedido
        /// </summary>
        public OrderRequestModelValidator()
        {
            RuleFor(order => order.OrderId)
            .GreaterThan(0)
            .WithMessage("O PedidoId deve ser maior que zero.");

            RuleFor(order => order.ClientId)
                .GreaterThan(0)
                .WithMessage("O ClienteId deve ser maior que zero.");

            RuleFor(order => order.Itens)
                .NotNull()
                .WithMessage("A lista de itens não pode ser nula.")
                .NotEmpty()
                .WithMessage("A lista de itens deve conter pelo menos um item.");

            RuleForEach(order => order.Itens)
                .SetValidator(new OrderItemRequestModelValidator());
        }
    }
}
