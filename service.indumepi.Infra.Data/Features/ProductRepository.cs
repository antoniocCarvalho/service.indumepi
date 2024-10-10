
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
    }
}