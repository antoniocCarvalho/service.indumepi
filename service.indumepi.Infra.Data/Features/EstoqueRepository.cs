using Microsoft.Extensions.Logging;
using service.indumepi.Domain.Aggregates.Estoque;
using System.Collections.Generic;
using System.Linq;

namespace service.indumepi.Infra.Data.Features
{
    public class EstoqueRepository
    {
        private readonly Context _context;
        private readonly ILogger<EstoqueRepository> _logger;

        public EstoqueRepository(Context context, ILogger<EstoqueRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void DeleteAll()
        {
            _context.Estoque.RemoveRange(_context.Estoque.ToList());
            _context.SaveChanges();
        }

        public void SaveEstoques(List<Estoque> estoques)
        {
            foreach (var estoque in estoques)
            {
                var existingEstoque = _context.Estoque
                    .FirstOrDefault(e => e.NCodProd == estoque.NCodProd || e.CCodigo == estoque.CCodigo);

                if (existingEstoque != null)
                {
                    existingEstoque.CDescricao = estoque.CDescricao;
                    existingEstoque.CodigoLocalEstoque = estoque.CodigoLocalEstoque;
                    existingEstoque.EstoqueMinimo = estoque.EstoqueMinimo;
                    existingEstoque.Fisico = estoque.Fisico;
                    existingEstoque.NCMC = estoque.NCMC;
                    existingEstoque.NPendente = estoque.NPendente;
                    existingEstoque.NPrecoUnitario = estoque.NPrecoUnitario;
                    existingEstoque.NSaldo = estoque.NSaldo;
                    existingEstoque.Reservado = estoque.Reservado;
                }
                else
                {
                    _context.Estoque.Add(estoque);
                }
            }
            _context.SaveChanges();
        }

        public List<Estoque> ListAll()
        {
            return _context.Estoque.ToList();
        }

        public Estoque GetByNCodProd(long nCodProd)
        {
            return _context.Estoque
                .FirstOrDefault(e => e.NCodProd == nCodProd);
        }
    }
}