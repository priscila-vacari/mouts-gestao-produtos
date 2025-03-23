using GestaoProdutos.API.Validators;
using FluentValidation.TestHelper;
using GestaoProdutos.Tests.Fakes.Entities;

namespace GestaoProdutos.Tests.API.Validators
{
    public class OrderItemRequestModelValidatorTests
    {
        private readonly OrderItemRequestModelValidator _validator;

        public OrderItemRequestModelValidatorTests()
        {
            _validator = new OrderItemRequestModelValidator();
        }

        [Fact]
        public void Should_Have_Error_When_ProductId_Less_Than_Zero()
        {
            var model = new OrderItemRequestModelFake().Generate();
            model.ProductId = 0;

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.ProductId)
                .WithErrorMessage("O ProductId deve ser maior que zero.");
        }

        [Fact]
        public void Should_Have_Error_When_Quantity_Less_Than_Zero()
        {
            var model = new OrderItemRequestModelFake().Generate();
            model.Quantity = 0;

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Quantity)
                .WithErrorMessage("A Quantidade deve ser maior que zero.");
        }

        [Fact]
        public void Should_Have_Error_When_Amount_Less_Than_Zero()
        {
            var model = new OrderItemRequestModelFake().Generate();
            model.Amount = 0;

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Amount)
                .WithErrorMessage("O Valor deve ser maior que zero.");
        }

        [Fact]
        public void Should_Have_Error_When_Amount_Not_In_PrecisionScale()
        {
            var model = new OrderItemRequestModelFake().Generate();
            model.Amount = 999999999.99m;

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Amount)
                .WithErrorMessage("O Valor deve ter no máximo 10 dígitos e 2 casas decimais.");
        }

        [Fact]
        public void Should_Validate_When_Model_Is_Valid()
        {
            var model = new OrderItemRequestModelFake().Generate();

            var result = _validator.TestValidate(model);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
