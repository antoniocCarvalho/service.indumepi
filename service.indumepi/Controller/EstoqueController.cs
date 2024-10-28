using global::service.indumepi.Infra.Data.Features;
using Microsoft.AspNetCore.Mvc;
using service.indumepi.Application.Service.EstoqueRequest;
using service.indumepi.Infra.Data.Features;

namespace service.indumepi.API.Controller
{
    [ApiController]
        [Route("api/[controller]")]
        public class EstoqueController : ControllerBase
        {
            private readonly EstoqueService _estoqueService;
            private readonly EstoqueRepository _productRepository;


            public EstoqueController(EstoqueService estoqueService, EstoqueRepository productRepository)
            {
                _estoqueService = estoqueService;
                _productRepository = productRepository;
            }

            [HttpGet("listar")]
            public async Task<IActionResult> ListarProdutos()
            {
                var produtos = await _estoqueService.ListarTodosOsProdutosAsync();
                if (produtos.Any())
                {
                _productRepository.DeleteAll();
                _productRepository.SaveEstoques(produtos);
                return Ok(produtos);
                }
                else
                {
                    return NotFound("Nenhum produto encontrado na API Omie.");
                }
            }


            [HttpGet("listar/front")]
            public async Task<IActionResult> ListarProdutosFront()
            {
                var produtos = _productRepository.ListAll(); ;
                if (produtos.Any())
                {
                    return Ok(produtos);
                }
                else
                {
                    return NotFound("Nenhum produto encontrado na API Omie.");
                }
            }

        }
    }


