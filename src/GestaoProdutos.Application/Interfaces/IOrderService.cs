using GestaoProdutos.Application.DTOs;
using GestaoProdutos.Domain.Enum;

namespace GestaoProdutos.Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDTO> GetByIdAsync(int id);
        Task<IEnumerable<OrderDTO>> GetByStatusAsync(OrderStatus status);
        Task<OrderDTO> AddAsync(OrderDTO request);
    }
}
