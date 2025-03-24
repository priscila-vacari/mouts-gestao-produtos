using AutoMapper;
using FluentAssertions;
using GestaoProdutos.API.Mapping;
using GestaoProdutos.API.Models;
using GestaoProdutos.Application.DTOs;
using GestaoProdutos.Tests.Fakes.DTO;
using GestaoProdutos.Tests.Fakes.Entities;

namespace GestaoProdutos.Tests.API.Mapping
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
        public void DoMapping_OrderRequestModel_To_OrderDTO()
        {
            var requestModel = new OrderRequestModelFake().Generate();

            var dto = _mapper.Map<OrderDTO>(requestModel);

            dto.Should().NotBeNull();
            dto.OrderId.Should().Be(requestModel.OrderId);
            dto.ClientId.Should().Be(requestModel.ClientId);
            dto.Itens.Should().HaveCount(requestModel.Itens.Count);
        }

        [Fact]
        public void DoMapping_OrderDTO_Para_OrderResponseModel()
        {
            var orderDto = new OrderDTOFake().Generate();

            var responseModel = _mapper.Map<OrderResponseModel>(orderDto);

            responseModel.Should().NotBeNull();
            responseModel.Id.Should().Be(orderDto.Id);
            responseModel.OrderId.Should().Be(orderDto.OrderId);
            responseModel.ClientId.Should().Be(orderDto.ClientId);
            responseModel.Tax.Should().Be(orderDto.Tax);
            responseModel.Status.Should().Be(orderDto.Status);
            responseModel.Itens.Should().HaveCount(orderDto.Itens.Count);
        }

        [Fact]
        public void DoMapping_OrderDTO_Para_OrderCreateResponseModel()
        {
            var orderDto = new OrderDTOFake().Generate();

            var responseModel = _mapper.Map<OrderCreateResponseModel>(orderDto);

            responseModel.Should().NotBeNull();
            responseModel.Id.Should().Be(orderDto.Id);
            responseModel.Status.Should().Be(orderDto.Status);
        }

        [Fact]
        public void DoMapping_OrderItemRequestModel_To_OrderItemDTO()
        {
            var requestModel = new OrderItemRequestModelFake().Generate();

            var dto = _mapper.Map<OrderItemDTO>(requestModel);

            dto.Should().NotBeNull();
            dto.ProductId.Should().Be(requestModel.ProductId);
            dto.Quantity.Should().Be(requestModel.Quantity);
            dto.Amount.Should().Be(requestModel.Amount);
        }

        [Fact]
        public void DoMapping_OrderItemDTO_To_OrderItemResponseModel()
        {
            var requestModel = new OrderItemDTOFake().Generate();

            var dto = _mapper.Map<OrderItemResponseModel>(requestModel);

            dto.Should().NotBeNull();
            dto.ProductId.Should().Be(requestModel.ProductId);
            dto.Quantity.Should().Be(requestModel.Quantity);
            dto.Amount.Should().Be(requestModel.Amount);
        }
    }
}
