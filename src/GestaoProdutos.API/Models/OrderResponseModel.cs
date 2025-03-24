using GestaoProdutos.Domain.Enum;
using System.Text.Json.Serialization;

namespace GestaoProdutos.API.Models
{
    /// <summary>
    /// Classe de resposta do pedido
    /// </summary>
    public class OrderResponseModel
    {
        /// <summary>
        /// Id
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// Id do pedido
        /// </summary>
        [JsonPropertyName("pedidoId")]
        public int OrderId { get; set; }

        /// <summary>
        /// Id do cliente
        /// </summary>
        [JsonPropertyName("clienteId")]
        public int ClientId { get; set; }

        /// <summary>
        /// Valor do imposto
        /// </summary>
        [JsonPropertyName("imposto")]
        public decimal Tax { get; set; }

        /// <summary>
        /// Itens
        /// </summary>
        [JsonPropertyName("itens")]
        public List<OrderItemResponseModel> Itens { get; set; } = [];

        /// <summary>
        /// Status
        /// </summary>
        [JsonPropertyName("status")]
        public OrderStatus Status { get; set; }
    }
}
