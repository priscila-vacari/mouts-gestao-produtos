using AutoMapper;
using FluentAssertions;
using GestaoProdutos.API.Controllers.v1;
using GestaoProdutos.API.Models;
using GestaoProdutos.Application.DTOs;
using GestaoProdutos.Application.Interfaces;
using GestaoProdutos.Domain.Enum;
using GestaoProdutos.Tests.Fakes.DTO;
using GestaoProdutos.Tests.Fakes.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace GestaoProdutos.Tests.API.Controllers
{
    public class OrderControllerTests
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IMapper _mapper;
        private readonly IOrderService _orderService;
        private readonly OrderController _controller;

        public OrderControllerTests()
        {
            _logger = Substitute.For<ILogger<OrderController>>();
            _mapper = Substitute.For<IMapper>();
            _orderService = Substitute.For<IOrderService>();
            _controller = new OrderController(_logger, _mapper, _orderService);
        }

        [Theory]
        [InlineData(1)]
        public async Task GetOrderById_ReturnsOkResult_WhenOrderExists(int id)
        {
            var order = new OrderDTOFakeData(id: id).Generate();
            _orderService.GetByIdAsync(id).Returns(order);

            var orderResponse = new OrderResponseModel { Id = id, OrderId = order.OrderId, ClientId = order.ClientId, Tax = order.Tax, Status = order.Status };
            _mapper.Map<OrderResponseModel>(order).Returns(orderResponse);

            var result = await _controller.GetOrderById(id);

            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(orderResponse);
        }

        [Theory]
        [InlineData(OrderStatus.Criado)]
        public async Task GetOrderByStatus_ReturnsOkResult_WhenOrdersExists(OrderStatus status)
        {
            var orders = new OrderDTOFakeData(status: status).Generate(3);
            _orderService.GetByStatusAsync(status).Returns(orders);

            List<OrderResponseModel> ordersResponse = [.. orders.Select(o => new OrderResponseModel { Id = o.Id, OrderId = o.OrderId, ClientId = o.ClientId, Tax = o.Tax, Status = o.Status })];
            _mapper.Map<IEnumerable<OrderResponseModel>>(orders).Returns(ordersResponse);

            var result = await _controller.GetOrderByStatus(status);

            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(ordersResponse);
        }

        [Fact]
        public async Task GetOrderByStatus_ReturnsBadRequest_WhenInvalidStatus()
        {
            var invalidStatus = (OrderStatus)999;

            var result = await _controller.GetOrderByStatus(invalidStatus);

            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult.StatusCode.Should().Be(400);
            badRequestResult.Value.Should().Be("O status informado não é válido.");
        }

        [Fact]
        public async Task AddOrder_ReturnsCreatedAtAction_WhenValidOrder()
        {
            int id = 1;
            var request = new OrderRequestModelFake().Generate();
            var orderDto = new OrderDTOFakeData(id: id).Generate();

            var createdOrder = new OrderCreateResponseModel { Id = orderDto.Id, Status = orderDto.Status };

            _mapper.Map<OrderDTO>(request).Returns(orderDto);
            _orderService.AddAsync(orderDto).Returns(orderDto);
            _mapper.Map<OrderCreateResponseModel>(orderDto).Returns(createdOrder);

            var result = await _controller.AddOrder(request);

            var createdResult = result as CreatedAtActionResult;
            createdResult.Should().NotBeNull();
            createdResult.StatusCode.Should().Be(201);
            createdResult.Value.Should().BeEquivalentTo(createdOrder);
        }

        [Theory]
        [InlineData(1)]
        public async Task UpdateOrder_ReturnsNoContentResult_WhenOrderExists(int id)
        {
            var request = new OrderRequestModelFake().Generate();
            var orderDto = new OrderDTOFakeData(id: id).Generate();

            var createdOrder = new OrderCreateResponseModel { Id = orderDto.Id, Status = orderDto.Status };

            _mapper.Map<OrderDTO>(request).Returns(orderDto);
            _orderService.UpdateAsync(id, orderDto).Returns(Task.CompletedTask);
            _mapper.Map<OrderCreateResponseModel>(orderDto).Returns(createdOrder);

            var result = await _controller.UpdateOrder(id, request);

            var noContentResult = result as NoContentResult;
            noContentResult.Should().NotBeNull();
            noContentResult.StatusCode.Should().Be(204);
        }

        [Theory]
        [InlineData(1)]
        public async Task DeleteOrderById_ReturnsNoContentResult_WhenOrderExists(int id)
        {
            _orderService.DeleteAsync(id).Returns(Task.CompletedTask);

            var result = await _controller.DeleteOrderById(id);

            var noContentResult = result as NoContentResult;
            noContentResult.Should().NotBeNull();
            noContentResult.StatusCode.Should().Be(204);
        }

    }
}