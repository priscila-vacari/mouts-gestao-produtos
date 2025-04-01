using AutoMapper;
using FluentAssertions;
using GestaoProdutos.Application.DTOs;
using GestaoProdutos.Application.Interfaces;
using GestaoProdutos.Application.Services;
using GestaoProdutos.Domain.Entities;
using GestaoProdutos.Domain.Enum;
using GestaoProdutos.Domain.Exceptions;
using GestaoProdutos.Infra.Interfaces;
using GestaoProdutos.Tests.Fakes.DTO;
using GestaoProdutos.Tests.Fakes.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Linq.Expressions;

namespace GestaoProdutos.Tests.Application.Services
{
    public class OrderServiceTests
    {
        private readonly ILogger<OrderService> _logger;
        private readonly IMapper _mapper;
        private readonly ICalculateTaxFactory _calculateTaxFactory;
        private readonly ICalculateTaxStrategy _calculateTaxStrategy;
        private readonly IServiceProvider _serviceProvider;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<OrderItem> _orderItemRepository;
        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            _logger = Substitute.For<ILogger<OrderService>>();
            _mapper = Substitute.For<IMapper>();
            _calculateTaxFactory = Substitute.For<ICalculateTaxFactory>();
            _calculateTaxStrategy = Substitute.For<ICalculateTaxStrategy>();
            _serviceProvider = Substitute.For<IServiceProvider>();
            _orderRepository = Substitute.For<IRepository<Order>>();
            _orderItemRepository = Substitute.For<IRepository<OrderItem>>();

            var serviceScope = Substitute.For<IServiceScope>();
            serviceScope.ServiceProvider.Returns(_serviceProvider);

            var scopeFactory = Substitute.For<IServiceScopeFactory>();
            scopeFactory.CreateScope().Returns(serviceScope);

            _serviceProvider.GetService(typeof(IServiceScopeFactory)).Returns(scopeFactory);
            _serviceProvider.GetService(typeof(IRepository<Order>)).Returns(_orderRepository);

