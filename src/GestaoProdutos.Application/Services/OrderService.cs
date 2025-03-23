using AutoMapper;
using GestaoProdutos.Application.DTOs;
using GestaoProdutos.Application.Interfaces;
using GestaoProdutos.Domain.Entities;
using GestaoProdutos.Domain.Enum;
using GestaoProdutos.Domain.Exceptions;
using GestaoProdutos.Infra.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GestaoProdutos.Application.Services
{
    public class OrderService(ILogger<OrderService> logger, IMapper mapper, ICalculateTaxFactory calculateTaxFactory, IServiceProvider serviceProvider) : IOrderService
    {
        private readonly ILogger<OrderService> _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly ICalculateTaxFactory _calculateTaxFactory = calculateTaxFactory;
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public async Task<OrderDTO> GetByIdAsync(int id)
        {
            _logger.LogInformation("Obtendo o pedido de id {id}.", id);

            using var scope = _serviceProvider.CreateScope();
            var _orderRepository = scope.ServiceProvider.GetRequiredService<IRepository<Order>>();
            var order = await _orderRepository.GetByIdAsyncIncludes(id, i => i.Itens) ?? throw new NotFoundException("Pedido não encontrado para o id.");

            var response = _mapper.Map<OrderDTO>(order);
            return response;
        }

        public async Task<IEnumerable<OrderDTO>> GetByStatusAsync(OrderStatus status)
        {
            _logger.LogInformation("Obtendo todos os pedidos do status {status}.", status);

            using var scope = _serviceProvider.CreateScope();
            var _orderRepository = scope.ServiceProvider.GetRequiredService<IRepository<Order>>();
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

            using var scope = _serviceProvider.CreateScope();
            var _orderRepository = scope.ServiceProvider.GetRequiredService<IRepository<Order>>();

            var orderExists = await _orderRepository.GetWhereAsync( o => o.OrderId == order.OrderId && o.ClientId == order.ClientId);
            if (orderExists.Any())
                throw new DuplicateEntryException("Pedido já cadastrado.");

            var calculateStrategy = await _calculateTaxFactory.CreateStrategy();
            order.Tax = calculateStrategy.Calculate(order.Itens);

            await _orderRepository.AddAsync(order);

            var response = _mapper.Map<OrderDTO>(order);
            return response;
        }
    }
}
