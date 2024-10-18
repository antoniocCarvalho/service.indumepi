using Microsoft.AspNetCore.Mvc;
using service.indumepi.Application.Service.ClientRequest;
using service.indumepi.Application.Service.FamilyRequest;
using service.indumepi.Infra.Data.Features;

namespace service.indumepi.API.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class FamiliesController : ControllerBase
    {
        private readonly FamilyService _familyService;
        private readonly FamilyRepository _familyRepository;


        public FamiliesController(FamilyService familyService, FamilyRepository familyRepository)
        {
            _familyService = familyService;
            _familyRepository = familyRepository;
        }

        [HttpGet("families")]
        public async Task<IActionResult> ListarFamilies()
        {
            var familias = await _familyService.ListarTodasAsFamiliasAsync();
            if (familias.Any())
            {
                _familyRepository.DeleteAll();
                _familyRepository.SaveFamilies(familias);
                return Ok(familias);
            }
            else
            {
                return NotFound("Nenhuma Familie encontrado na API Omie.");
            }
        }
    }
}