            _orderService = new OrderService(_logger, _mapper, _calculateTaxFactory, _orderRepository, _orderItemRepository);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturn_OrderDTO_WhenOrderExists()
        {
            var id = 1;
            var order = new OrderFake().Generate();

            var orderDTO = new OrderDTO { Id = id, OrderId = order.OrderId, ClientId = order.ClientId, Tax = order.Tax, Status = order.Status };

            _orderRepository.GetByIdAsyncIncludes(id, Arg.Any<Expression<Func<Order, object>>[]>()).Returns(Task.FromResult(order));
            _mapper.Map<OrderDTO>(order).Returns(orderDTO);

            var result = await _orderService.GetByIdAsync(id);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(orderDTO);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNotFoundException_WhenOrderNotExists()
        {
            var id = 1;
            Order? order = null;

            _orderRepository.GetByIdAsyncIncludes(id, Arg.Any<Expression<Func<Order, object>>[]>()).Returns(Task.FromResult(order));

            var act = async () => await _orderService.GetByIdAsync(id);

            await act.Should().ThrowAsync<NotFoundException>().WithMessage("Pedido não encontrado para o id.");
        }

        [Fact]
        public async Task GetByStatusAsync_ShouldReturn_OrdersDTO_WhenOrdersExists()
        {
            OrderStatus status = OrderStatus.Criado;
            var orders = new OrderFake().Generate(3);

            var ordersDto = new OrderDTOFake().Generate(3);

            _orderRepository.GetWhereAsyncIncludes(Arg.Any<Expression<Func<Order, bool>>>(), Arg.Any<Expression<Func<Order, object>>[]>()).Returns(orders);
            _mapper.Map<IEnumerable<OrderDTO>>(orders).Returns(ordersDto);

            var result = await _orderService.GetByStatusAsync(status);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(ordersDto);
        }

        [Fact]
        public async Task GetByStatusAsync_ShouldReturnNotFoundException_WhenOrderNotExists()
        {
            OrderStatus status = OrderStatus.Criado;
            List<Order> orders = [];

            _orderRepository.GetWhereAsyncIncludes(Arg.Any<Expression<Func<Order, bool>>>(), Arg.Any<Expression<Func<Order, object>>[]>()).Returns(orders);

            var act = async () => await _orderService.GetByStatusAsync(status);

            await act.Should().ThrowAsync<NotFoundException>().WithMessage("Pedidos não encontrados para o status.");
        }

        [Fact]
        public async Task AddAsync_ShouldAddNewOrder_WhenIsValid()
        {
            var id = 1;
            var orderDto = new OrderDTOFake().Generate();

            var order = new Order { Id = id, OrderId = orderDto.OrderId, ClientId = orderDto.ClientId, Tax = orderDto.Tax, Status = orderDto.Status, 
                Itens = [.. orderDto.Itens.Select(i => new OrderItem { Id = i.Id, ProductId = i.ProductId, Quantity = i.Quantity, Amount = i.Amount })]
            };

            _mapper.Map<Order>(orderDto).Returns(order);

            _orderRepository.GetWhereAsync(Arg.Any<Expression<Func<Order, bool>>>()).Returns([]);

            _calculateTaxFactory.CreateStrategy().Returns(_calculateTaxStrategy);

            _mapper.Map<OrderDTO>(order).Returns(orderDto);

            var result = await _orderService.AddAsync(orderDto);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(orderDto);
            await _orderRepository.Received(1).AddAsync(order);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnDuplicateEntryException_WhenOrderExists()
        {
            var id = 1;
            var orderDto = new OrderDTOFake().Generate();
            orderDto.Id = id;

            var ordersExists = new OrderFake().Generate(3);

            _orderRepository.GetWhereAsync(Arg.Any<Expression<Func<Order, bool>>>()).Returns(ordersExists);

            var act = async () => await _orderService.AddAsync(orderDto);

            await act.Should().ThrowAsync<DuplicateEntryException>().WithMessage("Pedido já cadastrado.");
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateOrder_WhenIsValid()
        {
            var id = 1;
            var orderDto = new OrderDTOFake().Generate();

            var order = new Order
            {
                Id = id,
                OrderId = orderDto.OrderId,
                ClientId = orderDto.ClientId,
                Tax = orderDto.Tax,
                Status = orderDto.Status,
                Itens = [.. orderDto.Itens.Select(i => new OrderItem { Id = i.Id, ProductId = i.ProductId, Quantity = i.Quantity, Amount = i.Amount })]
            };

            _mapper.Map<Order>(orderDto).Returns(order);

            _orderRepository.GetByIdAsyncIncludes(id, Arg.Any<Expression<Func<Order, object>>[]>()).Returns(Task.FromResult(order));

            _calculateTaxFactory.CreateStrategy().Returns(_calculateTaxStrategy);

            _mapper.Map<OrderDTO>(order).Returns(orderDto);

            await _orderService.UpdateAsync(order.Id, orderDto);

            await _orderRepository.Received(1).UpdateAsync(order);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnNotFoundException_WhenOrderNotExists()
        {
            var id = 1;
            var orderDto = new OrderDTOFake().Generate();
            orderDto.Id = id;

            var ordersExists = new OrderFake().Generate(3);

            _orderRepository.GetWhereAsyncIncludes(Arg.Any<Expression<Func<Order, bool>>>(), Arg.Any<Expression<Func<Order, object>>[]>()).Returns(ordersExists);

            var act = async () => await _orderService.UpdateAsync(id, orderDto);

            await act.Should().ThrowAsync<NotFoundException>().WithMessage("Pedido não encontrado para o id.");
        }

        [Fact]
        public async Task DeleteAsyncAsync_ShouldReturnSuccess_WhenOrderExists()
        {
            var id = 1;
            var order = new OrderFake().Generate();

            var orderDTO = new OrderDTO { Id = id, OrderId = order.OrderId, ClientId = order.ClientId, Tax = order.Tax, Status = order.Status };

            _orderRepository.GetByIdAsync(id).Returns(Task.FromResult(order));
            _mapper.Map<OrderDTO>(order).Returns(orderDTO);

            await _orderService.DeleteAsync(id);

            await _orderRepository.Received(1).DeleteAsync(order.Id);
        }

        [Fact]
        public async Task DeleteAsyncAsync_ShouldReturnNotFoundException_WhenOrderNotExists()
        {
            var id = 1;
            Order? order = null;

            _orderRepository.GetByIdAsync(id).Returns(Task.FromResult(order));

            var act = async () => await _orderService.GetByIdAsync(id);

            await act.Should().ThrowAsync<NotFoundException>().WithMessage("Pedido não encontrado para o id.");
        }

    }
}