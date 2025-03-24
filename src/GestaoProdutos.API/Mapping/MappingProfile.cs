using AutoMapper;
using GestaoProdutos.API.Models;
using GestaoProdutos.Application.DTOs;
using System.Diagnostics.CodeAnalysis;

namespace GestaoProdutos.API.Mapping
{
    /// <summary>
    /// Mapeamento de modelos
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class MappingProfile: Profile
    {
        /// <summary>
        /// Perfis de mapeamento
        /// </summary>
        public MappingProfile()
        {
            CreateMap<OrderRequestModel, OrderDTO>().ReverseMap();
            CreateMap<OrderResponseModel, OrderDTO>().ReverseMap();
            CreateMap<OrderCreateResponseModel, OrderDTO>().ReverseMap();
            CreateMap<OrderItemRequestModel, OrderItemDTO>().ReverseMap();
            CreateMap<OrderItemResponseModel, OrderItemDTO>().ReverseMap();
        }
    }
}
