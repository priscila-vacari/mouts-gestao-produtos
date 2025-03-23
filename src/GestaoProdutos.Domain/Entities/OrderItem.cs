using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoProdutos.Domain.Entities
{
    [Table("order_items")]
    public class OrderItem
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal Amount { get; set; }

        public virtual Order Order { get; set; } = new Order();
    }
}
