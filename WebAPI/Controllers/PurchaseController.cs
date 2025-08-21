using Microsoft.AspNetCore.Mvc;
using ClassLibraryProject.Entities;
using RepositoryLibraryProject.Interfaces;
using AutoMapper;
using System.Collections.Generic;
using WebAPI.Dtos.Purchase;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurchaseController : ControllerBase
    {
        private readonly IRepository<Purchase> _purchaseRepository;
        private readonly IMapper _mapper;
        public PurchaseController(IRepository<Purchase> purchaseRepository, IMapper mapper)
        {
            _purchaseRepository = purchaseRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PurchaseBaseDto>> GetAll()
        {
            var purchases = _purchaseRepository.GetAll();
            var dtos = _mapper.Map<IEnumerable<PurchaseBaseDto>>(purchases);
            return Ok(dtos);
        }

        [HttpPost]
        public ActionResult<PurchaseBaseDto> Create(PurchaseCreatorDto dto)
        {
            var purchase = _mapper.Map<Purchase>(dto);
            purchase.Id = Guid.NewGuid().ToString();
            _purchaseRepository.Add(purchase);
            var result = _mapper.Map<PurchaseBaseDto>(purchase);
            return CreatedAtAction(nameof(GetAll), new { id = purchase.Id }, result);
        }
    }
}
