using GestaoProdutos.Application.Strategies;
using FluentAssertions;
using GestaoProdutos.Domain.Entities;
using GestaoProdutos.Tests.Fakes.Entities;

namespace GestaoProdutos.Tests.Application.Strategies
{
    public class CalculateTaxReformTests
    {

        private readonly CalculateTaxReform _calculateTaxReform;

        public CalculateTaxReformTests()
        {
            _calculateTaxReform = new CalculateTaxReform();
        }

        [Fact]
        public void Calculate_ReturnsZero_When_ItensList_IsEmpty()
        {
            var emptyOrderItems = new List<OrderItem>();

            var result = _calculateTaxReform.Calculate(emptyOrderItems);

            result.Should().Be(0, "O imposto deve ser 0 quando não houver itens na lista");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        public void Calculate_ReturnsTaxValue_WhenOK(int qtGenerate)
        {
            var orderItems = new OrderItemFake().Generate(qtGenerate);

            var expectedTax = orderItems.Sum(i => i.Amount * i.Quantity) * 0.2m;

            var result = _calculateTaxReform.Calculate(orderItems);

            result.Should().Be(expectedTax, "O imposto deve ser calculado como a soma dos montantes X quantidade multiplicado por 0.2");
        }
    }
}
