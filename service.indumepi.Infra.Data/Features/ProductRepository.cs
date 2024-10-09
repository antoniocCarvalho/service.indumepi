
using Microsoft.Extensions.Logging;
using service.indumepi.Application.Service.ItemRequest;
using service.indumepi.Domain.Aggregates.Item;


namespace service.indumepi.Infra.Data.Features
{
    public class ProductRepository
    {
        private readonly Context _context;
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(Context context, ILogger<ProductRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void DeleteAll()
        {
            _context.Item.RemoveRange(_context.Item.ToList());
            _context.SaveChanges();
        }

        public void SaveProducts(List<Item> products)
        {
            foreach (var product in products)
            {
                var existingProduct = _context.Item
                    .FirstOrDefault(p => p.Codigo == product.Codigo || p.CodigoProduto == product.CodigoProduto);

                if (existingProduct != null)
                {
                    existingProduct.Descricao = product.Descricao;
                    existingProduct.ValorUnitario = product.ValorUnitario;
                    existingProduct.CodigoFamilia = product.CodigoFamilia;
                }
                else
                {
                    _context.Item.Add(product);
                }
            }
            _context.SaveChanges();
        }

        public async Task AtualizarProdutosAsync(ItemService productService)
        {
            try
            {
                DeleteAll();

                var produtos = await productService.ListarProdutosAsync();
                _logger.LogInformation($"Produtos recebidos: {produtos.Count}");


                if (produtos.Any())
                {
                    SaveProducts(produtos);
                    _logger.LogInformation("Produtos inseridos no banco de dados com sucesso.");

                    // Supondo que `produtos` contém informação de total de páginas (adapte conforme necessário)
                    var totalDePaginas = produtos.Count / 50 + 1;

                    // Itera pelas páginas restantes
                    for (int pagina = 2; pagina <= totalDePaginas; pagina++)
                    {
                        var paginaProdutos = await productService.ListarProdutosAsync(pagina);
                        SaveProducts(paginaProdutos);
                        _logger.LogInformation($"Inserção de produtos concluída - Página {pagina}");
                    }
                }
                else
                {
                    _logger.LogWarning("Nenhum produto encontrado.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao atualizar produtos: {ex.Message}");
            }
        }
    }
}