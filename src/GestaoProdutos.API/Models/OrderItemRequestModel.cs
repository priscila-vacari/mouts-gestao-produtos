using System.Text.Json.Serialization;

namespace GestaoProdutos.API.Models
{
    /// <summary>
    /// Classe de requisição do item do pedido
    /// </summary>
    public class OrderItemRequestModel
    {
        /// <summary>
        /// Id do produto
        /// </summary>
        [JsonPropertyName("produtoId")]
        public int ProductId { get; set; }

        /// <summary>
        /// Quantidade
        /// </summary>
        [JsonPropertyName("quantidade")]
        public int Quantity { get; set; }

        /// <summary>
        /// Valor
        /// </summary>
        [JsonPropertyName("valor")]
        public decimal Amount { get; set; }
    }
}
