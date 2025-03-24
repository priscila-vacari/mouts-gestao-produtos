using GestaoProdutos.API.Models;
using GestaoProdutos.API.Validators;
using FluentValidation.TestHelper;
using GestaoProdutos.Tests.Fakes.Entities;

namespace GestaoProdutos.Tests.API.Validators
{
    public class OrderRequestModelValidatorTests
    {
        private readonly OrderRequestModelValidator _validator;

        public OrderRequestModelValidatorTests()
        {
            _validator = new OrderRequestModelValidator();
        }

        [Fact]
        public void Should_Have_Error_When_OrderId_Less_Than_Zero()
        {
            var model = new OrderRequestModelFake().Generate();
            model.OrderId = 0;

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.OrderId)
                .WithErrorMessage("O PedidoId deve ser maior que zero.");
        }

        [Fact]
        public void Should_Have_Error_When_ClientId_Less_Than_Zero()
        {
            var model = new OrderRequestModelFake().Generate();
            model.ClientId = 0;

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.ClientId)
                .WithErrorMessage("O ClienteId deve ser maior que zero.");
        }

        [Fact]
        public void Should_Have_Error_When_Itens_Is_Null()
        {
            var model = new OrderRequestModelFake().Generate();
            model.Itens = null;

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Itens)
                .WithErrorMessage("A lista de itens não pode ser nula.");
        }

        [Fact]
        public void Should_Have_Error_When_Itens_Equal_Zero()
        {
            var model = new OrderRequestModelFake().Generate();
            model.Itens = new List<OrderItemRequestModel>();

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Itens)
                .WithErrorMessage("A lista de itens deve conter pelo menos um item.");
        }

        [Fact]
        public void Should_Validate_When_Model_Is_Valid()
        {
            var model = new OrderRequestModelFake().Generate();

            var result = _validator.TestValidate(model);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
