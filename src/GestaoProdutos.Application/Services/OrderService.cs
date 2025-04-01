using AutoMapper;
using GestaoProdutos.Application.DTOs;
using GestaoProdutos.Application.Interfaces;
using GestaoProdutos.Domain.Entities;
using GestaoProdutos.Domain.Enum;
using GestaoProdutos.Domain.Exceptions;
using GestaoProdutos.Infra.Interfaces;
using Microsoft.Extensions.Logging;

namespace GestaoProdutos.Application.Services
{
    public class OrderService(ILogger<OrderService> logger, IMapper mapper, ICalculateTaxFactory calculateTaxFactory, IRepository<Order> orderRepository, IRepository<OrderItem> orderItemRepository) : IOrderService
    {
        private readonly ILogger<OrderService> _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly ICalculateTaxFactory _calculateTaxFactory = calculateTaxFactory;
        private readonly IRepository<Order> _orderRepository = orderRepository;
        private readonly IRepository<OrderItem> _orderItemRepository = orderItemRepository;

        public async Task<OrderDTO> GetByIdAsync(int id)
        {
            _logger.LogInformation("Obtendo o pedido de id {id}.", id);

            var order = await _orderRepository.GetByIdAsyncIncludes(id, i => i.Itens) ?? throw new NotFoundException("Pedido não encontrado para o id.");

            var response = _mapper.Map<OrderDTO>(order);
            return response;
        }

        public async Task<IEnumerable<OrderDTO>> GetByStatusAsync(OrderStatus status)
        {
            _logger.LogInformation("Obtendo todos os pedidos do status {status}.", status);

            var orders = await _orderRepository.GetWhereAsyncIncludes(r => r.Status == status, i => i.Itens);

            if (!orders.Any())
                throw new NotFoundException("Pedidos não encontrados para o status.");

            var response = _mapper.Map<IEnumerable<OrderDTO>>(orders);
            return response;
        }

        public async Task<OrderDTO> AddAsync(OrderDTO request)
        {
            _logger.LogInformation("Criando novo pedido: {PedidoId}", request.OrderId);
            var order = _mapper.Map<Order>(request);

            await ValidateDuplicateOrder(order);

            order.Tax = await CalculateTax(order.Itens);

            await _orderRepository.AddAsync(order);

            var response = _mapper.Map<OrderDTO>(order);
            return response;
        }

        public async Task UpdateAsync(int id, OrderDTO request)
        {
            _logger.LogInformation("Atualizando o pedido: {id}", id);

            var order = await _orderRepository.GetByIdAsyncIncludes(id, i => i.Itens) ?? throw new NotFoundException("Pedido não encontrado para o id.");
            await DeleteItems(order.Itens);

            order = _mapper.Map<Order>(request);
            order.Id = id;

            order.Tax = await CalculateTax(order.Itens);

            await _orderRepository.UpdateAsync(order);
        }

        public async Task DeleteAsync(int id)
        {
            _logger.LogInformation("Excluindo o pedido de id {id}.", id);

            var order = await _orderRepository.GetByIdAsync(id) ?? throw new NotFoundException("Pedido não encontrado para o id.");

            await _orderRepository.DeleteAsync(order.Id);
        }

        private async Task DeleteItems(List<OrderItem> itens)
        {
            await _orderItemRepository.DeleteRangeAsync(itens);
        }

        private async Task ValidateDuplicateOrder(Order order)
        {
            var orderExists = await _orderRepository.GetWhereAsync(o => o.OrderId == order.OrderId && o.ClientId == order.ClientId);
            if (orderExists.Any())
                throw new DuplicateEntryException("Pedido já cadastrado.");
        }

        private async Task<decimal> CalculateTax(List<OrderItem> itens)
        {
            var calculateStrategy = await _calculateTaxFactory.CreateStrategy();
            return calculateStrategy.Calculate(itens);
        }
    }
}
