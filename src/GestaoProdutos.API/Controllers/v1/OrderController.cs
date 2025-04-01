using Asp.Versioning;
using AutoMapper;
using GestaoProdutos.API.Models;
using GestaoProdutos.Application.DTOs;
using GestaoProdutos.Application.Interfaces;
using GestaoProdutos.Domain.Enum;
using Microsoft.AspNetCore.Mvc;

namespace GestaoProdutos.API.Controllers.v1
{
    /// <summary>
    /// Controller responsável pelos pedidos
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="mapper"></param>
    /// <param name="orderService"></param>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/pedidos")]
    public class OrderController(ILogger<OrderController> logger, IMapper mapper, IOrderService orderService) : BaseController(logger, mapper)
    {
        private readonly IOrderService _orderService = orderService;

        /// <summary>
        /// Busca o pedido pelo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Retorna os dados do pedido especificado</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOrderById(int id)
        {
            _logger.LogInformation("Obter os dados do pedido {id}", id);

            var orderResponse = await _orderService.GetByIdAsync(id);

            var response = _mapper.Map<OrderResponseModel>(orderResponse);
            return Ok(response);
        }

        /// <summary>
        /// Busca todos os pedido através do status
        /// </summary>
        /// <param name="status"></param>
        /// <returns>Retorna todos os pedidos do status especificado</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOrderByStatus([FromQuery] OrderStatus status)
        {
            _logger.LogInformation("Obter os pedidos do status {status}", status);

            if (!Enum.IsDefined(typeof(OrderStatus), status))
                return BadRequest("O status informado não é válido.");

            var ordersResponse = await _orderService.GetByStatusAsync(status);

            var response = _mapper.Map<IEnumerable<OrderResponseModel>>(ordersResponse);
            return Ok(response);
        }

        /// <summary>
        /// Adiciona um novo pedido
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Retorna o pedido criado</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddOrder([FromBody] OrderRequestModel request)
        {
            _logger.LogInformation("Criar novo pedido {pedidoId}.", request.OrderId);

            var orderRequest = _mapper.Map<OrderDTO>(request);

            var orderResponse = await _orderService.AddAsync(orderRequest);

            var response = _mapper.Map<OrderCreateResponseModel>(orderResponse);
            return CreatedAtAction(nameof(AddOrder), response);
        }

        /// <summary>
        /// Atualiza um pedido
        /// </summary>
        /// <param name="request"></param>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateOrder([FromRoute] int id, [FromBody] OrderRequestModel request)
        {
            _logger.LogInformation("Atualizar o pedido {id}.", id);

            var orderRequest = _mapper.Map<OrderDTO>(request);

            await _orderService.UpdateAsync(id, orderRequest);

            return NoContent();
        }

        /// <summary>
        /// Excluir o pedido pelo id
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteOrderById(int id)
        {
            _logger.LogInformation("Excluir o pedido {id}", id);

            await _orderService.DeleteAsync(id);

            return NoContent();
        }
    }
}
