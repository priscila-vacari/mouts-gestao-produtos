using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace GestaoProdutos.API.Controllers
{
    /// <summary>
    /// Base controller
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="mapper"></param>
    [ApiController]
    public abstract class BaseController(ILogger<BaseController> logger, IMapper mapper) : ControllerBase
    {
        /// <summary>
        /// Logger
        /// </summary>
        public readonly ILogger<BaseController> _logger = logger;

        /// <summary>
        /// Mapper
        /// </summary>
        public readonly IMapper _mapper = mapper;
    }
}
