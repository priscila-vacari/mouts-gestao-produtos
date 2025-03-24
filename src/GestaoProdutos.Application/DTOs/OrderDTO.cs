using GestaoProdutos.Domain.Enum;

namespace GestaoProdutos.Application.DTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int ClientId { get; set; }

        public List<OrderItemDTO> Itens { get; set; } = new List<OrderItemDTO>();

        public decimal Tax { get; set; }

        public OrderStatus Status { get; set; }
    }
}
