using AutoMapper;
using GestaoProdutos.Application.DTOs;
using GestaoProdutos.Domain.Entities;
using FluentAssertions;
using GestaoProdutos.Application.Mapping;
using GestaoProdutos.Tests.Fakes.DTO;

namespace GestaoProdutos.Tests.Application.Mapping
{
    public class MappingProfileTests
    {
        private readonly IConfigurationProvider _configurationProvider;
        private readonly IMapper _mapper;

        public MappingProfileTests()
        {
            _configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = _configurationProvider.CreateMapper();
        }

        [Fact]
        public void DoMapping_OrderDTO_To_Order()
        {
            var dto = new OrderDTOFake().Generate();

            var order = _mapper.Map<Order>(dto);

            order.Should().NotBeNull();
            order.OrderId.Should().Be(dto.OrderId);
            order.ClientId.Should().Be(dto.ClientId);
            order.Itens.Should().HaveCount(dto.Itens.Count);
        }

        [Fact]
        public void DoMapping_OrderItemDTO_To_OrderItem()
        {
            var dto = new OrderItemDTOFake().Generate();

            var orderItem = _mapper.Map<OrderItemDTO>(dto);

            orderItem.Should().NotBeNull();
            orderItem.ProductId.Should().Be(dto.ProductId);
            orderItem.Quantity.Should().Be(dto.Quantity);
            orderItem.Amount.Should().Be(dto.Amount);
        }
    }
}
