using GestaoProdutos.Domain.Enum;
using System.Text.Json.Serialization;

namespace GestaoProdutos.API.Models
{
    /// <summary>
    /// Classe de resposta da requisição do pedido
    /// </summary>
    public class OrderCreateResponseModel
    {
        /// <summary>
        /// Id 
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// Status do pedido
        /// </summary>
        [JsonPropertyName("status")]
        public OrderStatus Status { get; set; }
    }
}
