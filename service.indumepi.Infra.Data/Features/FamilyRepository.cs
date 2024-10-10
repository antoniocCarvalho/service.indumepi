using Microsoft.Extensions.Logging;
using service.indumepi.Application.Service;
using service.indumepi.Application.Service.FamilyRequest;
using service.indumepi.Domain.Aggregates;
using service.indumepi.Domain.Aggregates.Family;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace service.indumepi.Infra.Data.Features
{
    public class FamilyRepository
    {
        private readonly Context _context;
        private readonly ILogger<FamilyRepository> _logger;

        public FamilyRepository(Context context, ILogger<FamilyRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void DeleteAll()
        {
            _context.Families.RemoveRange(_context.Families.ToList());
            _context.SaveChanges();
        }

        public void SaveFamilies(List<Family> families)
        {
            foreach (var family in families)
            {
                var existingFamily = _context.Families
                    .FirstOrDefault(p => p.CodFamilia == family.CodFamilia || p.Codigo == family.Codigo);

                if (existingFamily != null)
                {
                    existingFamily.NomeFamilia = family.NomeFamilia;
                    existingFamily.Inativo = family.Inativo;
                }
                else
                {
                    _context.Families.Add(family);
                }
            }
            _context.SaveChanges();
        }

    }
}
