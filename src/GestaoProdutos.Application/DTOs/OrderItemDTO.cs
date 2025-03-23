﻿namespace GestaoProdutos.Application.DTOs
{
    public class OrderItemDTO
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal Amount { get; set; }
    }
}
