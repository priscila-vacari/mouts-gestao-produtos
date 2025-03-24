using System.Text.Json.Serialization;

namespace GestaoProdutos.API.Models
{
    /// <summary>
    /// Classe de requisição do pedido
    /// </summary>
    public class OrderRequestModel
    {
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
        /// Itens do pedido
        /// </summary>
        [JsonPropertyName("itens")]
        public List<OrderItemRequestModel> Itens { get; set; } = [];
    }
}
