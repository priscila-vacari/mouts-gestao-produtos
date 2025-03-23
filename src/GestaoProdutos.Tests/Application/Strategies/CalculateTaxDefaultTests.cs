using FluentAssertions;
using GestaoProdutos.Application.Strategies;
using GestaoProdutos.Domain.Entities;
using GestaoProdutos.Tests.Fakes.Entities;

namespace GestaoProdutos.Tests.Application.Strategies
{
    public class CalculateTaxDefaultTests
    {

        private readonly CalculateTaxDefault _calculateTaxDefault;

        public CalculateTaxDefaultTests()
        {
            _calculateTaxDefault = new CalculateTaxDefault();
        }

        [Fact]
        public void Calculate_ReturnsZero_When_ItensList_IsEmpty()
        {
            var emptyOrderItems = new List<OrderItem>();

            var result = _calculateTaxDefault.Calculate(emptyOrderItems);

            result.Should().Be(0, "O imposto deve ser 0 quando não houver itens na lista");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        public void Calculate_ReturnsTaxValue_WhenOK(int qtGenerate)
        {
            var orderItems = new OrderItemFake().Generate(qtGenerate);

            var expectedTax = orderItems.Sum(i => i.Amount * i.Quantity) * 0.3m;

            var result = _calculateTaxDefault.Calculate(orderItems);

            result.Should().Be(expectedTax, "O imposto deve ser calculado como a soma dos montantes X quantidade multiplicado por 0.3");
        }
    }
}
