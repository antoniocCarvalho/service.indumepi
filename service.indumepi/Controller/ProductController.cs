﻿using Microsoft.AspNetCore.Mvc;
using service.indumepi.Application.Service.ItemRequest;
using service.indumepi.Infra.Data.Features;

namespace service.indumepi.API.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ItemService _itemService;
        private readonly ProductRepository _productRepository;


        public ProductController(ItemService itemService, ProductRepository productRepository)
        {
            _itemService = itemService;
            _productRepository = productRepository;
        }

        [HttpGet("listar")]
        public async Task<IActionResult> ListarProdutos()
        {
            var produtos = await _itemService.ListarTodosOsProdutosAsync();
            if (produtos.Any())
            {
                _productRepository.DeleteAll();
                _productRepository.SaveProducts(produtos);
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
            var produtos =  _productRepository.ListAll(); ;
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
