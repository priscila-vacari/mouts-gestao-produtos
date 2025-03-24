namespace GestaoProdutos.Application.Interfaces
{
    public interface ICalculateTaxFactory
    {
        Task<ICalculateTaxStrategy> CreateStrategy();
    }
}
