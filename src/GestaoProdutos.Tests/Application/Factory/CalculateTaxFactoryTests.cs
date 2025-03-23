using FluentAssertions;
using GestaoProdutos.Application.Factories;
using GestaoProdutos.Application.Strategies;
using Microsoft.FeatureManagement;
using NSubstitute;

namespace GestaoProdutos.Tests.Application.Factory
{
    public class CalculateTaxFactoryTests
    {
        private readonly IFeatureManager _featureManager;
        private readonly CalculateTaxFactory _factory;

        public CalculateTaxFactoryTests()
        {
            _featureManager = Substitute.For<IFeatureManager>();
            _factory = new CalculateTaxFactory(_featureManager);
        }

        [Fact]
        public async Task CreateStrategy_ShouldReturn_CalculateTaxReform_When_TaxReform_Enabled()
        {
            _featureManager.IsEnabledAsync("TaxReform").Returns(true);

            var result = await _factory.CreateStrategy();

            result.Should().BeOfType<CalculateTaxReform>();
        }

        [Fact]
        public async Task CreateStrategy_ShouldReturn_CalculateTaxDefault_Quando_TaxReform_When_TaxReform_Disabled()
        {
            _featureManager.IsEnabledAsync("TaxReform").Returns(false);

            var result = await _factory.CreateStrategy();

            result.Should().BeOfType<CalculateTaxDefault>();
        }

        [Fact]
        public async Task CreateStrategy_ShouldCall_IsEnabledAsync_FromFeatureManager_WithValidParams()
        {
            var featureName = "TaxReform";

            await _factory.CreateStrategy();

            await _featureManager.Received(1).IsEnabledAsync(featureName);
        }
    }
}
