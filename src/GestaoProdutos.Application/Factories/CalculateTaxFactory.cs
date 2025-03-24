using GestaoProdutos.Application.Interfaces;
using GestaoProdutos.Application.Strategies;
using Microsoft.FeatureManagement;

namespace GestaoProdutos.Application.Factories
{
    public class CalculateTaxFactory(IFeatureManager featureManager) : ICalculateTaxFactory
    {
        private readonly IFeatureManager _featureManager = featureManager;

        public async Task<ICalculateTaxStrategy> CreateStrategy()
        {
            bool newTaxRule = await _featureManager.IsEnabledAsync("TaxReform");
            return newTaxRule ? new CalculateTaxReform() : new CalculateTaxDefault();
        }
    }
}
