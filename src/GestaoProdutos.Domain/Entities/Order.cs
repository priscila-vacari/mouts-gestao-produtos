using GestaoProdutos.Domain.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoProdutos.Domain.Entities
{
    [Table("orders")]
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int ClientId { get; set; }

        public decimal Tax { get; set; }

        public OrderStatus Status { get; set; }

        public virtual List<OrderItem> Itens { get; set; } = new List<OrderItem>();
    }
}
