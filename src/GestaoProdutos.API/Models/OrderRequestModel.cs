using System.Text.Json.Serialization;

namespace GestaoProdutos.API.Models
{
    /// <summary>
    /// Classe de requisição do pedido
    /// </summary>
    public class OrderRequestModel
    {
        [JsonPropertyName("pedidoId")]
        public int OrderId { get; set; }

        [JsonPropertyName("clienteId")]
        public int ClientId { get; set; }

        [JsonPropertyName("itens")]
        public List<OrderItemRequestModel> Itens { get; set; } = new List<OrderItemRequestModel>();
    }
}
