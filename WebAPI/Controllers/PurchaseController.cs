using Microsoft.AspNetCore.Mvc;
using ClassLibraryProject.Entities;
using RepositoryLibraryProject.Interfaces;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Dtos.Purchase;
using ServiceLibraryProject;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurchaseController : ControllerBase
    {
        private readonly PurchaseService _purchaseService;
        private readonly IMapper _mapper;
        public PurchaseController(PurchaseService purchaseService, IMapper mapper)
        {
            _purchaseService = purchaseService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PurchaseBaseDto>>> GetAll()
        {
            var purchases = await _purchaseService.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<PurchaseBaseDto>>(purchases);
            return Ok(dtos);
        }

        [HttpPost]
        public async Task<ActionResult<PurchaseBaseDto>> Create(PurchaseCreatorDto dto)
        {
            var purchase = _mapper.Map<Purchase>(dto);
            purchase.Id = Guid.NewGuid().ToString();
            await _purchaseService.AddAsync(purchase);
            var result = _mapper.Map<PurchaseBaseDto>(purchase);
            return CreatedAtAction(nameof(GetAll), new { id = purchase.Id }, result);
        }
    }
}
