using GestaoProdutos.Domain.Enum;
using System.Text.Json.Serialization;

namespace GestaoProdutos.API.Models
{
    /// <summary>
    /// Classe de resposta do pedido
    /// </summary>
    public class OrderResponseModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("pedidoId")]
        public int OrderId { get; set; }

        [JsonPropertyName("clienteId")]
        public int ClientId { get; set; }

        [JsonPropertyName("imposto")]
        public decimal Tax { get; set; }

        [JsonPropertyName("itens")]
        public List<OrderItemResponseModel> Itens { get; set; } = new List<OrderItemResponseModel>();

        [JsonPropertyName("status")]
        public OrderStatus Status { get; set; }
    }
}
