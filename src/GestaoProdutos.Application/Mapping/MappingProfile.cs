using AutoMapper;
using GestaoProdutos.Application.DTOs;
using GestaoProdutos.Domain.Entities;

namespace GestaoProdutos.Application.Mapping
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<OrderDTO, Order>().ReverseMap();
            CreateMap<OrderItemDTO, OrderItem>().ReverseMap();
        }
    }
}
